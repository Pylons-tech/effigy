using GameTypes;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FrontEndPartyController : MonoBehaviour
{
    public readonly List<Puppet> AllPuppets = new List<Puppet>();
    public Puppet[] Party { get; private set; } = new Puppet[4];

    private void LoadInitialPartyState ()
    {
        // nop - chain isn't active yet
    }

    public void GetFreeStarterPuppet ()
    {
        if (!HasAnyValidPuppets)
        {
            AddPuppet(Puppet.GetStarter());
        }
        else throw new Exception("Can't get another starter ");
    }

    public bool HasAnyValidPuppets
    {
        get
        {
            foreach (var p in AllPuppets) if (p.State != PuppetState.Dead) return true;
            return false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddPuppet (Puppet puppet)
    {

    }
}
