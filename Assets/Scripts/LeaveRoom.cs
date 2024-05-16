using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveRoom : MonoBehaviour
{
    public void OnLeaveRoom()
    {
        MenuManager.Instance.OpenMenu("title");
    }
}
