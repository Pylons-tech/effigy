using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagedScrollRect : MonoBehaviour
{
    public MenuSystem MenuSystem { get; private set; }

    [Flags]
    private enum ScrollType
    {
        None = 0,
        Horizontal = 1,
        Vertical = 1 << 1,
        Both = Horizontal | Vertical
    }

    public Vector2 Padding;

    public ManagedScrollRectChild Prototype;
    private ScrollRect scrollRect;
    private Type dataType;
    private List<ManagedScrollRectChild> ChildElements = new List<ManagedScrollRectChild>();

    void Awake ()
    {
        scrollRect = GetComponent<ScrollRect>();
        MenuSystem = MenuSystem.Get(gameObject.scene);
    }

    private void RecalculateScrollRectBoundsAndChildPositions ()
    {
        var st = ScrollType.None;
        if (scrollRect.horizontal)
        {
            if (scrollRect.vertical) st = ScrollType.Both;
            else st = ScrollType.Horizontal;
        }
        else if (scrollRect.vertical) st = ScrollType.Vertical;
        if (st == ScrollType.None) throw new Exception("Can't recalculate bounds of a ScrollRect w/ no legal scroll direction");
        var size = Vector2.zero;

        // Content rect has to have correct anchors before moving child elements around

        foreach (var element in ChildElements)
        {
            if (st.HasFlag(ScrollType.Horizontal)) size.x += element.GetElementDimensions().x + Padding.x;
            else size.x = element.GetElementDimensions().x + Padding.x;
            if (st.HasFlag(ScrollType.Vertical)) size.y += element.GetElementDimensions().y + Padding.y;
            else size.y = element.GetElementDimensions().y + Padding.y;
        }
        if (st.HasFlag(ScrollType.Horizontal)) size.x -= Padding.x;
        if (st.HasFlag(ScrollType.Vertical)) size.y -= Padding.y;

        var prevElementsSpacing = Vector2.zero;

        for (int i = 0; i < ChildElements.Count; i++)
        {
            var rt = ChildElements[i].RectTransform();
            rt.anchoredPosition = prevElementsSpacing;
            var dimensions = ChildElements[i].GetElementDimensions();
            if (st.HasFlag(ScrollType.Horizontal)) prevElementsSpacing.x += dimensions.x + Padding.x;
            if (st.HasFlag(ScrollType.Vertical)) prevElementsSpacing.y -= dimensions.y + Padding.y;
            ChildElements[i].gameObject.SetActive(true);
        }
        scrollRect.content.sizeDelta = size;
    }

    public void PopulateList<T>(T[] data)
    {
        if (ChildElements.Count > data.Length)
        {
            for (int i = ChildElements.Count - 1; i >= data.Length; i--)
            {
                ChildElements[i].Destroy();
                ChildElements.RemoveAt(i);
            }
        }
        else if (ChildElements.Count < data.Length)
        {
            for (int i = ChildElements.Count; i < data.Length; i++) ChildElements.Add(Prototype.Duplicate());
        }
        for (int i = 0; i < data.Length; i++) ChildElements[i].ConformToData(data[i]);
        RecalculateScrollRectBoundsAndChildPositions();
    }
}
