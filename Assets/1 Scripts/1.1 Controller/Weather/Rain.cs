using UnityEngine;

public class Rain : MonoBehaviour, IWeatherState
{
    public static Rain Instance;
    private void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    public void OnEnter()
    {
        WeatherManger.Instance.SetLightIntensity(0.5f);
        WeatherManger.Instance.ToogleRain(true);
        WeatherManger.Instance.SetFogIntensity(0.02f);
    }

    public void OnLeave()
    {
        WeatherManger.Instance.ToogleRain(false);
        WeatherManger.Instance.SetFogIntensity(0);
    }

    public void OnUpdate()
    {
        
    }
}