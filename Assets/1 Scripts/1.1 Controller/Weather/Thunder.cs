using System.Collections;
using UnityEngine;

public class Thunder : MonoBehaviour, IWeatherState
{
    public static Thunder Instance;
    public GameObject Lightning;
    public GameObject LightningStart;
    public GameObject LightningEnd;
    public float thunderTime = 0;
    public bool startClock = false;
    private void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

    }
    public void OnEnter()
    {
        WeatherManger.Instance.SetLightIntensity(0.2f);
        WeatherManger.Instance.ToogleRain(true);
        WeatherManger.Instance.ToogleRain2(true);

        WeatherManger.Instance.SetFogIntensity(0.02f);
        thunderTime = Random.Range(10, 20);
        Lightning.SetActive(true);
        startClock = true;
    }

    public void OnLeave()
    {
        startClock = false;
        WeatherManger.Instance.ToogleRain(false);
        WeatherManger.Instance.ToogleRain2(false);
        WeatherManger.Instance.SetFogIntensity(0);

        Lightning.SetActive(false);

        StopCoroutine(ActiveThunder());
        StopCoroutine(DisableThunder());
    }

    public void OnUpdate()
    {
        if(startClock){
            if(thunderTime <= 0){
                thunderTime = Random.Range(5, 10);
                SpawnThunder();
            } else {
                thunderTime -= Time.deltaTime;
            }
        }
        
    }
    public void SpawnThunder(){
        Vector3 currentCameraForward = CameraController.Instance.GetCurrentCamera().transform.forward;
        float randomXPos = Random.Range(-60, 60);
        float randomZPos = Random.Range(20, 70);
        LightningEnd.transform.position = new Vector3(currentCameraForward.x + randomXPos, Player.LocalPlayer.transform.position.y, currentCameraForward.z + randomZPos);
        LightningStart.transform.position = LightningEnd.transform.position + Vector3.up * 30;

        StartCoroutine(ActiveThunder());

    }
    private IEnumerator ActiveThunder(){
        yield return new WaitForSeconds(0.5f);
        Lightning.GetComponent<LineRenderer>().enabled = true;
        StartCoroutine(DisableThunder());
    }
    private IEnumerator DisableThunder(){
        yield return new WaitForSeconds(1);
        Lightning.GetComponent<LineRenderer>().enabled = false;
    }
}
