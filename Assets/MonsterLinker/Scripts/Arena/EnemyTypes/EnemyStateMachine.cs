using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    [Header("Values all enemies share")]
    [Tooltip("Drag n drop enemy for the arena here")]
    public Enemy enemy;

    //Attacks for UI and INI check
    public List<BaseAttack> curBaseAttackInput;
    //Attacks for DMG and animation
    public List<Attack> curAttackInput;

    //For chain and FA checks
    public List<FAChain> possibleChains;
    public List<FeralArt> possibleFAs;
    public List<int> BAsToDelete;

    public int slotcount = 5;
    public int InputNo = 0;
    public int FANo = 0;
    public int Pos = 0;

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

    public int FA3_RPCost;
    public int FA4_RPCost;
    public int FA5_RPCost;

    public int Chain_RPCost;

    public eEnemyState enemyState;

    public BAEffectsHandler baeffectshandler;

    public virtual void GetEnemyValues()
    {
        maxHitPoints = enemy.MaxHitPoints;
        maxInputSlots = enemy.MaxInputSlots;
        curRP = 0;
        curHP = maxHitPoints;
        FAs = enemy.FAs;
    }

    public virtual void WriteAttackList()
    {
        foreach (Attack attack in curBaseAttackInput)
        {
            curAttackInput.Add(attack);
        }
    }

    //thresholds: bleiben gleich in jedem type
    public virtual void CheckEnemyState()
    {
        print("setting enemy state");
    }

    //BA inputs sind unterschiedlich bei jedem gegner
    public virtual void EnemyState(eEnemyState EnemyState)
    {
        enemyState = EnemyState;
        print("current enemy state: " + EnemyState);
    }

    public virtual void ChooseAttack()
    {
        print("choosing an attack pattern");
        //go through the attacks in order
        //or random depending on enemy type
    }

    public virtual void CheckForChain()
    {
        foreach (FAChain chain in possibleChains)
        {
            if (curBaseAttackInput.SequenceEqual(chain.ChainInputList))
            {
                for (int i = 0; i < 5; i++)
                {
                    BAsToDelete.Add(i);
                    print("i: " + i);
                }

                curAttackInput.Clear();
                curAttackInput = chain.NeededFeralArts;
                //arenaui.VisializeFAs(BAsToDelete, Color.yellow);
            }
            else
            {
                print("no chainable FAs equal");
                CheckForFeral();
            }
        }
    }

    public virtual void CheckForFeral()
    {
        //check if FA      
        print("checking for FA");

    startFAloop:
        while (FANo < possibleFAs.Count)
        {
            print("cur FA: " + possibleFAs[FANo].name);

            if (possibleFAs[FANo].FeralArtInput.Count > (slotcount - Pos))
            {
                FANo += 1;
                goto startFAloop;
            }

            for (InputNo = 0; InputNo < possibleFAs[FANo].FeralArtInput.Count; InputNo++)
            {
                print("looping through curFA, cur at Input no: " + InputNo);

                if (possibleFAs[FANo].FeralArtInput[InputNo] == curBaseAttackInput[InputNo + Pos])
                {
                    //check next input
                    print("positive, checking next input");
                    //save BA pos in list -> int list
                    BAsToDelete.Add(InputNo + Pos);

                }
                else
                {
                    //check next FA
                    print("negative, checking next FA");
                    BAsToDelete.Clear();
                    InputNo = 0;
                    FANo += 1;
                    goto startFAloop;
                }
            }

            //If it is a feral art
            if (BAsToDelete.Count == possibleFAs[FANo].FeralArtInput.Count)
            {
                print("FA found, adding " + possibleFAs[FANo].name + " to list");

                for (int i = BAsToDelete.Count; i > 0; i--)
                    curAttackInput.RemoveAt(BAsToDelete[i - 1]);

                curAttackInput.Insert(BAsToDelete[0], possibleFAs[FANo]);
                arenaui.VisializeFAs(BAsToDelete, Color.magenta);
                Pos += BAsToDelete.Count;
                FANo = 0;
                //return;
            }
            else
            {
                BAsToDelete.Clear();
                print("no FA found, next FA?");
                FANo += 1;
            }
        }

        if (Pos < 2)
        {
            print("5 slots, pos = " + Pos + " , keep running FA check");
            FANo = 0;
            Pos += 1;
            goto startFAloop;
        }
        else
        {
            print("do not run FA check anymore");
            return;
        }
    }

    public virtual void SetInput()
    {
        print("updating enemy input UI");
        initiativecheck.curEnemyInput = curBaseAttackInput;
        arenaui.UpdateEnemyInput(curBaseAttackInput);
        //baeffectshandler.curEnemyAttacks = curAttackInput;
        //update ui to current attack input
    }

    public virtual void ClearInput()
    {
        curAttackInput.Clear();
        arenaui.UpdateEnemyInput(curBaseAttackInput);
    }
}
