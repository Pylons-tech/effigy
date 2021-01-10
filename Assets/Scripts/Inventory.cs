using GameTypes;
using System.Collections.Generic;

public static class Inventory
{
    public static int Mana;
    public static List<Puppet> Puppets = new List<Puppet>();
    public static List<Relic> Relics = new List<Relic>();

    public static Puppet Remove (Puppet puppet)
    {
        Puppets.Remove(puppet);
        return puppet;
    }

    public static Relic Add(Relic relic)
    {
        Relics.Add(relic);
        return relic;
    }

    public static Puppet Add(Puppet puppet)
    {
        Puppets.Add(puppet);
        return puppet;
    }

    public static Relic Remove (Relic relic)
    {
        Relics.Remove(relic);
        return relic;
    }
}