using System.Collections;
using System.Collections.Generic;
//using UnityEditorInternal;
using UnityEngine;
using Photon.Pun;

public class MenuManager : MonoBehaviour
{
    // Variable bound the class not the object
    public static MenuManager Instance;

    [SerializeField] Menu[] menus;
    private void Awake()
    {
        Instance = this;
    }
    public void OpenMenu (string menuName)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].menuName == menuName)
            {
                OpenMenu(menus[i]);
            }
            else if (menus[i].isOpen)
            {
                // Close menus that are not the one we want open
                CloseMenu(menus[i]);
            }
        }
    }

    public void OpenMenu (Menu menu)
    {
        for(int i = 0;i < menus.Length;i++) 
        {
            if (menus[i].isOpen)
            {
                CloseMenu(menus[i]);
            }
        }
        menu.Open();
    }

    public void CloseMenu(Menu menu)
    {
        menu.Close();
    }
}
