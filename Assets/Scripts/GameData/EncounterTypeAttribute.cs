using System;

namespace GameData
{
    public class EncounterTypeAttribute : Attribute
    {
        public readonly EncounterType EncounterType;

        public EncounterTypeAttribute(EncounterType encounterType) => EncounterType = encounterType;
    }
}