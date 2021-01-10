using GameTypes;

namespace GameData.EnemyData
{
    public static partial class EnemyTable
    {
        [EnemyType(EnemyType.RogueGoblinoid)]
        public static EnemyBase RogueGoblinoid => new EnemyBase(
                enemyType: EnemyType.RogueGoblinoid,
                name: "Rogue Goblinoid",
                element: PuppetElement.Dark,
                maxHp: 40,
                attack: 10,
                defense: 3,
                speed: 4,
                level: 1,
                mana: 100
            );
    }
}