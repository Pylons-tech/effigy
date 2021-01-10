using UnityEngine;

namespace GameTypes
{
    public class Relic
    {
        public Relic(string name, int lightPts, int darkPts, int earthPts, int hP, int attack, int defense, int speed, int mana, int level)
        {
            Name = name;
            LightPts = lightPts;
            DarkPts = darkPts;
            EarthPts = earthPts;
            HP = hP;
            Attack = attack;
            Defense = defense;
            Speed = speed;
            Mana = mana;
            Level = level;
        }

        public string Name { get; private set; }
        public int LightPts { get; private set; }
        public int DarkPts { get; private set; }
        public int EarthPts { get; private set; }
        public int HP { get; private set; }
        public int Attack { get; private set; }
        public int Defense { get; private set; }
        public int Speed { get; private set; }
        public int Mana { get; private set; }
        public int Level { get; private set; }
       

        public static Relic GenerateRelicForEnemyLevel (int enemyLevel)
        {
            var relicLevel = 1 + (Mathf.FloorToInt(enemyLevel / 5));
            if (relicLevel > 10) relicLevel = 10;
            var element = PuppetElement.None;
            if (Random.Range(0, 2) == 1) element = (PuppetElement)Random.Range(1, 4);
            var lightPts = 0;
            var darkPts = 0;
            var earthPts = 0;
            var hp = 0;
            var atk = 0;
            var def = 0;
            var spd = 0;
            for (int i = 0; i < relicLevel + 1; i++)
            {
                switch (element)
                {
                    case PuppetElement.Light:
                        lightPts += Random.Range(0, 5);
                        break;
                    case PuppetElement.Dark:
                        darkPts += Random.Range(0, 5);
                        break;
                    case PuppetElement.Earth:
                        earthPts += Random.Range(0, 5);
                        break;
                }
                hp += Random.Range(2, 9);
                atk += Random.Range(0, 6);
                def += Random.Range(0, 6);
                spd += Random.Range(0, 6);
            }

            return new Relic("foo", lightPts, darkPts, earthPts, hp, atk, def, spd, 0, relicLevel);
        }
    }
}