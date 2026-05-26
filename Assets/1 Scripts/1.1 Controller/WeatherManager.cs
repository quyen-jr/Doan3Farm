using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine.UI;

public class Weather : MonoBehaviour
{
    public Sprite[] weatherSprites;
    public Image targetImage;
    void Start()
    {
        StartCoroutine(GetRequest("https://api.weatherapi.com/v1/current.json?key=be7649f2ab9d456896080535240310&q=Ho Chi Minh&aqi=no"));
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                HandleWeatherData(webRequest.downloadHandler.text);
            }
        }
    }
    public void HandleWeatherData(string data){
        WeatherData weatherData = JsonConvert.DeserializeObject<WeatherData>(data);
        //Debug.Log(weatherData.current.condition.text);
        //Debug.Log(weatherData.current.condition.icon);
        switch(weatherData.current.condition.text){
            case "Sunny":
                targetImage.sprite = weatherSprites[0];
                break;
            case "Patchy rain nearby":
                targetImage.sprite = weatherSprites[1];
                break;
            case "Patchy light rain with thunder":
                targetImage.sprite = weatherSprites[2];
                break;
            case "Partly Cloudy":
                targetImage.sprite = weatherSprites[3];
                break;
            case "Moderate rain":
                targetImage.sprite = weatherSprites[4];
                break;
            case "Moderate or heavy rain with thunder":
                targetImage.sprite = weatherSprites[5];
                break;
        }
    }
    
}
public class WeatherCondition
{
    public string text { get; set; }
    public string icon { get; set; }
    public int code { get; set; }
}

public class CurrentWeather
{
    public float temp_c { get; set; }
    public float temp_f { get; set; }
    public WeatherCondition condition { get; set; }
    public float wind_kph { get; set; }
    public float pressure_mb { get; set; }
    public float humidity { get; set; }
    public float feelslike_c { get; set; }
    public float vis_km { get; set; }
}

public class Location
{
    public string name { get; set; }
    public string country { get; set; }
    public string localtime { get; set; }
}

public class WeatherData
{
    public Location location { get; set; }
    public CurrentWeather current { get; set; }
}