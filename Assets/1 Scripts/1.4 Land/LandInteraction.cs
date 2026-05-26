using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class LandInteraction : MonoBehaviour
{
    public GameObject hoe;
    public GameObject spray;
    public GameObject wateringCan;
    public GameObject fertilizer;
    public GameObject pickaxe;
    private Dictionary<string, GameObject> _tools = new();
    private PhotonView _photonView;

    private void Awake()
    {
        _photonView = GetComponentInParent<PhotonView>();
    }

    private void Start()
    {
        _tools.Clear();
        _tools.Add("Hoe", hoe);
        _tools.Add("Spray", spray);
        _tools.Add("Watering can", wateringCan);
        _tools.Add("Fertilizer", fertilizer);
        _tools.Add("Pickaxe", pickaxe);

        if (_photonView == null || _photonView.IsMine)
        {
            UIController.Instance.landInteraction = this;
        }

        AddTools();
    }
    public void AddTools()
    {
        _tools.Clear();
        _tools.Add("Hoe", hoe);
        _tools.Add("Spray", spray);
        _tools.Add("Watering can", wateringCan);
        _tools.Add("Fertilizer", fertilizer);
        _tools.Add("Pickaxe", pickaxe);
    }
    public void SwitchToolPlayer(List<GameObject> toolObjects)
    {
        DisableAllTools();
        _tools.Clear();
        hoe = toolObjects[0];
        spray = toolObjects[1];
        wateringCan = toolObjects[2];
        fertilizer = toolObjects[3];
        pickaxe = toolObjects[4];
        _tools.Add("Hoe", hoe);
        _tools.Add("Spray", spray);
        _tools.Add("Watering can", wateringCan);
        _tools.Add("Fertilizer", fertilizer);
        _tools.Add("Pickaxe", pickaxe);
    }
    public void Hoe()
    {

        DisableAllTools();
        EnableTool("Hoe");
        if (Player.LocalPlayer != null) Player.LocalPlayer.playerAnimation.SetAnimTrigger("Hoe");
    }
    public void Poach()
    {
        DisableAllTools();
        EnableTool("Hoe");
        if (Player.LocalPlayer != null) Player.LocalPlayer.playerAnimation.SetAnimTrigger("Poach");
    }

    public void Plant()
    {
        DisableAllTools();
        if (Player.LocalPlayer != null) Player.LocalPlayer.playerAnimation.SetAnimTrigger("Plant");
    }
    public void Water()
    {
        DisableAllTools();
        EnableTool("Watering can");
        if (Player.LocalPlayer != null) Player.LocalPlayer.playerAnimation.SetAnimTrigger("Water");
    }
    public void Fertilize()
    {
        DisableAllTools();
        EnableTool("Fertilizer");
        if (Player.LocalPlayer != null) Player.LocalPlayer.playerAnimation.SetAnimTrigger("Fertilize");
    }
    public void Spray()
    {
        DisableAllTools();
        EnableTool("Spray");
        if (Player.LocalPlayer != null) Player.LocalPlayer.playerAnimation.SetAnimTrigger("Pesticide");
    }
    public void Harvest()
    {
        if (Player.LocalPlayer != null) Player.LocalPlayer.playerAnimation.SetAnimTrigger("Harvest");
    }
    public void DisableAllTools()
    {
        foreach (KeyValuePair<string, GameObject> tool in _tools)
        {
            tool.Value.SetActive(false);
        }
    }
    public void EnableTool(string toolName)
    {
        foreach (KeyValuePair<string, GameObject> tool in _tools)
        {
            if (tool.Key == toolName)
            {
                tool.Value.SetActive(true);
                return;
            }
        }
    }
    public void DisableTool(string toolName)
    {
        _tools[toolName].SetActive(false);
    }
}


