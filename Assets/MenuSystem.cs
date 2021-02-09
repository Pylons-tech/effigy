using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Menu;

public class MenuSystem : MonoBehaviour
{
    public RectTransform FocusShade;
    /// <summary>
    /// Linked list (not a stack, despite the name - we need to shuffle the order of these around frequently; it's a "stack"
    /// in that lower items are "on top of" higher ones) used to track the relationships between menus and which ones have focus right noow.
    /// </summary>
    private LinkedList<Frame> _focusStack = new LinkedList<Frame>();

    private Dictionary<Type, Menu> _menus = new Dictionary<Type, Menu>();
    private bool _dirty;

    public Menu OpenMenu<TMenu>() where TMenu : Menu
    {
        var menu = _menus[typeof(TMenu)];
        if (menu == null) throw new Exception(
            $"MenuSystem {gameObject.name} in scene {SceneManager.GetActiveScene().name} " +
            $"doesn't contain a child menu of type {typeof(TMenu).FullName}");
        menu.Open();
        return menu;
    }

    public Menu OpenMenu<TMenu, TMode>(TMode mode) where TMenu : Menu where TMode : Enum
    {
        var menu = OpenMenu<TMenu>();
        menu.ChangeMode(mode);
        return menu;
    }

    public void AddToTopOfFocusStack(Frame frame)
    {
        _focusStack.AddLast(frame);
        _dirty = true;
    }

    public void RemoveFromFocusStack(Frame frame)
    {
        _focusStack.Remove(frame);
        _dirty = true;
    }

    public Frame CurrentFocusFrame
    {
        get
        {
            if (_focusStack.Count > 0) return _focusStack.Last.Value;
            else return null;
        }
    }

    private void ConformMenuPositionsToFocusHierarchy()
    {
        var frame = _focusStack.First;
        for (int i = 0; i < _focusStack.Count; i++)
        {
            foreach (var menu in frame.Value.Menus)
            {
                menu.rectTransform.localPosition = new Vector3(menu.rectTransform.localPosition.x, menu.rectTransform.localPosition.y, i + 1);
            }
            frame = frame.Next;
        }
        // FocusShade blocks everything that isn't part of the active frame, preventing OnClick events from firing, but
        // we'll still need to implement a more robust system for dealing w/ kb/m or gamepad input.
        FocusShade.localPosition = new Vector3(FocusShade.localPosition.x, FocusShade.localPosition.y, _focusStack.Count - 1);
    }

    void Awake()
    {
        foreach (var menu in GetComponentsInChildren<Menu>(true))
            _menus[menu.GetType()] = menu;
    }

    void Update()
    {
        if (_dirty)
        {
            ConformMenuPositionsToFocusHierarchy();
            _dirty = false;
        }
    }

    public T GetMenu<T> () where T : Menu
    {
        throw new NotImplementedException("implement this!!!!");
    }

    public static MenuSystem Get(Scene scene)
    {
        throw new NotImplementedException("implement this!!!!");
    }
}
