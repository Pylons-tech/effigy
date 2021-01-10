using System;

namespace GameData
{
    public class EnemyTypeAttribute : Attribute
    {
        public readonly EnemyType EnemyType;

        public EnemyTypeAttribute(EnemyType enemyType) => EnemyType = enemyType;
    }
}