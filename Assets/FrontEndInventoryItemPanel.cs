using GameTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontEndInventoryItemPanel : ManagedScrollRectChild
{
    public RectTransform Panel;

    [ExecuteAlways]
    void Awake()
    {
        Debug.Log("baa");
    }

    public override void ConformToData(object data)
    {
        return;
        throw new System.NotImplementedException();
    }

    public override Vector2 GetElementDimensions()
    {
        return Panel.sizeDelta;
        throw new System.NotImplementedException();
    }
}
