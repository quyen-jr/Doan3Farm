using UnityEngine;
using System.Collections;

public class GPSManager : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(GetLocation());
    }

    IEnumerator GetLocation()
    {
        if (!Input.location.isEnabledByUser)
        {
           // Debug.Log("Location services not enabled by user.");
            yield break;
        }

        Input.location.Start();

        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait < 1)
        {
            Debug.Log("Timed out");
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Unable to determine device location");
            yield break;
        }
        else
        {
            Debug.Log("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude);
        }

        Input.location.Stop();
    }
}
