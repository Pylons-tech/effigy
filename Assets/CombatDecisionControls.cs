using UnityEngine;
public class CombatDecisionControls : MonoBehaviour
{
    private CombatTextbox combatTextbox;
    private CombatUi combatUi;
    private CombatHost combatHost;
    public bool Active { get; private set; }

    void Start()
    {
        combatHost = FindObjectOfType<CombatHost>();
        combatUi = FindObjectOfType<CombatUi>();
        combatTextbox = FindObjectOfType<CombatTextbox>();
        combatUi.UpdateCombatUi += (s, e) =>
        {
            if (e.FieldAnim == GameTypes.FieldAnimType.Start || e.FieldAnim == GameTypes.FieldAnimType.NextRound)
                Active = true;
            else Active = false;
            gameObject.SetActive(Active);
        };
    }

    public void SelectFlee()
    {
        combatHost.Flee();
        combatTextbox.Release();
    }

    public void SelectFight()
    {
        combatHost.Attack();
        combatTextbox.Release();
    }
}