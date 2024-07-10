<h1>Simple Skies - A Minimilast, Aesthetic, Simple Weather App</h1>

<h2>Get it on the Google Play Store here!</h2>

[Get it from the play store here!](https://play.google.com/store/apps/details?id=com.renosapps.simpleskies&fbclid=IwZXh0bgNhZW0CMTAAAR3wJ4VxjRoL7SsfKbOVnAPqB6LetLV-N2HlOPWKlpJDJetEYsQxS5IT4J8_aem__opdxU7UceRx5XKbHuvyqQ)<br/>
<img src="https://i.imgur.com/0gQQzP1.jpeg" alt="Landing Page" width="100%">

<h2>Description</h2>
I built simple skies to practice consuming REST APIs in .NET MAUI. 

The app consists of a home screen which loads to the city the user is in, displaying the current weather for the hour, and the next 7 days of weather are displayed in a horizontal collection at the bottom of the screen.<br/><br/>
Users can then use the search bar to search for a city or a country. If a location cant be found, a message displays to the user and the weather location is moved to africa.<br/><br/>
The aim of this app was to provide a weather app that displays only key information about the weather for the current day and forecast, which allows the app to provide a clean, aesthetic minimalist design, whilst<br/>
serving its primary funtion to deliver important weather information to the user.<br/><br/>

To get a location, the app takes the string that the user enters, and puts it through a Geocoding method that takes the parameter and measures it up against a list of locations as placemarks. If a placemark matches the entry, the method returns the name & coordinates.<br/>
If a location doesnt have a city name, it will look for the name of the country that location is in and return that with the coordinates, otherwise, as stated before, if a location cant be found the app defaults to Africa with an error message.<br/><br/>

This app was a lot of fun to make, and I learned a lot through the process of working with the APIs, databinding the returned values, utilizing Lottie animation and more!

<br />


<h2>Languages/Tech Stack Used</h2>

- <b>C#</b> 
- <b>XAML</b>
- <b>.NET MAUI</b>
- <b>Open-Meteo.com (Weather APIs)</b>

<h2>Nuget Packages</h2>

  - <b>PropertyChanged.Fody</b>
  - <b>CommunityToolkit.Maui</b>
  - <b>SkiaSkarp.Extended.UI.Maui/b>
  - <b>Xamarin.AndroidX.Lifecylce.LiveData</b>

<h2>Environments Used </h2>

- <b>Visual Studio</b>
- <b>Debugged on Android Devices</b>

<h2>App Screenshots/Visual Walkthrough:</h2>

<p align="center">
  <table>
    <tr>
      <td align="center" valign="top" width="25%">
        <b>Home Page Light</b><br>
        <img src="https://i.imgur.com/17SvA0a.jpeg" alt="Home Page, Loaded Location" width="100%">
      </td>
      <td align="center" valign="top" width="25%">
        <b>Home Page Dark</b><br>
        <img src="https://i.imgur.com/3pfSgBS.jpeg" alt="Home Page Dark Mode, Loaded Location" width="100%">
      </td>
      <td align="center" valign="top" width="25%">
        <b>Trail Guide Page</b><br>
        <img src="https://i.imgur.com/SvTbLGh.jpeg" alt="LA Search, Light" width="100%">
      </td>
      <td align="center" valign="top" width="25%">
        <b>Plant Collection Page</b><br>
        <img src="https://i.imgur.com/9bDDxJk.jpeg" alt="NZ Search, Dark" width="100%">
      </td>
    </tr>
    
  </table>
</p>
