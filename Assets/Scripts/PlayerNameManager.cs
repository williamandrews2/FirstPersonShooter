using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerNameManager : MonoBehaviour
{
    [SerializeField] TMP_InputField usernameInput;

    private void Start()
    {
        if (PlayerPrefs.HasKey("username"))
        {
            usernameInput.text = PlayerPrefs.GetString("username");
            PhotonNetwork.NickName = PlayerPrefs.GetString("username");
        }
        else
        {
            SetRandomUsername();
        }
    }

    public void OnUsernameInputValueChanged()
    {
        if(usernameInput.text == "")
        {
            SetRandomUsername();
        }

        PhotonNetwork.NickName = usernameInput.text;
        PlayerPrefs.SetString("username", usernameInput.text);
    }

    void SetRandomUsername()
    {
        usernameInput.text = "Player" + Random.Range(0, 10000).ToString("0000");
        OnUsernameInputValueChanged(); // Save the random username so we do not generate a random one each time.
    }
}
