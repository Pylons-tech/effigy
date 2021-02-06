using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generic menu class.
/// Provides support for focus hierarchies, some limited auto-layout
/// for (e.g.) side-by-side views of, say, character/relic inventories,
/// and a basic system for producing multi-mode menus.
/// 
/// If a menu only has one mode, it should inherit from NonModalMenu instead
/// of inheriting directly from base class Menu.
/// 
/// TO-DO: Write a menu system manager that actually uses all the focus state
/// data we're tracking now to actually handle the grunt work of shuffling objects
/// and keeping users from interacting w/ non-focused menus.
/// </summary>
public abstract class Menu : MonoBehaviour
{
    /// <summary>
    /// Attribute that must be attached to all Menu instances.
    /// Specifies the way this menu's mode transitions work.
    /// </summary>
    public class MenuAttribute : Attribute
    {
        public readonly ModeDescriptor[] Modes;
        public readonly Enum ModeEnum;

        public MenuAttribute(ModeDescriptor[] modes, Enum modeEnum)
        {
            Modes = modes;
            ModeEnum = modeEnum;
        }
    }

    /// <summary>
    /// Specifies focus-on-open behavior and rect position/size data for
    /// a menu mode.
    /// </summary>
    public readonly struct ModeDescriptor
    {
        public readonly OnOpenFocusBehavior OnOpenFocusBehavior;
        public readonly Vector2 AnchorMin;
        public readonly Vector2 AnchorMax;
        public readonly Vector2 AnchoredPosition;
        public readonly Vector2 SizeDelta;

        public ModeDescriptor(OnOpenFocusBehavior onOpenFocusBehavior, Vector2 anchorMin, Vector2 anchorMax, Vector2 anchoredPosition, Vector2 sizeDelta)
        {
            OnOpenFocusBehavior = onOpenFocusBehavior;
            AnchorMin = anchorMin;
            AnchorMax = anchorMax;
            AnchoredPosition = anchoredPosition;
            SizeDelta = sizeDelta;
        }
    }

    /// <summary>
    /// TakeFocus: when menu is opened, automatically grabs focus from all existing menus.
    /// ShareFocus: when menu is opened, joins the active focus frame.
    /// </summary>
    public enum OnOpenFocusBehavior
    {
        None = 0,
        TakeFocus = 1,
        ShareFocus = 2
    }

    /// <summary>
    /// Data class used to track the focus hierarchy of open menus.
    /// </summary>
    public class Frame
    {
        /// <summary>
        /// The list of menus occupying this frame. Don't
        /// modify this directly - use Frame.Add() and Frame.Remove()
        /// instead.
        /// </summary>
        public readonly LinkedList<Menu> Menus = new LinkedList<Menu>();

        /// <summary>
        /// Create a new frame. m must contain at least one object, or this
        /// will throw an exception.
        /// </summary>
        public Frame (params Menu[] m)
        {
            if (m.Length == 0) throw new Exception("Can't create an empty focus frame");
            foreach (var menu in m)
            {
                if (menu._frame != null) menu._frame.Remove(menu);
                Menus.AddLast(menu);
                menu._frame = this;
            }
        }

        /// <summary>
        /// Movees this frame to the top of the focus stack.
        /// Objects within it will have UI focus.
        /// </summary>
        public void TakeFocus()
        {
            FocusStack.Remove(this);
            FocusStack.AddLast(this);
        }

        /// <summary>
        /// Close all menus within the frame.
        /// On the last menu's closing, this frame will
        /// automatically be removed from the stack and
        /// destroyed.
        /// </summary>
        public void CloseAll()
        {
            foreach (var menu in Menus) menu.Close();
        }

        /// <summary>
        /// Add one or more menus to the frame.
        /// </summary>
        public void Add(params Menu[] m)
        {
            foreach (var menu in m)
            {
                if (menu._frame != null) menu._frame.Remove(menu);
                Menus.AddLast(menu);
                menu._frame = this;
            }
        }

        /// <summary>
        /// Removes one or more menus from the frame.
        /// If at any point the frame has no occupants as
        /// a result, it is automaticaly destroyed.
        /// </summary>
        public void Remove(params Menu[] m)
        {
            foreach (var menu in m)
            {
                Menus.Remove(menu);
                menu._frame = null;
            }
            if (Menus.Count == 0) FocusStack.Remove(this);
        }

        /// <summary>
        /// The frame on top of the focus stack, assuming it is not empty.
        /// Null if so.
        /// </summary>
        public static Frame Current
        {
            get
            {
                if (FocusStack.Count > 0) return FocusStack.Last.Value;
                else return null;
            }
        }
    }

