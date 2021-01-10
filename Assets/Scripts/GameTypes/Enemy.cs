using GameData;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameTypes
{
    public partial class Enemy
    {
        public Enemy(EnemyType enemyType, string name, PuppetElement element, PuppetState state, int maxHP, int currentHP, int attack, int defense, int speed, int level, int mana)
        {
            EnemyType = enemyType;
            Name = name;
            Element = element;
            State = state;
            MaxHP = maxHP;
            CurrentHP = currentHP;
            Attack = attack;
            Defense = defense;
            Speed = speed;
            Level = level;
            Mana = mana;
        }

        public EnemyType EnemyType { get; private set; }

        public string Name { get; private set; }
        public PuppetElement Element { get; private set; }
        public PuppetState State { get; private set; }
        public int MaxHP { get; private set; }
        public int CurrentHP { get; private set; }
        public int Attack { get; private set; }
        public int Defense { get; private set; }
        public int Speed { get; private set; }
        public int Level { get; private set; }
        public int Mana { get; private set; }


        public void Damage(int dmg)
        {
            CurrentHP -= dmg;
            if (CurrentHP < 0) CurrentHP = 0;
        }

        public void Live(Action<InteractionResult> callback)
        {
            State = PuppetState.Alive;
            CurrentHP = MaxHP;
            callback(new InteractionResult(InteractionState.Success));
        }

        public void Die(Action<InteractionResult> callback)
        {
            State = PuppetState.Dead;
            CurrentHP = 0;
            callback(new InteractionResult(InteractionState.Success));
        }

        public void Combat (Puppet[] puppets, bool retreat)
        {
            foreach (var puppet in puppets)
            {
                if (puppet.State != PuppetState.Alive) continue;
                var thisElemMod = 1.0f;
                var puppetElemMod = 1.0f;
                if (Element != PuppetElement.None && puppet.Element != PuppetElement.None)
                {
                    if (puppet.Element.Trump() == Element) thisElemMod = 1.33f;
                    else if (Element.Trump() == puppet.Element) puppetElemMod = 1.33f;
                }
                var r = Random.Range(0.9f, 1.1f);
                var thisAtk = Mathf.CeilToInt(thisElemMod * Attack);
                var thisDef = Mathf.CeilToInt(thisElemMod * Defense);
                var thisSpd = Mathf.CeilToInt(thisElemMod * Speed);
                var puppetAtk = Mathf.CeilToInt(puppetElemMod * puppet.Attack * r);
                var puppetDef = Mathf.CeilToInt(puppetElemMod * puppet.Defense * r);
                var puppetSpd = Mathf.CeilToInt(puppetElemMod * puppet.Speed * r);
                var damage = 0;

                void DoThisAttack ()
                {
                    if (retreat) puppetDef += puppetSpd / 2;
                    damage = thisAtk - puppetDef;
                    if (damage < 1) damage = 1;
                    puppet.Damage(damage);
                }

                if (thisSpd > puppetSpd) DoThisAttack();
                if (!retreat)
                {
                    damage = puppetAtk - thisDef;
                    if (damage < 1) damage = 1;
                    Damage(damage);
                }
                if (puppetSpd >= thisSpd) DoThisAttack();

                if (CurrentHP < 1) Die((result) => { Debug.Log($"{Name} died: {result.State}"); });
                if (puppet.CurrentHP < 1) puppet.Die((result) => { Debug.Log($"{puppet.Name} died: {result.State}"); });
                if (State == PuppetState.Dead)
                {
                    foreach (var p in puppets) p.AwardMana(Mathf.CeilToInt(Mana / puppets.Length));
                    break;
                }
            }
        }
    }
}