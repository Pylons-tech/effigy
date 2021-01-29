using UnityEngine;

public abstract class ManagedScrollRectChild : MonoBehaviour
{
    [ExecuteAlways]
    void Awake()
    {
        // Ensures anchor point is in rect's top left corner - constant anchor point greatly simplifies the way element placement would otherwise have to work
        if ((transform as RectTransform).anchorMin != Vector2.up) (transform as RectTransform).anchorMin = Vector2.up;
        if ((transform as RectTransform).anchorMax != Vector2.up) (transform as RectTransform).anchorMax = Vector2.up;
        if ((transform as RectTransform).pivot != Vector2.up) (transform as RectTransform).pivot = Vector2.up;
    }

    public abstract Vector2 GetElementDimensions();

    public abstract void ConformToData(object data);

    public void Destroy() => Destroy(gameObject);

    public ManagedScrollRectChild Duplicate()
    {
        var newObj = Instantiate(gameObject).GetComponent(GetType());
        (newObj.transform as RectTransform).SetParent(transform.parent);
        return (ManagedScrollRectChild)newObj;
    }

    public RectTransform RectTransform() => transform as RectTransform;
}