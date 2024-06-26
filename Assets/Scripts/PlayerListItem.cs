using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;
using Photon.Realtime;
using TMPro;

public class PlayerListItem : MonoBehaviourPunCallbacks
{
    // This script is responsible for destroying itself when the player leaves the room.

    Player player;
    [SerializeField] TMP_Text text;

    public void SetUp(Player _player)
    {
        // Set up a private reference to the player
        player = _player;
        text.text = _player.NickName;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        // If the player that left the room matches the player we have referenced, 
        // destroy the player name in the list.
        if(player == otherPlayer)
        {
            Destroy(gameObject);
        }
    }

    public override void OnLeftRoom()
    {
        // Destroy ourselves if we have left the room since we cannot see the players anymore
        Destroy(gameObject);
    }

}
