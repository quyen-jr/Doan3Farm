using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DateManager : MonoBehaviour
{
    public static DateManager Instance;
    public Text date;
    private void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    public GameObject Date;
    public void SetDate(string date){
        string[] dateParts = date.Split("-");
        string dateDisplay = dateParts[2] + "/" + dateParts[1] + "/" + dateParts[0];
      //  this.date.text = dateDisplay;
    }
}