    /// <summary>
    /// The Frame this menu occupies. Don't update this manually; use Frame.Add() and Frame.Remove().
    /// </summary>
    private Frame _frame;

    /// <summary>
    /// ModeDescriptors corresponding to each mode specified by this menu's ModeEnum.
    /// </summary>
    private ModeDescriptor[] ModeDescriptorTable;
    /// <summary>
    /// An enumeration used to specify each mode this menu can operate in.
    /// </summary>
    private Enum ModeEnum;
    /// <summary>
    /// The ModeDescriptor for this menu's current mode.
    /// This is virtual to enable NonModalMenu to override it in order to provide sane
    /// output.
    /// </summary>
    public virtual ModeDescriptor CurrentModeDescriptor => ModeDescriptorTable[_mode];
    public T CurrentMode<T>() where T : Enum
    {
        if (typeof(T) != ModeEnum.GetType()) throw new Exception("ChangeMode must match menu's MenuAttribute.ModeEnum");
        return (T)(object)(_mode);
    }
    /// <summary>
    /// Index of the current mode.
    /// Used to look up ModeEnum values and ModeDescriptorTable entries.
    /// </summary>
    private int _mode;
    /// <summary>
    /// GameObject's transform as a RectTransform.
    /// </summary>
    public RectTransform rectTransform { get; private set; }

    /// <summary>
    /// Linked list (not a stack, despite the name - we need to shuffle the order of these around frequently; it's a "stack"
    /// in that lower items are "on top of" higher ones) used to track the relationships between menus and which ones have focus right noow.
    /// </summary>
    private static LinkedList<Frame> FocusStack = new LinkedList<Frame>();

    protected virtual void Awake ()
    {
        rectTransform = transform as RectTransform;
        foreach (var attr in GetType().GetCustomAttributes(false))
        {
            if (attr.GetType() == typeof(MenuAttribute))
            {
                ModeDescriptorTable = (attr as MenuAttribute).Modes;
                ModeEnum = (attr as MenuAttribute).ModeEnum;
                break;
            }
        }
    }

    /// <summary>
    /// Take focus by joining the current active focus frame.
    /// This means you don't unfocus anything presently in focus.
    /// </summary>
    public void TakeFocusJoinFrame()
    {
        FocusStack.Last.Value.Add(this);
    }

    /// <summary>
    /// Take focus by creating a new focus frame, unfocusing anything
    /// presently in focus.
    /// </summary>
    public void TakeFocusNewFrame()
    {
        FocusStack.AddLast(new Frame(this));
    }

    /// <summary>
    /// Change menu's operating mode.
    /// ModeDescriptor values will be implemented immediately;
    /// implementation-specific behavior should be implemented in your
    /// OnModeChanged method
    /// </summary>
    public void ChangeMode<T>(T val) where T: Enum
    {
        if (typeof(T) != ModeEnum.GetType()) throw new Exception("ChangeMode must match menu's MenuAttribute.ModeEnum");
        var lastMode = _mode;
        _mode = (int)(object)val;
        rectTransform.anchorMin = CurrentModeDescriptor.AnchorMin;
        rectTransform.anchorMax = CurrentModeDescriptor.AnchorMax;
        rectTransform.sizeDelta = CurrentModeDescriptor.SizeDelta;
        rectTransform.anchoredPosition = CurrentModeDescriptor.AnchoredPosition;
        OnModeChanged((T)(object)lastMode);
    }

    /// <summary>
    /// Handles state transition to current mode from previousMode.
    /// </summary>
    protected abstract void OnModeChanged<T>(T previousMode);

    /// <summary>
    /// Opens this menu. Takes focus automatically based on current mode's OnOpenFocusBehavior.
    /// (If OnOpenFocusBehavior is none, menu is opened but doesn't take focus.)
    /// </summary>
    public void Open()
    {
        gameObject.SetActive(true);
        if (CurrentModeDescriptor.OnOpenFocusBehavior == OnOpenFocusBehavior.ShareFocus) TakeFocusJoinFrame();
        else if (CurrentModeDescriptor.OnOpenFocusBehavior == OnOpenFocusBehavior.TakeFocus) TakeFocusNewFrame();
    }

    /// <summary>
    /// Closes this menu. Removes it from the frame it currently occupies.
    /// </summary>
    public void Close()
    {
        gameObject.SetActive(false);
        _frame.Remove(this);
    }

    public bool HasFocus {
        get => _frame != null && _frame == FocusStack.Last?.Value;
    }
}
