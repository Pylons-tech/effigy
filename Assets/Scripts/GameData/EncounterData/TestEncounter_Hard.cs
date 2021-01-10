using GameTypes;

namespace GameData.EncounterData
{
    public static partial class EncounterTable
    {
        [EncounterType(EncounterType.TestEncounter_Hard)]
        public static Encounter TestEncounter_Hard => new Encounter(
            enemies: new EnemyType[]{ EnemyType.BigGreenMeanie });
    }
}