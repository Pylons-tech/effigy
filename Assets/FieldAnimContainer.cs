using System.Collections.Generic;
using UnityEngine;
using GameTypes;

public class FieldAnimContainer : MonoBehaviour
{
    private Dictionary<FieldAnimType, FieldAnimController> FieldAnimCache = new Dictionary<FieldAnimType, FieldAnimController>();

    void Start()
    {
        CombatUi.Instance.UpdateCombatUi += (s, evt) =>
        {
            PlayAnimation(evt.FieldAnim);
        };
    }

    private void PlayAnimation (FieldAnimType fieldAnim)
    {
        if (FieldAnimCache.TryGetValue(fieldAnim, out var controller)) controller.Play();
        else
        {
            var prefab = Resources.Load<FieldAnimController>($"Anim/Battle/Field/{fieldAnim}_Controller");
            if (prefab != null)
            {
                FieldAnimCache[fieldAnim] = Instantiate(prefab);
                FieldAnimCache[fieldAnim].transform.SetParent(transform, true);
                FieldAnimCache[fieldAnim].transform.localPosition = Vector3.zero;
                FieldAnimCache[fieldAnim].transform.localRotation = Quaternion.identity;
                FieldAnimCache[fieldAnim].Play();
            }
        }
    }
}
