using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Microsoft.Maui.Devices.Sensors;
using PropertyChanged;
using SimpleSkies.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimpleSkies.MVVM.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class MainViewModel
    {
        #region Private Fields
        private HttpClient client;
        #endregion
        #region Properties
        //Object of model to hold return data from API
        public WeatherData WeatherData { get; set; }
        public string PlaceName { get; set; }
        public DateTime Date {  get; set; } = DateTime.Now;
        //private IGeolocation Geolocation { get; set; }
        public bool IsLoading { get; set; }
        public bool IsVisible { get; set; }
        private CancellationTokenSource _cancelTokenSource;
        #endregion

        #region Commands & Tasks
        //Command for searchBar. Saves coordinates of city to a variable through GetCoordinates task, then gives that variable to the GetWeather task.
        public ICommand SearchCommand => new Command(async (searchText) =>
        {
            //PlaceName = searchText.ToString();
            var location = 
                await GetCoordinatesAsync(searchText.ToString());
            try
            {
                if (location != null)
                {
                    var places = await Geocoding.GetPlacemarksAsync(location);
                    var place = places.FirstOrDefault();

                    var locationString = place?.Locality;
                if (string.IsNullOrWhiteSpace(locationString)) 
                    {
                        locationString = place?.CountryName ?? "Fuck Knows...";
                    }
                    PlaceName = locationString;
                    await GetWeatherAsync(location);
                }

                else
                {
                    HandleLocationNotFound();
                }

            }
            catch (Exception ex)
            {
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                string text = "Sorry, search failed!";
                ToastDuration duration = ToastDuration.Long;
                double fontSize = 14;
                var toast = Toast.Make(text, duration, fontSize);
                Console.WriteLine("****The failing source was: {0}, the string was: {1}, message was: {2}. Callstack: {3}****", 
                    ex.Source, ex.ToString(), ex.Message, ex.StackTrace);
                await toast.Show(cancellationTokenSource.Token);
            }

        });
        
        //Creates IEnumerable to iterate through a group of locations and return the first item.
        //Locations are acquired through Geocoding class with method GetLocationsAsync which takes a string parameter address.
        private async Task<Location?> GetCoordinatesAsync(string? address)
        {
            IEnumerable<Location> locations = await Geocoding.GetLocationsAsync(address);

            try
            {
                Location? location = locations.FirstOrDefault();

                if (location != null)
                {
                    Console.WriteLine("Latitude: {0}, Longitude: {1}, Altitude: {2}", location.Latitude, location.Longitude, location.Altitude);

                    return location;
                }

                else
                {
                    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                    string text = "Sorry, I can't retrieve the coordinates!";
                    ToastDuration duration = ToastDuration.Long;
                    double fontSize = 14;
                    var toast = Toast.Make(text, duration, fontSize);
                    await toast.Show(cancellationTokenSource.Token);

                    return null;
                }

            }
            catch (Exception)
            {
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                string text = "Something went wrong! Try again later.";
                ToastDuration duration = ToastDuration.Long;
                double fontSize = 14;
                var toast = Toast.Make(text, duration, fontSize);
                await toast.Show(cancellationTokenSource.Token);

                return null; ;

            }
        }

        // Creates URL string and a variable to hold the response of a GET request. 
        private async Task GetWeatherAsync(Location location)
        {
            var url = $"https://api.open-meteo.com/v1/forecast?latitude={location.Latitude}&longitude={location.Longitude}&current=temperature_2m,weather_code,wind_speed_10m,wind_direction_10m&daily=weather_code,temperature_2m_max,temperature_2m_min&timezone=Pacific%2FAuckland";
            IsLoading = true;
            var response = await client.GetAsync(url);

            try
            {
                if (response.IsSuccessStatusCode)
                {
                    //If the status code returns successful, we use a variable to save the response, which is the content of the response read as a stream.
                    using (var responseStream = await response.Content.ReadAsStreamAsync())
                    {
                        //A new variable is made to hold the data as a WeatherData object, which is assigned from the Deserialized responseStream variable.
                        var data = 
                            await JsonSerializer
                            .DeserializeAsync<WeatherData>(responseStream);

                        //Then we assign the data object to the WeatherData object.
                        WeatherData = data;

                        //Here we we populate the weeks forecast cards with data from ther weatherdata object.
                        //The code reads each item in the daily object of weather data inside an if loop, and saves them into an object of Daily2.
                        //Displayed through collectionview bindings in the front end.
                        for (int i = 0; i < WeatherData.daily.time.Length; i++)
                        {
                            var weekday = new Daily2
                            {
                                time = WeatherData.daily.time[i],
                                temperature_2m_max = WeatherData.daily.temperature_2m_max[i],
                                temperature_2m_min = WeatherData.daily.temperature_2m_min[i],
                                weather_code = WeatherData.daily.weather_code[i]
                            };

                            WeatherData.daily2.Add(weekday);
                        }
                    }

                    IsLoading = false;
                    IsVisible = true;
                }

                else
                {
                    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                    string text = "Could'nt find that Location! Try again later.";
                    ToastDuration duration = ToastDuration.Long;
                    double fontSize = 14;
                    var toast = Toast.Make(text, duration, fontSize);
                    await toast.Show(cancellationTokenSource.Token);

                    IsLoading = false;
                    IsVisible = true;

                }

                

            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public async Task InitializeAsync()
        {
            try
            {
                // Request location permissions
                var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

                if (status != PermissionStatus.Granted)
                {
                    // Notify the user that permission is needed and return
                    await NotifyPermissionNeeded();
                    return;
                }

                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                _cancelTokenSource = new CancellationTokenSource();
                var deviceLocation = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);
                
                if (deviceLocation != null)
                {
                    var places = await Geocoding.GetPlacemarksAsync(deviceLocation);
                    var place = places?.FirstOrDefault();

                    var locationString = place?.Locality;
                    if (string.IsNullOrWhiteSpace(locationString))
                    {
                        locationString = place?.CountryName ?? "Unkown Location";
                    }

                    Console.WriteLine($"Latitude: {deviceLocation.Latitude}, Longitude: {deviceLocation.Longitude}, Altitude: {deviceLocation.Altitude}");
                    PlaceName = locationString;

                    await GetWeatherAsync(deviceLocation); 
                }
                else
                {
                    HandleLocationNotFound();
                }
            }
            catch (Exception ex)
            {
                HandleLocationNotFound();
                HandleException(ex);
            }
        }

        private async Task NotifyPermissionNeeded()
        {
            // Notify the user that location permission is required
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            string text = "Location permission is required to display weather information. Please enable location services in your settings.";
            ToastDuration duration = ToastDuration.Long;
            double fontSize = 14;
            var toast = Toast.Make(text, duration, fontSize);
            await toast.Show(cancellationTokenSource.Token);
        }

        private async void HandleLocationNotFound()
        {
            
            Location? location = await GetCoordinatesAsync("Africa");
            await GetWeatherAsync(location);
            PlaceName = "Africa";

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            string text = "Sorry, that location can't be found. Meanwhile, In Africa...";
            ToastDuration duration = ToastDuration.Long;
            double fontSize = 14;
            var toast = Toast.Make(text, duration, fontSize);
            await toast.Show(cancellationTokenSource.Token);
        }

        private async void HandleException(Exception ex)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            string text = "Error Loading!";
            ToastDuration duration = ToastDuration.Long;
            Console.WriteLine("*****Exception: " + ex.Message + ": " + ex.StackTrace);
            double fontSize = 14;
            var toast = Toast.Make(text, duration, fontSize);
            await toast.Show(cancellationTokenSource.Token);
        }

        #endregion

        #region Constructor
        public MainViewModel() 
        {
            client = new HttpClient();
        }
        #endregion

       
    }
}
