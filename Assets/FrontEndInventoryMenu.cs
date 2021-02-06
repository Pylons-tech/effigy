using GameTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// TO-DO: I wrote that whole Menu class b/c it'd
/// make the relationship we need between inventory and character menus
/// easier, so let's actually implement it here now.
/// </summary>
public class FrontEndInventoryMenu : MonoBehaviour
{
    public enum InventoryMenuMode
    {
        ViewItems = 0,
        MakingTrade = 1,
        FulfillingTrade = 2,
        FeedingPuppets = 3
    }

    public InventoryMenuMode Mode { get; private set; }

    public static FrontEndInventoryMenu Instance { get; private set; }
    public FrontEndInventoryItemPanel Prototype;
    public ScrollView ScrollView;
    public RectTransform ContentRoot;
    public float Spacing;
    public ManagedScrollRect ManagedScrollRect;

    void Awake()
    {
        Instance = this;
        Prototype.gameObject.SetActive(false);
    }

    void Start()
    {
        Open(InventoryMenuMode.FeedingPuppets);
    }

    public void Open(InventoryMenuMode mode)
    {
        gameObject.SetActive(true);
        Mode = mode;
        PopulateList(new Relic[] { new Relic("", 0, 0, 0, 0, 0, 0, 0, 0, 0), new Relic("", 0, 0, 0, 0, 0, 0, 0, 0, 0), new Relic("", 0, 0, 0, 0, 0, 0, 0, 0, 0) });
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void PopulateList (Relic[] relics)
    {
        ManagedScrollRect.PopulateList(relics);
    }
}
