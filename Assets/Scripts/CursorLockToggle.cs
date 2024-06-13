using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CursorLockToggle : MonoBehaviour
{
    PhotonView PV;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    private void Start()
    {
        // Initialize the player with a locked cursor.
        LockCursor();
    }

    private void Update()
    {
        if (PV.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (Cursor.lockState == CursorLockMode.Locked)
                {
                    UnlockCursor();
                }
                else
                {
                    LockCursor();
                }
            }
        }
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
