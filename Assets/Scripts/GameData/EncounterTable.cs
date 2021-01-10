using GameTypes;
using System;
using System.Reflection;

namespace GameData
{
    public static partial class EncounterTable
    {
        public static readonly Encounter[] Encounters;

        static EncounterTable()
        {
            var arr = new Encounter[Enum.GetValues(typeof(EncounterType)).Length];
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    foreach (var prop in type.GetProperties(BindingFlags.Static | BindingFlags.Public))
                    {
                        var attr = prop.GetCustomAttribute<EncounterTypeAttribute>();
                        if (prop.PropertyType == typeof(Encounter) && attr != null)
                        {
                            if (arr[(int)attr.EncounterType].IsValid)
                                throw new Exception($"At least two definitions exist for encounter {attr.EncounterType}. Failed to populate encounter table.");
                            arr[(int)attr.EncounterType] = (Encounter)prop.GetValue(null, new object[0]);
                        }
                    }
                }
            }
            Encounters = arr;
        }

        public static Encounter Get(EncounterType et) => Encounters[(int)et];
    }
}