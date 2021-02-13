using UnityEngine;

/// <summary>
/// A menu that doesn't have any mode transitions.
/// </summary>
public class NonModalMenu : Menu
{
    public OnOpenFocusBehavior DefaultOnOpenFocusBehavior;

    /// <summary>
    /// NonModalMenu does not implement OnModeChanged, since it doesn't
    /// change modes. If this happens, it will throw an exception.
    /// </summary>
    protected override void OnModeChanged<T>(T previousMode)
    {
        throw new System.NotImplementedException("NonModalMenu attempted to change modes");
    }

    public override ModeDescriptor CurrentModeDescriptor =>
        new ModeDescriptor(DefaultOnOpenFocusBehavior, rectTransform.anchorMin, rectTransform.anchorMax, rectTransform.anchoredPosition, rectTransform.sizeDelta);

    protected override ModeDescriptor[] EmitModeDescriptors()
    {
        return new ModeDescriptor[] { new ModeDescriptor(DefaultOnOpenFocusBehavior, rectTransform.anchorMin, rectTransform.anchorMax, rectTransform.anchoredPosition, rectTransform.sizeDelta)};
    }
}