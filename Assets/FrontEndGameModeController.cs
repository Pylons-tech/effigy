using GameData;
using GameTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FrontEndGameModeController : MonoBehaviour
{
    public const int LOAD_FIRST_SCENE = 0;
    public const int FRONT_END_SCENE = 1;
    public const int MAP_SCENE = 2;
    public const int BATTLE_SCENE = 3;
    
    private bool BattleSceneIsLoaded => SceneManager.GetSceneByBuildIndex(BATTLE_SCENE).isLoaded;
    private Battle lastBattle;

    public static FrontEndGameModeController Instance { get; private set; }

    void Awake ()
    {
        Instance = this;
    }

    private void LoadLastBattle ()
    {
        // We don't have battle-struct-on-chain yet so we can't load a battle in progress
        lastBattle = null;
    }

    /// <summary>
    /// used for the frontend temp dropdown
    /// </summary>
    public void StartBattleFromInt(int v)
    {
        if (v > 0) StartBattle((EncounterType)v);
    }

    public void StartBattle (EncounterType encounterType)
    {    
        if (BattleSceneIsLoaded)
        {  
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(BATTLE_SCENE));
            CombatHost.Instance.gameObject.SetActive(true);
            if (CombatHost.Instance.current != null) throw new System.Exception("Should not be attempting to start a battle with one ongoing");
            CombatHost.Instance.StartEncounter(encounterType);
        }
        else
        {
            SceneManager.LoadSceneAsync(BATTLE_SCENE, LoadSceneMode.Additive).completed += (operation) =>
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(BATTLE_SCENE));
                CombatHost.Instance.StartEncounter(encounterType);
            };
        }
    }

    public void ReturnFromCombat ()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(FRONT_END_SCENE));
        CombatHost.Instance.Cleanup();
        CombatHost.Instance.gameObject.SetActive(false);
    }
}
