using UnityEngine;

public abstract class ManagedScrollRectChild : MonoBehaviour
{
    [ExecuteAlways]
    void Awake()
    {
        Debug.Log("aaa");
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