using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalReferencesAssigner : MonoBehaviour
{
    private void Awake() {
        GlobalReference.Player = GameObject.FindWithTag("Player");
    }
}
