using GameTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FrontEndInventoryMenu : MonoBehaviour
{
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
        Open();
    }

    public void Open()
    {
        gameObject.SetActive(true);

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
