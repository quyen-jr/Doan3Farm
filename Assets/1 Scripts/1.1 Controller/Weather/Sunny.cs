using UnityEngine;

public class Sunny : MonoBehaviour, IWeatherState {
    public static Sunny Instance;
    private void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

    }
    public void OnEnter()
    {
        WeatherManger.Instance.SetLightIntensity(1);
    }

    public void OnLeave()
    {
        
    }

    public void OnUpdate()
    {
        
    }
}