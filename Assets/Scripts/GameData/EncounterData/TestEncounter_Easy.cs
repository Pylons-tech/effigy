using GameTypes;

namespace GameData.EncounterData
{
    public static partial class EncounterTable
    {
        [EncounterType(EncounterType.TestEncounter_Easy)]
        public static Encounter TestEncounter_Easy => new Encounter(
            enemies: new EnemyType[]{ EnemyType.RogueGoblinoid });
    }
}