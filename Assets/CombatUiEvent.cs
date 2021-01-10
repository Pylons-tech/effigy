using GameTypes;
using System;

public class CombatUiEvent : EventArgs
{
    public readonly string TextboxString;
    public readonly FieldAnimType FieldAnim;
    public readonly EnemyAnimType EnemyAnim;
    public readonly PuppetAnimType PuppetAnim;
    public readonly int Priority;

    public CombatUiEvent(string textboxString, FieldAnimType fieldAnim, EnemyAnimType enemyAnim, PuppetAnimType puppetAnim, int priority)
    {
        TextboxString = textboxString;
        FieldAnim = fieldAnim;
        EnemyAnim = enemyAnim;
        PuppetAnim = puppetAnim;
        Priority = priority;
    }
}