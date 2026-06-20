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
    // THÊM: Biến tham chiếu đến script Animation của riêng nhân vật này (không dùng LocalPlayer chung chung nữa)
    private PlayerAnimation _myPlayerAnimation;

    private void Awake()
    {
        _photonView = GetComponentInParent<PhotonView>();
        // Lấy script PlayerAnimation nằm trên cùng GameObject (hoặc Parent) của nhân vật này
        _myPlayerAnimation = GetComponentInParent<PlayerAnimation>(); 
    }

    private void Start()
    {
        AddTools();

        if (_photonView == null || _photonView.IsMine)
        {
            UIController.Instance.landInteraction = this;
        }
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

    // ==========================================
    // CÁC HÀNH ĐỘNG (ĐÃ SỬA ĐỂ ĐỒNG BỘ MẠNG)
    // ==========================================

    public void Hoe() => ExecuteAction("Hoe", "Hoe");
    public void Poach() => ExecuteAction("Hoe", "Poach");
    public void Plant() => ExecuteAction(string.Empty, "Plant");
    public void Water() => ExecuteAction("Watering can", "Water");
    public void Fertilize() => ExecuteAction("Fertilizer", "Fertilize");
    public void Spray() => ExecuteAction("Spray", "Pesticide");
    public void Harvest() => ExecuteAction(string.Empty, "Harvest");

    private void ExecuteAction(string toolName, string animTrigger)
    {
        SetActiveToolAndSync(toolName);

        if (_photonView != null && _photonView.IsMine)
        {
            _photonView.RPC(nameof(RpcPlayActionAnim), RpcTarget.All, animTrigger);
        }
    }

    [PunRPC]
    private void RpcPlayActionAnim(string triggerName)
    {
        if (_myPlayerAnimation != null)
        {
            _myPlayerAnimation.SetAnimTrigger(triggerName);
        }
    }

    // ==========================================
    // LOGIC DỤNG CỤ
    // ==========================================

    public void SetActiveToolAndSync(string toolName)
    {
        SetActiveToolLocal(toolName);

        if (_photonView != null && _photonView.IsMine)
        {
            _photonView.RPC(nameof(RpcSetActiveTool), RpcTarget.Others, toolName);
        }
    }

    private void SetActiveToolLocal(string toolName)
    {
        DisableAllTools();
        if (!string.IsNullOrEmpty(toolName))
        {
            EnableTool(toolName);
        }
    }

    [PunRPC]
    private void RpcSetActiveTool(string toolName)
    {
        SetActiveToolLocal(toolName);
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
        DisableToolLocal(toolName);

        if (_photonView != null && _photonView.IsMine)
        {
            _photonView.RPC(nameof(RpcDisableTool), RpcTarget.Others, toolName);
        }
    }

    private void DisableToolLocal(string toolName)
    {
        if (_tools.ContainsKey(toolName))
        {
            _tools[toolName].SetActive(false);
        }
    }

    [PunRPC]
    private void RpcDisableTool(string toolName)
    {
        DisableToolLocal(toolName);
    }
}