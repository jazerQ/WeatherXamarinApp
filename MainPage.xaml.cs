using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Globalization;
using System.Net;
using System.IO;
using Xamarin.Essentials;
using System.Collections.Specialized;
using System.Net.NetworkInformation;

namespace weatherApp
{
    public class IpInfo
    {
        [JsonProperty("ip")]
        public string Ip { get; set; }

        [JsonProperty("hostname")]
        public string Hostname { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }
        [JsonProperty("loc")]
        public string Loc { get; set; }
        [JsonProperty("org")]
        public string Org { get; set; }
        [JsonProperty("postal")]
        public string Postal { get; set; }
    }

    public partial class MainPage : ContentPage
    {
        const string API = "190e45095e74b5804d8d36b814c299b5";
        public MainPage()
        {
            InitializeComponent();
        }
        private async void Begin(object sender, EventArgs e)
        {
            string url1 = "http://checkip.dyndns.org";
            System.Net.WebRequest req = System.Net.WebRequest.Create(url1);
            System.Net.WebResponse resp = req.GetResponse();
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            string response = sr.ReadToEnd().Trim();
            string[] ipAddressWithText = response.Split(':');
            string ipAddressWithHTMLEnd = ipAddressWithText[1].Substring(1);
            string[] ipAddress = ipAddressWithHTMLEnd.Split('<');
            string IP = ipAddress[0];
            string URL = "http://api.ipstack.com/" + IP + "?access_key=3abab0bc54dfd360da2a3cedff386c49";
            HttpClient client1 = new HttpClient();
            string answer1 = await client1.GetStringAsync(URL);
            var json1 = JObject.Parse(answer1);
            string geo = json1["city"].ToString();
            GPS.Text = $"Вы сейчас в городе {geo}";
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={geo}&appid={API}&units=metric";
            string answer = await client1.GetStringAsync(url);
            var json = JObject.Parse(answer);
            string temperature = json["main"]["temp"].ToString();
            TempLable.Text = $"Погода сейчас: {temperature}°";

            string feeling = json["main"]["feels_like"].ToString();
            FeelLable.Text = $"Ощущается как: {feeling}°";

            string min_temp = json["main"]["temp_min"].ToString();
            Min_TempLable.Text = $"Минимальная температура сегодня: {min_temp}°";

            string max_temp = json["main"]["temp_max"].ToString();
            Max_TempLable.Text = $"Максимальная температура сегодня: {max_temp}°";

            string wind_speed = json["wind"]["speed"].ToString();
            WindLable.Text = $"Скорость ветра: {wind_speed}°";
            string weather = json["weather"][0]["main"].ToString();

            if (weather == "Clouds")
            {
                ImageWeather.Source = "https://icons.iconarchive.com/icons/oxygen-icons.org/oxygen/256/Status-weather-many-clouds-icon.png";
                weather = "Облачно";

            }
            else if (weather == "Snow")
            {
                ImageWeather.Source = "https://cdn-icons-png.flaticon.com/512/1779/1779932.png";
                weather = "Снег";
            }
            else if (weather == "Clear")
            {
                ImageWeather.Source = "https://icons.iconarchive.com/icons/oxygen-icons.org/oxygen/256/Status-weather-clear-icon.png";
                weather = "Ясно";
            }
            else if (weather == "Rain")
            {
                ImageWeather.Source = "https://icons.iconarchive.com/icons/oxygen-icons.org/oxygen/256/Status-weather-snow-icon.png";
                weather = "Дождь";
            }
            else if (weather == "Mist")
            {
                ImageWeather.Source = "https://icons.iconarchive.com/icons/oxygen-icons.org/oxygen/256/Status-weather-snow-icon.png";
                weather = "Туман";

            }
            else if (weather == "Drizzle")
            {
                ImageWeather.Source = "https://icons.iconarchive.com/icons/oxygen-icons.org/oxygen/256/Status-weather-snow-icon.png";
                weather = "Моросит";

            }
            Weather.Text = $"{weather}";
        }
        private async void GetWeather_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(userInput.Text) || userInput.Text.Length <= 2)
            {
                await DisplayAlert("Ошибка", "Я не знаю городов с таким названием", "ОК");
                return;
            }
            else
            {
                string city = userInput.Text.Trim().ToLower();
                HttpClient client = new HttpClient();
                string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={API}&units=metric";
                try
                {
                    string answer = await client.GetStringAsync(url);
                    var json = JObject.Parse(answer);
                    string temperature = json["main"]["temp"].ToString();
                    TempLable.Text = $"Погода сейчас: {temperature}°";

                    string feeling = json["main"]["feels_like"].ToString();
                    FeelLable.Text = $"Ощущается как: {feeling}°";

                    string min_temp = json["main"]["temp_min"].ToString();
                    Min_TempLable.Text = $"Минимальная температура сегодня: {min_temp}°";

                    string max_temp = json["main"]["temp_max"].ToString();
                    Max_TempLable.Text = $"Максимальная температура сегодня: {max_temp}°";

                    string wind_speed = json["wind"]["speed"].ToString();
                    WindLable.Text = $"Скорость ветра: {wind_speed}°";

                    string weather = json["weather"][0]["main"].ToString();
                    if (weather == "Clouds")
                    {
                        ImageWeather.Source = "https://icons.iconarchive.com/icons/oxygen-icons.org/oxygen/256/Status-weather-many-clouds-icon.png";
                        weather = "Облачно";

                    }
                    else if (weather == "Snow")
                    {
                        ImageWeather.Source = "https://cdn-icons-png.flaticon.com/512/1779/1779932.png";
                        weather = "Снег";
                    }
                    else if (weather == "Clear")
                    {
                        ImageWeather.Source = "https://icons.iconarchive.com/icons/oxygen-icons.org/oxygen/256/Status-weather-clear-icon.png";
                        weather = "Ясно";
                    }
                    else if (weather == "Rain")
                    {
                        ImageWeather.Source = "https://icons.iconarchive.com/icons/oxygen-icons.org/oxygen/256/Status-weather-snow-icon.png";
                        weather = "Дождь";
                    }
                    else if (weather == "Mist")
                    {
                        ImageWeather.Source = "https://icons.iconarchive.com/icons/oxygen-icons.org/oxygen/256/Status-weather-snow-icon.png";
                        weather = "Туман";

                    }
                    else if (weather == "Drizzle")
                    {
                        ImageWeather.Source = "https://icons.iconarchive.com/icons/oxygen-icons.org/oxygen/256/Status-weather-snow-icon.png";
                        weather = "Моросит";

                    }
                    Weather.Text = $"{weather}";
                }
                catch (Exception ex) { await DisplayAlert("Ошибка", "Я не знаю городов с таким названием", "ОК"); }
            }

        }



    }
}
