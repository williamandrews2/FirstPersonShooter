using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public string menuName;
    public bool isOpen;

    public void Open()
    {
        // Enable menu
        gameObject.SetActive(true);
        isOpen = true;
    }

    public void Close()
    {
        // Disable menu
        gameObject.SetActive(false);
        isOpen = false;
    }
}
