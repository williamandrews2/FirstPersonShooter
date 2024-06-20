using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreenManager : MonoBehaviour
{
    public Canvas pauseMenu;

    PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    private void Update()
    {
        // Toggle the pause menu on and off.
        if(Cursor.lockState == CursorLockMode.None)
        {
            pauseMenu.gameObject.SetActive(true);
        }
        else
        {
            pauseMenu.gameObject.SetActive(false);
        }
    }

    public void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void LeaveLobby()
    {
        // Possibly needs to be relocated to another script.(not working currently)
        if (PV.IsMine)
        {
            PhotonNetwork.Destroy(this.gameObject);
            SceneManager.LoadScene(0);
        }
    } 

    public void ExitGame()
    {
        Application.Quit();
    }
}
