using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontEndInventoryMenu : MonoBehaviour
{
    public static FrontEndInventoryMenu Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
