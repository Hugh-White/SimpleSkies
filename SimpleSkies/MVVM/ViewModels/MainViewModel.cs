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

        #endregion

        #region Commands & Tasks
        //Command for searchBar. Saves coordinates of city to a variable through GetCoordinates task, then gives that variable to the GetWeather task.
        public ICommand SearchCommand => new Command(async (searchText) =>
        {
            PlaceName = searchText.ToString();
            var location = 
                await GetCoordinatesAsync(searchText.ToString());
            await GetWeatherAsync(location);
        });
        
        //Creates IEnumerable to iterate through a group of locations and return the first item.
        //Locations are acquired through Geocoding class with method GetLocationsAsync which takes a string parameter address.
        private async Task<Location> GetCoordinatesAsync(string address)
        {
            IEnumerable<Location> locations = await Geocoding.GetLocationsAsync(address);

            Location location = locations?.FirstOrDefault();

            if (location != null)
            {
                Console.WriteLine("Latitude: {0}, Longitude: {1}, Altitude: {2}", location.Latitude, location.Longitude, location.Altitude);
            }

            return location;
        }

        // Creates URL string and a variable to hold the response of a GET request. 
        private async Task GetWeatherAsync(Location location)
        {
            var url = $"https://api.open-meteo.com/v1/forecast?latitude={location.Latitude}&longitude={location.Longitude}&current=temperature_2m,weather_code,wind_speed_10m,wind_direction_10m&daily=weather_code,temperature_2m_max,temperature_2m_min&timezone=Pacific%2FAuckland";
            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                //If the status code returns successful, we use a variable to save the response, which is the content of the response read as a stream.
                using (var responseStream = await response.Content.ReadAsStreamAsync())
                {
                    //A new variable is made to hold the data as a WeatherData object, which is assigned from the Deserialized responseStream variable.
                    var data = 
                        await JsonSerializer
                        .DeserializeAsync<WeatherData>(responseStream);

                    //Lastly we assign the data object to the WeatherData object.
                    WeatherData = data;
                }
            }
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
