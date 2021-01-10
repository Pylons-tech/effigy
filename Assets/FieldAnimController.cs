using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldAnimController : MonoBehaviour
{
    public Animator animator;

    public void Play()
    {
        CombatUi.Instance.IncrementSemaphore();
        gameObject.SetActive(true);
        animator.SetTrigger("Play");
    }

    public void Finish()
    {
        CombatUi.Instance.DecrementSemaphore();
        gameObject.SetActive(false);
    }
}
