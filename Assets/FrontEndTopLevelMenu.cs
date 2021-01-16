using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontEndTopLevelMenu : MonoBehaviour
{
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
}
