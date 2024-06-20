using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    PhotonView PV;

    GameObject controller;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    private void Start()
    {
        // Ensure that PV is owned by local player.
        if(PV.IsMine)
        {
            CreateController();
        }
    }

    void CreateController()
    {
        Transform spawnpoint = SpawnManager.Instance.GetInitialSpawnpoint();
        // Instantiate our player controller.
        controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), spawnpoint.position, spawnpoint.rotation,0,new object[] {PV.ViewID});
    }

    void Respawn()
    {
        Transform spawnpoint = SpawnManager.Instance.GetRespawnpoint();
        // Instantiate the player controller.
        controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), spawnpoint.position, spawnpoint.rotation, 0, new object[] { PV.ViewID });
    }

    public void Die()
    {
        // Destroy current controller.
        PhotonNetwork.Destroy(controller);

        // Instantiate a new player controller upon respawning.
        Respawn();
    }
}
