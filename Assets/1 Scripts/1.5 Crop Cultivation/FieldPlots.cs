using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; // 1. THÊM thư viện Photon

// 2. Thêm cái này để Unity tự động gắn PhotonView nếu bạn quên
[RequireComponent(typeof(PhotonView))] 
public class FieldPlots : MonoBehaviour
{
    public bool IsBought;
    [SerializeField] private GameObject fence;

    [SerializeField] private int _maxCropInSmallPlot;
    List<WayPointImage> wayPointImagesList = new List<WayPointImage>();
    private bool isPlayerInField = false;
    
    // 3. THÊM biến PhotonView
    private PhotonView _photonView; 

    private void Awake()
    {
        // 4. Lấy component PhotonView khi game bắt đầu
        _photonView = GetComponent<PhotonView>();
    }

    public void AddWayPointIMGToField(WayPointImage _wayPointImage)
    {
        if (_wayPointImage != null)
        {
            wayPointImagesList.Add(_wayPointImage);
        }
        else Debug.Log(" waypoint null");
    }

    public void RemoveWayPointIMGToFIeld(WayPointImage _wayPointImage)
    {
        if (wayPointImagesList.Contains(_wayPointImage))
        {
            wayPointImagesList.Remove(_wayPointImage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        isPlayerInField = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isPlayerInField = false;
    }

    public bool CheckPlayerInField() => isPlayerInField;
    public int GetMaxCropInSmallPlot() => _maxCropInSmallPlot;

    // ==========================================
    // LOGIC ĐỒNG BỘ MUA ĐẤT
    // ==========================================

    // Hàm này được gọi khi người chơi bấm nút mua (ví dụ từ UI)
    public void SetBuyThisField()
    {
        if (_photonView != null)
        {
            // Bắn lệnh RPC sang tất cả mọi người, kèm theo cờ Buffered cho người vào sau
            _photonView.RPC(nameof(RpcSyncBuyField), RpcTarget.AllBuffered);
        }
    }

    // Hàm này sẽ được Photon gọi trên MỌI MÁY CLIENT
    [PunRPC]
    private void RpcSyncBuyField()
    {
        IsBought = true;
        if (fence != null)
        {
            fence.SetActive(false);
        }
    }
}