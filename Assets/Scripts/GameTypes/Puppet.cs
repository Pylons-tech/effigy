using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameTypes
{
    public class Puppet
    {
        const float ELEMENT_VALUE_MOD = 1.15f;
        public string Name { get; private set; }
        public PuppetElement Element { get; private set; }
        public int LightPts { get; private set; }
        public int DarkPts { get; private set; }
        public int EarthPts { get; private set; }
        public PuppetState State { get; private set; }
        public int MaxHP { get; private set; }
        public int CurrentHP { get; private set; }
        public int Attack { get; private set; }
        public int Defense { get; private set; }
        public int Speed { get; private set; }
        public int Level { get; private set; }
        public int RemainingEvolutions { get; private set; }
        public int Mana { get; private set; }
        public int Value { get; private set; }

        public Puppet(string name, PuppetElement element, int lightPts, int darkPts, int earthPts, PuppetState state, int maxHP, int currentHP, int attack, int defense, int speed, int level, int remainingEvolutions, int mana)
        {
            Name = name;
            Element = element;
            LightPts = lightPts;
            DarkPts = darkPts;
            EarthPts = earthPts;
            State = state;
            MaxHP = maxHP;
            CurrentHP = currentHP;
            Attack = attack;
            Defense = defense;
            Speed = speed;
            Level = level;
            RemainingEvolutions = remainingEvolutions;
            Mana = mana;
            CalcValue();
        }

        public static Puppet GetStarter ()
        {
            return new Puppet(
                name: "foo",
                element: PuppetElement.None,
                lightPts: 0,
                darkPts: 0,
                earthPts: 0,
                state: PuppetState.New,
                maxHP: 30,
                currentHP: 0,
                attack: 15,
                defense: 15,
                speed: 15,
                level: 1,
                remainingEvolutions: 0,
                mana: 0);
        }

        public static Puppet GetPuppetOfRoughValue (int value)
        {
            var element = PuppetElement.None;
            var lightPts = 0;
            var darkPts = 0;
            var earthPts = 0;
            if (Random.Range(0, 10) >= 3)
            {
                element = (PuppetElement)Random.Range(1, 4);
                value = Mathf.CeilToInt(value / ELEMENT_VALUE_MOD);
                if (element == PuppetElement.Light) lightPts += 5;
                else if (element == PuppetElement.Dark) darkPts += 5;
                else if (element == PuppetElement.Earth) earthPts += 5;
            }
            value = Random.Range(Mathf.FloorToInt(value * 0.9f), Mathf.CeilToInt(value * 1.1f));
            var hpShare = 0;
            var atkShare = 0;
            var defShare = 0;
            var spdShare = 0;
            for (int i = 0; i < 100; i++)
            {
                var r = Random.Range(0, 100);
                if (r <= 33) hpShare++;
                else if (r <= 55) atkShare++;
                else if (r <= 77) defShare++;
                else spdShare++;
            }
            var derivedHp = Mathf.CeilToInt((hpShare / 100f) * value);
            var derivedAtk = Mathf.CeilToInt((atkShare / 100f) * value);
            var derivedDef = Mathf.CeilToInt((defShare / 100f) * value);
            var derivedSpd = Mathf.CeilToInt((spdShare / 100f) * value);

            var level = 1;
            if (element != PuppetElement.None) level++;
            level += (derivedHp - 30) / (33 / 100);
            level += (derivedAtk - 15) / (22 / 100);
            level += (derivedDef - 15) / (22 / 100);
            level += (derivedSpd - 15) / (22 / 100);
            return new Puppet(
                name: "foo",
                element: element,
                lightPts: lightPts,
                darkPts: darkPts,
                earthPts: earthPts,
                state: PuppetState.New,
                maxHP: derivedHp,
                currentHP: 0,
                attack: derivedAtk,
                defense: derivedDef,
                speed: derivedSpd,
                level: level,
                remainingEvolutions: 0,
                mana: 0);
        }

        public static PuppetElement GetElement (int lightPts, int darkPts, int earthPts)
        {
            if (lightPts > darkPts)
            {
                if (lightPts >= earthPts) return PuppetElement.Light;
                else return PuppetElement.Earth;
            }
            else if (earthPts > darkPts) return PuppetElement.Earth;
            else if (darkPts > lightPts) return PuppetElement.Dark;
            else return PuppetElement.None;
        }

        public void Live (Action<InteractionResult> callback)
        {
            State = PuppetState.Alive;
            RemainingEvolutions = 4;
            CurrentHP = MaxHP;
            Mana = 0;
            callback(new InteractionResult(InteractionState.Success));
        }

        public void Die (Action<InteractionResult> callback)
        {
            State = PuppetState.Dead;
            RemainingEvolutions = 0;
            CurrentHP = 0;
            Mana = 0;
            callback(new InteractionResult(InteractionState.Success));
        }

        public void Sacrifice (Action<InteractionResult> callback)
        {
            Inventory.Mana += Mana;
            State = PuppetState.Dead;
            RemainingEvolutions = 0;
            CurrentHP = 0;
            Mana = 0;
            callback(new InteractionResult(InteractionState.Success));
        }

        public void Damage (int dmg)
        {
            CurrentHP -= dmg;
            if (CurrentHP < 0) CurrentHP = 0;
        }

        public void AwardMana (int mana)
        {
            Mana += mana;
        }

        public void Feed (Relic relic, Action<InteractionResult> callback)
        {
            LightPts += relic.LightPts;
            DarkPts += relic.DarkPts;
            EarthPts += relic.EarthPts;
            Element = GetElement(LightPts, DarkPts, EarthPts);
            CurrentHP += relic.HP;
            if (CurrentHP < 1) CurrentHP = 1; // feeding a puppet a relic can't kill it
            MaxHP += relic.HP;
            Attack += relic.Attack;
            Defense += relic.Defense;
            Speed += relic.Speed;
            Level += relic.Level;
            RemainingEvolutions += 1;
            Mana += relic.Mana;
            CalcValue();
            Inventory.Remove(relic);
            callback(new InteractionResult(InteractionState.Success));
        }

        public void CalcValue ()
        {
            Value = (MaxHP + Attack + Defense + Speed) * 10;
            if (Element != PuppetElement.None) Value = Mathf.CeilToInt(Value * ELEMENT_VALUE_MOD);
        }
    }
}