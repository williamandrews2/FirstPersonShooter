using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerManager : MonoBehaviour
{
    PhotonView PV;

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
        // Instantiate our player controller.
        Debug.Log("Instantiated player controller");
    }
}
