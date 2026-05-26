using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherClassify : MonoBehaviour
{
    public enum WeatherType {
        Clear,
        Rain,
        Cloud,
        Thunder
    }
    public WeatherType ClassifyWeather(string weather){
        switch(weather){
            case "Sunny":
            case "Clear":
                return WeatherType.Clear;
            
            case "Patchy rain possible":
            case "Patchy light rain":
            case "Light rain":
            case "Moderate rain at times":
            case "Moderate rain":
            case "Heavy rain at times":
            case "Heavy rain":
            case "Light rain shower":
            case "Moderate or heavy rain shower":
            case "Torrential rain shower":
            case "Freezing drizzle":
            case "Light freezing rain":
            case "Moderate or heavy freezing rain":
                return WeatherType.Rain;
            
            case "Partly cloudy":
            case "Cloudy":
            case "Overcast":
            case "Mist":
            case "Patchy light drizzle":
            case "Light drizzle":
            case "Patchy freezing drizzle possible":
                return WeatherType.Cloud;

            case "Thundery outbreaks possible":
            case "Patchy light rain with thunder":
            case "Moderate or heavy rain with thunder":
            case "Patchy light snow with thunder":
            case "Moderate or heavy snow with thunder":
                return WeatherType.Thunder;
        }
        
        return WeatherType.Clear;
    }
}
