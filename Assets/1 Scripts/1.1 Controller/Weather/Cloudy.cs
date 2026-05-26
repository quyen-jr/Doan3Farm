using UnityEngine;

public class Cloudy : MonoBehaviour, IWeatherState
{
    public static Cloudy Instance;
    private void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

    }
    public void OnEnter()
    {
        WeatherManger.Instance.SetLightIntensity(0.8f);
    }

    public void OnLeave()
    {
        
    }

    public void OnUpdate()
    {
        
    }
}