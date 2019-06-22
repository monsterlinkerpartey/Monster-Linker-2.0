using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    [Header("Values all enemies share")]
    [Tooltip("Drag n drop enemy for the arena here")]
    public Enemy enemy;

    public List<BaseAttack> curAttackInput;

    [HideInInspector]
    public ArenaUIHandler arenaui;
    [HideInInspector]
    public InitiativeCheck initiativecheck;
    [HideInInspector]
    public List<FeralArt> FAs;
    [HideInInspector]
    public int maxHitPoints;
    [HideInInspector]
    public int maxInputSlots;
    [HideInInspector]
    public float curHP;
    [HideInInspector]
    public float curRP;
       
    public virtual void GetEnemyValues()
    {
        maxHitPoints = enemy.MaxHitPoints;
        maxInputSlots = enemy.MaxInputSlots;
        curRP = 0;
        curHP = maxHitPoints;
        FAs = enemy.FAs;
    }

    //thresholds: bleiben gleich in jedem type
    public virtual void CheckEnemyState()
    {
        print("setting enemy state");
        //if own hp
        //if own rp
        //if player hp
    }

    //BA inputs sind unterschiedlich bei jedem gegner
    public virtual void EnemyState(eEnemyState EnemyState)
    {        
        print("current enemy state: " + EnemyState);
    }

    public virtual void ChooseAttack()
    {
        print("choosing an attack pattern");
        //go through the attacks in order
        //or random depending on enemy type
    }

    public virtual void CheckForFeral()
    {
        print("checking for FA");
        //check if RP enough?
        //check if FA
        //mark it or some shit               
    }

    public virtual void SetInput()
    {
        print("updating enemy input UI");
        initiativecheck.curEnemyInput = curAttackInput;
        arenaui.UpdateEnemyInput(curAttackInput);
        //update ui to current attack input
    }

    public virtual void ClearInput()
    {
        curAttackInput.Clear();
        arenaui.UpdateEnemyInput(curAttackInput);
    }
}
