using Photon.Pun;
using UnityEngine;

public class NetworkSpawnManager : MonoBehaviourPunCallbacks
{
    [Header("Player Prefab")]
    [SerializeField] private string playerPrefabName = "Player";

    [Header("Spawn Points")]
    [SerializeField] private Transform[] spawnPoints;

    private bool hasSpawned;

    private void Start()
    {
        TrySpawnPlayer();
    }

    public override void OnJoinedRoom()
    {
        TrySpawnPlayer();
    }

    private void TrySpawnPlayer()
    {
        if (hasSpawned) return;

        if (!PhotonNetwork.InRoom)
        {
            Debug.LogWarning("Chưa ở trong Photon Room nên chưa spawn player.");
            return;
        }

        SpawnPlayer();
        hasSpawned = true;
    }

    private void SpawnPlayer()
    {
        Vector3 spawnPosition = Vector3.zero;
        Quaternion spawnRotation = Quaternion.identity;

        if (spawnPoints != null && spawnPoints.Length > 0)
        {
            int index = (PhotonNetwork.LocalPlayer.ActorNumber - 1) % spawnPoints.Length;

            spawnPosition = spawnPoints[index].position;
            spawnRotation = spawnPoints[index].rotation;
        }

        GameObject player = PhotonNetwork.Instantiate(
            playerPrefabName,
            spawnPosition,
            spawnRotation
        );

        Debug.Log("Đã spawn player: " + player.name);
    }
}
