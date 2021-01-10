using GameTypes;

namespace GameData.EnemyData
{
    public static partial class EnemyTable
    {
        [EnemyType(EnemyType.BigGreenMeanie)]
        public static EnemyBase BigGreenMeanie => new EnemyBase(
                enemyType: EnemyType.BigGreenMeanie,
                name: "Big Green Meanie",
                element: PuppetElement.Earth,
                maxHp: 105,
                attack: 23,
                defense: 11,
                speed: 1,
                level: 5,
                mana: 250
            );
    }
}