using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : MonoBehaviour
{
    public static FightManager Instance;

    //TODO: warum benutzt der switch nicht dieses enum? 
    public eArenaState ArenaState;

    //TODO: List of all scripts in the arena scene

    void Start()
    {
        #region Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        #endregion
        SwitchState(eArenaState.Intro);
    }


    void Update()
    {
        
    }

    //will be called by other scripts, update the arenastate and then run functions from the scripts
    public void SwitchState(eArenaState ArenaState)
    {
        switch (ArenaState)
        {
            case eArenaState.Intro:
                print("arena state: " + ArenaState);
                StartCoroutine(Test());
                break;
            case eArenaState.PlayerInput:
                break;
            case eArenaState.EnemyInput:
                break;
            case eArenaState.AdvantageCheck:
                print("arena state: " + ArenaState);
                StartCoroutine(Test2());
                break;
            case eArenaState.QTEAttack:
                break;
            case eArenaState.QTEBlock:
                break;
            case eArenaState.NextRound:
                break;
            case eArenaState.Result:
                break;
        }
    }

    IEnumerator Test()
    {
        print("now waiting");
        yield return new WaitForSeconds(3f);
        print("switching to arena state: advantage");
        SwitchState(eArenaState.AdvantageCheck);
    }

    IEnumerator Test2()
    {
        print("now waiting");
        yield return new WaitForSeconds(3f);
        print("switching to arena state: input");
        SwitchState(eArenaState.Intro);
    }
}