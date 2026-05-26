using UnityEngine;
using UnityEngine.UI;

public class TemperatureManager : MonoBehaviour{
    public static TemperatureManager Instance;
    public Text temp;
    private void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    public void SetTemperature(string temp){
       // this.temp.text = temp;
    }
}