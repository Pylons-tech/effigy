using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontEndTopLevelMenu : NonModalMenu
{
    private FrontEndInventoryMenu.InventoryMenuMode inventoryMenuMode;

    void Awake ()
    {
        base.Awake();
    }

    public void SetTestMenuInventoryMode (int mode)
    {
        inventoryMenuMode = (FrontEndInventoryMenu.InventoryMenuMode)mode;
    }

    public void OpenInventoryMenuFromTestMenu() => menuSystem.OpenMenu<FrontEndInventoryMenu, FrontEndInventoryMenu.InventoryMenuMode>(inventoryMenuMode);
}
