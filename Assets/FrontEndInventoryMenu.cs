using GameTypes;
using System;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// TO-DO: I wrote that whole Menu class b/c it'd
/// make the relationship we need between inventory and character menus
/// easier, so let's actually implement it here now.
/// </summary>
public class FrontEndInventoryMenu : Menu
{
    public enum InventoryMenuMode
    {
        ViewItems = 0,
        MakingTrade = 1,
        FulfillingTrade = 2,
        FeedingPuppets = 3
    }

    public InventoryMenuMode Mode { get; private set; }
    public FrontEndInventoryItemPanel Prototype;
    public ScrollView ScrollView;
    public RectTransform ContentRoot;
    public float Spacing;
    public ManagedScrollRect ManagedScrollRect;
    // todo: ???
    private readonly static ModeDescriptor[] _ModeDescriptorTable = new ModeDescriptor[]
    {
        new ModeDescriptor(OnOpenFocusBehavior.TakeFocus, Vector2.zero, Vector2.zero, Vector2.zero, Vector2.one),
        new ModeDescriptor(OnOpenFocusBehavior.TakeFocus, Vector2.zero, Vector2.zero, Vector2.zero, Vector2.one),
        new ModeDescriptor(OnOpenFocusBehavior.TakeFocus, Vector2.zero, Vector2.zero, Vector2.zero, Vector2.one),
        new ModeDescriptor(OnOpenFocusBehavior.TakeFocus, Vector2.zero, Vector2.zero, Vector2.zero, Vector2.one)
    };

    void Awake()
    {
        base.Awake();
    }

    private void PopulateList (Relic[] relics)
    {
        ManagedScrollRect.PopulateList(relics);
    }

    protected override void OnModeChanged<T>(T previousMode)
    {
        Debug.Log("Implement mode switch behavior for inventory menu please");
        return;
    }
}
