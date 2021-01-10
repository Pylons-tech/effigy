using GameTypes;
using System;
using System.Reflection;

namespace GameData
{
    public static partial class EnemyTable
    {
        public static readonly EnemyBase[] Enemies;

        static EnemyTable()
        {
            var arr = new EnemyBase[Enum.GetValues(typeof(EnemyType)).Length];
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    foreach (var prop in type.GetProperties(BindingFlags.Static | BindingFlags.Public))
                    {
                        var attr = prop.GetCustomAttribute<EnemyTypeAttribute>();
                        if (prop.PropertyType == typeof(EnemyBase) && attr != null)
                        {
                            if (arr[(int)attr.EnemyType].EnemyType != EnemyType.None)
                                throw new Exception($"At least two definitions exist for enemy {attr.EnemyType}. Failed to populate encounter table.");
                            arr[(int)attr.EnemyType] = (EnemyBase)prop.GetValue(null, new object[0]);
                        }
                    }
                }
            }
            Enemies = arr;
        }

        public static EnemyBase Get(EnemyType et) => Enemies[(int)et];
    }
}