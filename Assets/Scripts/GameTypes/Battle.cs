using GameData;
using System;
using System.Collections.Generic;

namespace GameTypes
{
    public class Battle
    {
        public class BattleEventArgs : EventArgs
        {
            public readonly Enemy[] Enemies;
            public readonly Puppet[] Puppets;
            public readonly Relic[] Relics;
            public readonly int Round;

            public BattleEventArgs(Enemy[] enemy, Puppet[] puppets, Relic[] relics, int round)
            {
                Enemies = enemy;
                Puppets = puppets;
                Relics = relics;
                Round = round;
            }

            public BattleEventArgs(Enemy[] enemy, Puppet[] puppets, int round)
            {
                Enemies = enemy;
                Puppets = puppets;
                Round = round;
            }
        }

        public event EventHandler<BattleEventArgs> OnPuppetDies;
        public event EventHandler<BattleEventArgs> OnEnemyDies;
        public event EventHandler<BattleEventArgs> OnRetreated;
        public event EventHandler<BattleEventArgs> OnRelics;
        public event EventHandler<BattleEventArgs> OnNextRound;
        public event EventHandler<BattleEventArgs> OnBattleLost;
        public const int PARTY_SIZE = 4;

        public bool Ended { get; private set; }
        public List<Puppet> Party { get; private set; } = new List<Puppet>();
        bool[] knownDead = { false, false, false, false };
        public Enemy[] Enemies { get; private set; }
        public int Round { get; private set; }
        public EncounterType Encounter { get; private set; }

        public Battle(EncounterType encounter, params Puppet[] puppets)
        {
            if (puppets.Length < 1) throw new Exception("Can't start a battle without a party!");
            else if (puppets.Length > PARTY_SIZE)
                throw new Exception($"Can't start battle w/ {puppets.Length} puppets - larger than {PARTY_SIZE}");
            foreach (var p in puppets)
            {
                if (p.State == PuppetState.New) throw new Exception("Shouldn't be starting a battle w/ a new puppet"); 
                // don't ask why this exception is here or what dumb, dumb things happened that make this an error
                // we're explicitly looking for
                Party.Add(p);
            }
            Encounter = encounter;
            var enemies = EncounterTable.Get(Encounter).Enemies;
            Enemies = new Enemy[enemies.Length];
            for (int i = 0; i < Enemies.Length; i++) Enemies[i] = EnemyTable.Get(enemies[i]).Instantiate();
        }

        void GenerateRelics()
        {
            var relics = new Relic[UnityEngine.Random.Range(1, 4)];
            for (int i = 0; i < relics.Length; i++) relics[i] = Inventory.Add(Relic.GenerateRelicForEnemyLevel(Enemies[0].Level));
            OnRelics?.Invoke(this, new BattleEventArgs(Enemies, Party.ToArray(), relics, Round));
        }

        public void DoCombat(bool retreat)
        {
            Round++;
            Enemies[0].Combat(Party.ToArray(), retreat);
            var dead = new List<Puppet>();
            int deathCount = 0;
            for (int i = 0; i < Party.Count; i++)
            {
                if (Party[i].State == PuppetState.Dead && !knownDead[i])
                {
                    dead.Add(Party[i]);
                    knownDead[i] = true;
                }
                if (knownDead[i]) deathCount++;
            }

            if (dead.Count > 0) OnPuppetDies?.Invoke(this, new BattleEventArgs(Enemies, dead.ToArray(), Round));
            if (deathCount == Party.Count)
            {
                OnBattleLost?.Invoke(this, new BattleEventArgs(Enemies, Party.ToArray(), Round));
                Ended = true;
            }
            else if (Enemies[0].State == PuppetState.Dead)
            {
                GenerateRelics();
                OnEnemyDies?.Invoke(this, new BattleEventArgs(Enemies, Party.ToArray(), Round));
                Ended = true;
            }
            else if (retreat)
            {
                OnRetreated?.Invoke(this, new BattleEventArgs(Enemies, Party.ToArray(), Round));
                Ended = true;
            }
            else OnNextRound?.Invoke(this, new BattleEventArgs(Enemies, Party.ToArray(), Round));
        }
    }
}