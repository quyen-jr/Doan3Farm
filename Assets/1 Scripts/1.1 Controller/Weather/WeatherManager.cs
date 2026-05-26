using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine.UI;
using System;
using TMPro;

public class WeatherManger : MonoBehaviour
{
    public static WeatherManger Instance;
    public enum WeatherState {
        Sunny,
        Cloudy,
        Rain,
        Thunder
    }
    public Sprite[] weatherSprites;
    public Image targetImage;
    public Light mainLight;
    public LightingSettings lightingSettings;
    public float transitionTime;
    public GameObject RainFx;
    public GameObject RainFX2;

    public float _targetLightIntensity;
    public float _targetFogIntensity;
    private WeatherState _currentWeatherState;
    private IWeatherState _currentWeather;
    private WeatherClassify weatherClassify;
    private WeatherClassify.WeatherType _currentWeatherType;

    [Header("Weather UI")]
    [SerializeField] private TextMeshProUGUI calenderText;
    [SerializeField] private TextMeshProUGUI temperatureText;

    private float timeElapsed = 0f; 
    private float updateInterval = 120f; 

    private void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        weatherClassify = GetComponent<WeatherClassify>();
    }
    void Start()
    {
        StartCoroutine(GetRequest("https://api.weatherapi.com/v1/current.json?key=be7649f2ab9d456896080535240310&q=Don Duong&aqi=no"));
        _currentWeatherState = WeatherState.Rain;
        ChangeWeather(WeatherState.Sunny);
    }
    private void Update() {
        if(Mathf.Abs(mainLight.intensity - _targetLightIntensity) > 0.01f){
            mainLight.intensity = Mathf.Lerp(mainLight.intensity, _targetLightIntensity, Time.deltaTime / transitionTime);
        }

        if(Mathf.Abs(RenderSettings.fogDensity - _targetFogIntensity) > 0.001f){
            RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, _targetFogIntensity, Time.deltaTime / (transitionTime * 2));
        }
        timeElapsed += Time.deltaTime; // Cộng thêm thời gian đã trôi qua
        //if (timeElapsed >= updateInterval)
        //{
        //    timeElapsed = 0f; // Reset lại thời gian đã trôi qua
        //    StartCoroutine(GetRequest("https://api.weatherapi.com/v1/current.json?key=be7649f2ab9d456896080535240310&q=Don Duong&aqi=no"));
        //}


        _currentWeather.OnUpdate();
    }
    public void ChangeWeather(WeatherState state){
        if(_currentWeatherState != state){
            if(_currentWeather != null)
                _currentWeather.OnLeave();

            if(state == WeatherState.Sunny){
                _currentWeather = Sunny.Instance;
            } else if(state == WeatherState.Rain){
                _currentWeather = Rain.Instance;
            } else if(state == WeatherState.Cloudy){
                _currentWeather = Cloudy.Instance;
            } else if(state == WeatherState.Thunder){
                _currentWeather = Thunder.Instance;
            }

            _currentWeatherState = state;
            _currentWeather.OnEnter();
        }
        
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
        Debug.Log(data);
        WeatherData weatherData = JsonConvert.DeserializeObject<WeatherData>(data);
        HandleDateAndTemperatureDate(weatherData);
        Debug.Log(weatherData.current.condition.text);
        // Debug.Log(weatherData.current.condition.icon);
        // Debug.Log(weatherData.location.localtime);
        // Debug.Log(weatherData.current.temp_c);
        DateManager.Instance.SetDate(weatherData.location.localtime.Split(" ")[0]);
        TemperatureManager.Instance.SetTemperature(weatherData.current.temp_c.ToString());

        //switch(weatherData.current.condition.text){
        //    case "Sunny":
        //        targetImage.sprite = weatherSprites[0];
        //        break;
        //    case "Patchy rain nearby":
        //        targetImage.sprite = weatherSprites[1];
        //        break;
        //    case "Patchy light rain with thunder":
        //        targetImage.sprite = weatherSprites[2];
        //        break;
        //    case "Partly cloudy":
        //        targetImage.sprite = weatherSprites[3];
        //        break;
        //    case "Moderate rain":
        //        targetImage.sprite = weatherSprites[4];
        //        break;
        //    case "Moderate or heavy rain with thunder":
        //        targetImage.sprite = weatherSprites[5];
        //        break;
        //}

        if(weatherClassify.ClassifyWeather(weatherData.current.condition.text) != _currentWeatherType){
            _currentWeatherType = weatherClassify.ClassifyWeather(weatherData.current.condition.text);

            if(_currentWeatherType == WeatherClassify.WeatherType.Clear){
               // targetImage.sprite = weatherSprites[0];
                ChangeWeather(0);
            } else if(_currentWeatherType == WeatherClassify.WeatherType.Cloud){
                //targetImage.sprite = weatherSprites[3];
                ChangeWeather(2);
            } else if(_currentWeatherType == WeatherClassify.WeatherType.Rain){
             //   targetImage.sprite = weatherSprites[1];
                ChangeWeather(4);
            } else if(_currentWeatherType == WeatherClassify.WeatherType.Thunder){
              //  targetImage.sprite = weatherSprites[2];
                ChangeWeather(7);
            }
        }
        
    }
    public void HandleDateAndTemperatureDate(WeatherData _weatherData)
    {
        WeatherData weatherData = _weatherData;
        if(calenderText==null|| temperatureText==null) return;
        string localTime = weatherData.location.localtime; 
        string[] dateParts = localTime.Split(" ")[0].Split("-"); 
        int day = int.Parse(dateParts[2]);
        int month = int.Parse(dateParts[1]); 

        string monthName = GetMonthName(month); 


        calenderText.text=day+" "+monthName;
        temperatureText.text = weatherData.current.temp_c.ToString();
        //Debug.Log($"{day} {monthName}");

        //Debug.Log(weatherData.current.temp_c.ToString());
    }
    public void ChangeWeather(int w)
        {
            if(Enviro.EnviroManager.instance.Weather != null)
            {
                if(Enviro.EnviroManager.instance.Weather.Settings.weatherTypes.Count >= w)
                   Enviro.EnviroManager.instance.Weather.ChangeWeather(Enviro.EnviroManager.instance.Weather.Settings.weatherTypes[w]);
            }
        }

    public void SetLightIntensity(float intensity) =>  _targetLightIntensity = intensity;
    public void SetFogIntensity(float intensity) => _targetFogIntensity = intensity;
    public void Weather_Sunny() => ChangeWeather(WeatherState.Sunny);
    public void Weather_Rain() => ChangeWeather(WeatherState.Rain);
    public void Weather_Cloudy() => ChangeWeather(WeatherState.Cloudy);
    public void Weather_Thunder() => ChangeWeather(WeatherState.Thunder);
    public void ToogleRain(bool toogle) => RainFx.SetActive(toogle);
    public void ToogleRain2(bool toogle) => RainFX2.SetActive(toogle);
    private string GetMonthName(int month)
    {
        switch (month)
        {
            case 1: return "Jan";
            case 2: return "Feb";
            case 3: return "Mar";
            case 4: return "Apr";
            case 5: return "May";
            case 6: return "Jun";
            case 7: return "Jul";
            case 8: return "Aug";
            case 9: return "Sep";
            case 10: return "Oct"; 
            case 11: return "Nov";
            case 12: return "Dec";
            default: return "";
        }
    }

}






