using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    [SerializeField] private NPCBase[] _listNPC;

    private Transform _mainCharacterTransform;

    private const float k_TimeDelay = 0.5f;
    private const float k_MinDistance = 3f;

    private void Awake() 
    {
        _listNPC = GetComponentsInChildren<NPCBase>();
    }

    private void AttachCharacterTransform()
    {
        _mainCharacterTransform = Player.LocalPlayer.transform;
    } 

    private void Start() 
    {
        AttachCharacterTransform();
        StartCoroutine(AutoHighLightNPCNearestMainCharacter());
    }

    private IEnumerator AutoHighLightNPCNearestMainCharacter()
    {
        while(true)
        {
            yield return new WaitForSeconds(k_TimeDelay);

            foreach(var npc in _listNPC)
            {
                if(DistanceToMainChar(npc.GetPosition()) < k_MinDistance)
                {
                    npc.TurnOnHighLight();
                }
                else 
                {
                    npc.TurnOffHighLight();
                }
            }
        }
    }

    private float DistanceToMainChar(Vector3 pos)
    {
        return Vector3.Distance(pos, _mainCharacterTransform.position);
    }
}

