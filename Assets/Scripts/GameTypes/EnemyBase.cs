using GameData;

namespace GameTypes
{
    public readonly struct EnemyBase
    {
        public readonly EnemyType EnemyType;
        public readonly string Name;
        public readonly PuppetElement Element;
        public readonly int MaxHp;
        public readonly int Attack;
        public readonly int Defense;
        public readonly int Speed;
        public readonly int Level;
        public readonly int Mana;

        public EnemyBase(EnemyType enemyType, string name, PuppetElement element, int maxHp, int attack, int defense, int speed, int level, int mana)
        {
            EnemyType = enemyType;
            Name = name;
            Element = element;
            MaxHp = maxHp;
            Attack = attack;
            Defense = defense;
            Speed = speed;
            Level = level;
            Mana = mana;
        }

        public Enemy Instantiate ()
        {
            return new Enemy(EnemyType, Name, Element, PuppetState.Alive, MaxHp, MaxHp, Attack, Defense, Speed, Level, Mana);
        }
    }
}