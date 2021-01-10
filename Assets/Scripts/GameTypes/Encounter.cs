using GameData;

namespace GameTypes
{
    public readonly struct Encounter
    {
        public readonly EnemyType[] Enemies;

        public Encounter(EnemyType[] enemies) => Enemies = enemies;

        public bool IsValid => Enemies != null && Enemies.Length > 0;
    }
}