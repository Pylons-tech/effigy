using System;
using System.Collections.Generic;
using UnityEngine;

public class CombatUi : MonoBehaviour
{
    // we want a semaphore, behaviorally, but System.Threading semaphores are needlessly heavy here bc
    // this is all running on the unity engine thread no matter what
    private int SemaphoreCount = 0;
    public event EventHandler<CombatUiEvent> UpdateCombatUi;
    private List<CombatUiEvent> pendingEvents = new List<CombatUiEvent>();
    
    public void Stage(CombatUiEvent evt) => pendingEvents.Add(evt);
    public static CombatUi Instance;

    void Awake()
    {
        Instance = this;
    }

    void Update ()
    {
        if (SemaphoreCount < 1 && pendingEvents.Count > 0) UpdateCombatUi(this, GetHighestPriorityPendingEvent());
        if (pendingEvents.Count == 0 && SemaphoreCount == 0 && CombatHost.Instance.current.Ended) FrontEndGameModeController.Instance.ReturnFromCombat();
    }

    private CombatUiEvent GetHighestPriorityPendingEvent()
    {
        CombatUiEvent ret = null;
        foreach (var evt in pendingEvents)
        {
            if (ret == null) ret = evt;
            else if (evt.Priority > ret.Priority) ret = evt;
        }
        pendingEvents.Remove(ret);
        return ret;
    }

    public void IncrementSemaphore() => SemaphoreCount++;

    public void DecrementSemaphore ()
    {
        if (SemaphoreCount > 0) SemaphoreCount--;
    }

    public void Cleanup ()
    {
        pendingEvents.Clear();
        SemaphoreCount = 0;
    }
}