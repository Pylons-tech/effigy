using GameData;
using GameTypes;
using System.Text;
using UnityEngine;

public class CombatHost : MonoBehaviour
{
    private CombatUi CombatUi;
    public Battle current { get; private set; }
    public static CombatHost Instance { get; private set; }

    void Awake()
    {
        Instance = this;
        CombatUi = FindObjectOfType<CombatUi>();
    }

    public void StartEncounter (EncounterType e)
    {
        var tempParty = new Puppet[] { Puppet.GetStarter(), Puppet.GetStarter(), Puppet.GetStarter(), Puppet.GetStarter() };
        foreach (var p in tempParty) p.Live((_) => { });
        current = new Battle(e, tempParty);
        current.OnPuppetDies += (s, b) => { PuppetDies(b.Puppets); };
        current.OnEnemyDies += (s, b) => { EnemyDies(); };
        current.OnRelics += (s, b) => { GotRelics(b.Relics); };
        current.OnRetreated += (s, b) => { Retreated(); };
        current.OnNextRound += (s, b) => { BetweenRounds(); };
        current.OnBattleLost += (s, b) => { Lost(); };
        BetweenRounds();
    }

    void BetweenRounds()
    {
        string str;
        Debug.Log(current.Round);
        if (current.Round == 0)
        {
            if (current.Enemies[0].CurrentHP < current.Enemies[0].MaxHP / 2) str = $"You've confronted an enemy {current.Enemies[0].Name}!\nIt looks weary. Attack it, or flee?";
            else str = $"You've confronted an enemy {current.Enemies[0].Name}!\nAttack it, or flee?";
            CombatUi.Stage(
                new CombatUiEvent(str, FieldAnimType.Start, EnemyAnimType.Spawn, PuppetAnimType.None, 100));
        }
        else
        {
            if (current.Enemies[0].CurrentHP < current.Enemies[0].MaxHP / 2) str = $"The enemy {current.Enemies[0].Name} looks weary.\nAttack it, or flee?";
            else str = $"The enemy {current.Enemies[0].Name} stands firm.\nAttack it, or flee?";
            CombatUi.Stage(
                new CombatUiEvent(str, FieldAnimType.NextRound, EnemyAnimType.Damage, PuppetAnimType.Damage, 100));
        }
        Debug.Log(str);
    }

    public void Attack()
    {
        CombatUi.Stage(
            new CombatUiEvent($"Attacking the enemy {current.Enemies[0].Name}!", FieldAnimType.AtkRound, EnemyAnimType.None, PuppetAnimType.None, 400));
        current.DoCombat(false);
    }

    public void Flee ()
    {
        CombatUi.Stage(
            new CombatUiEvent($"Fleeing the enemy {current.Enemies[0].Name}!", FieldAnimType.None, EnemyAnimType.None, PuppetAnimType.None, 400));
        current.DoCombat(true);
    }

    public void Lost ()
    {
        CombatUi.Stage(
            new CombatUiEvent($"Defeated!", FieldAnimType.Lose, EnemyAnimType.None, PuppetAnimType.None, 230));
    }

    void EnemyDies()
    {
        CombatUi.Stage(
           new CombatUiEvent($"The enemy {current.Enemies[0].Name} has fallen!", FieldAnimType.Win, EnemyAnimType.Die, PuppetAnimType.None, 220));
    }

    void GotRelics(Relic[] r)
    {
        var sb = new StringBuilder("Got relics: ");
        foreach (var relic in r) sb.Append($"{relic.Name}, ");
        sb.Length -= 2;
        CombatUi.Stage(
            new CombatUiEvent(sb.ToString(), FieldAnimType.None, EnemyAnimType.None, PuppetAnimType.None, 170));
    }

    void PuppetDies (Puppet[] p)
    {
        var sb = new StringBuilder();
        foreach (var puppet in p) sb.AppendLine($"{puppet.Name} has fallen...!");
        CombatUi.Stage(
            new CombatUiEvent(sb.ToString(), FieldAnimType.None, EnemyAnimType.None, PuppetAnimType.Die, 250));
    }

    void Retreated()
    {
        CombatUi.Stage(
           new CombatUiEvent($"Successfully escaped from the enemy {current.Enemies[0].Name}!", FieldAnimType.Flee, EnemyAnimType.Despawn, PuppetAnimType.None, 200));
    }

    public void Cleanup()
    {
        current = null;
        CombatUi.Cleanup();
    }
}
