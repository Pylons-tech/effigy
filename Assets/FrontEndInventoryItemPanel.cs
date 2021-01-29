using GameTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FrontEndInventoryItemPanel : ManagedScrollRectChild
{
    public RectTransform Panel;
    public TextMeshProUGUI RelicName;
    public TextMeshProUGUI RelicHP;
    public TextMeshProUGUI RelicAtk;
    public TextMeshProUGUI RelicDef;
    public TextMeshProUGUI RelicSpd;
    public TextMeshProUGUI RelicMana;
    public TextMeshProUGUI RelicLevel;
    public TextMeshProUGUI RelicDark;
    public TextMeshProUGUI RelicLight;
    public TextMeshProUGUI RelicEarth;
    public Button UseButton;
    public TextMeshProUGUI ButtonText;

    public override void ConformToData(object data)
    {
        var r = data as Relic;
        RelicName.text = r.Name;
        RelicHP.text = r.HP.ToString();
        RelicAtk.text = r.Attack.ToString();
        RelicDef.text = r.Defense.ToString();
        RelicSpd.text = r.Speed.ToString();
        RelicMana.text = r.Mana.ToString();
        RelicLevel.text = r.Level.ToString();
        RelicDark.text = r.DarkPts.ToString();
        RelicLight.text = r.LightPts.ToString();
        RelicEarth.text = r.EarthPts.ToString();

        switch (FrontEndInventoryMenu.Instance.Mode)
        {
            case FrontEndInventoryMenu.InventoryMenuMode.FeedingPuppets:
                UseButton.gameObject.SetActive(true);
                ButtonText.text = "Give to puppet";
                UseButton.enabled = CanGiveItemToPuppet(r);
                UseButton.onClick = new Button.ButtonClickedEvent();
                UseButton.onClick.AddListener(() => { DoGiveItemToPuppet(r); });
                break;
            case FrontEndInventoryMenu.InventoryMenuMode.FulfillingTrade:
                UseButton.gameObject.SetActive(true);
                ButtonText.text = "Add to trade";
                UseButton.enabled = CanAddItemToFulfillTrade(r);
                UseButton.onClick = new Button.ButtonClickedEvent();
                UseButton.onClick.AddListener(() => { DoAddItemToFulfillTrade(r); });
                break;
            case FrontEndInventoryMenu.InventoryMenuMode.MakingTrade:
                UseButton.gameObject.SetActive(true);
                ButtonText.text = "Add to trade";
                UseButton.enabled = CanAddItemToNewTrade(r);
                UseButton.onClick = new Button.ButtonClickedEvent();
                UseButton.onClick.AddListener(() => { DoAddItemToNewTrade(r); });
                break;
            case FrontEndInventoryMenu.InventoryMenuMode.ViewItems:
                UseButton.gameObject.SetActive(false);
                break;
        }
    }

    public override Vector2 GetElementDimensions()
    {
        return Panel.sizeDelta;
    }

    public bool CanGiveItemToPuppet(Relic relic)
    {
        return true;
    }

    public void DoGiveItemToPuppet(Relic relic)
    {
        throw new NotImplementedException();
    }

    public bool CanAddItemToFulfillTrade(Relic relic)
    {
        return true;
    }

    public void DoAddItemToFulfillTrade(Relic relic)
    {
        throw new NotImplementedException();
    }

    public bool CanAddItemToNewTrade(Relic relic)
    {
        return true;
    }

    public void DoAddItemToNewTrade(Relic relic)
    {
        throw new NotImplementedException();
    }
}
