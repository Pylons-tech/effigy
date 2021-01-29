﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontEndTopLevelMenu : MonoBehaviour
{
    private FrontEndInventoryMenu.InventoryMenuMode inventoryMenuMode;

    public static FrontEndTopLevelMenu Instance { get; private set; }

    void Awake ()
    {
        Instance = this;
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void SetTestMenuInventoryMode (int mode)
    {
        inventoryMenuMode = (FrontEndInventoryMenu.InventoryMenuMode)mode;
    }

    public void OpenInventoryMenuFromTestMenu() => FrontEndInventoryMenu.Instance.Open(inventoryMenuMode);
}
