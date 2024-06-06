using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UsernameDisplay : MonoBehaviour
{
    [SerializeField] PhotonView playerPV;
    [SerializeField] TMP_Text text;

    private void Start()
    {
        if (playerPV.IsMine)
        {
            // Disable username above our head if we are the local player.
            gameObject.SetActive(false);
        }
        text.text = playerPV.Owner.NickName;
    }
}
