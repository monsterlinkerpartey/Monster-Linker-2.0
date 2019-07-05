using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasyEnemyType : EnemyStateMachine
{
    [Header("Values for only this type of enemy")]
    [Tooltip("Drag n drop enemy for the arena here TOO")]
    public EnemyEasy curEnemy;

    [Tooltip("Base Attack Pattern, 5 Slots")]
    [SerializeField] List<BaseAttack> BA_normal_noRP1;
    [Tooltip("Base Attack Pattern, 5 Slots")]
    [SerializeField] List<BaseAttack> BA_normal_noRP2;
    [Tooltip("Base Attack Pattern, 5 Slots")]
    [SerializeField] List<BaseAttack> BA_normal_FA1;
    [Tooltip("Base Attack Pattern, 5 Slots")]
    [SerializeField] List<BaseAttack> BA_lowHP_FA2;
    [Tooltip("Base Attack Pattern, 5 Slots")]
    [SerializeField] List<BaseAttack> BA_lowHP_noFA;

    [SerializeField] int normal_noRP_State = 0;

    public override void GetEnemyValues()
    {
        base.GetEnemyValues();

        BA_normal_noRP1 = curEnemy.BA_normal_noRP1;
        BA_normal_noRP2 = curEnemy.BA_normal_noRP2;
        BA_normal_FA1 = curEnemy.BA_normal_FA1;
        BA_lowHP_FA2 = curEnemy.BA_lowHP_FA2;
        BA_lowHP_noFA = curEnemy.BA_lowHP_noFA;
    }

    public override void CheckEnemyState()
    {
        base.CheckEnemyState();
        
        //wenn höher als 50% HP
        if (baeffectshandler.curEnemyHP > (baeffectshandler.maxEnemyHP * 0.5))
        {
            //wenn RP höher als 3er FA cost
            if (baeffectshandler.curEnemyRP >= FA3_RPCost)
            {
                EnemyState(eEnemyState.Normal_FA3Use);                
            }
            else
            {
                EnemyState(eEnemyState.Normal_NoRP);              

            }
        }
        else
        {
            //wenn RP höher als 3er FA cost
            if (baeffectshandler.curEnemyRP >= FA3_RPCost)
            {

                EnemyState(eEnemyState.LowHP_FA3Use);              
            }
            else
            {
                EnemyState(eEnemyState.LowHP_NoRP);              

            }
        }
    }

    public override void EnemyState(eEnemyState EnemyState)
    {
        switch (EnemyState)
        {
            case eEnemyState.Normal_FA3Use:
                curBaseAttackInput = BA_normal_FA1;
                WriteAttackList();
                CheckForFeral();
                break;
            case eEnemyState.Normal_NoRP:
                if (normal_noRP_State > 2)
                    normal_noRP_State = 0;
                ChooseAttack();
                WriteAttackList();
                break;
            case eEnemyState.LowHP_FA3Use:
                curBaseAttackInput = BA_lowHP_FA2;
                WriteAttackList();
                CheckForFeral();
                break;
            case eEnemyState.LowHP_NoRP:
                curBaseAttackInput = BA_lowHP_noFA;
                WriteAttackList();
                break;
            default:
                curBaseAttackInput = BA_normal_noRP1;
                Debug.LogError("couldnt find enemy status, reverting to default");
                break;
        }

        base.EnemyState(EnemyState);
    }

    public override void WriteAttackList()
    {
        base.WriteAttackList();
    }

    public override void ChooseAttack()
    {
        base.ChooseAttack();

        normal_noRP_State += 1;

        switch(normal_noRP_State)
        {
            case 1:
                curBaseAttackInput = BA_normal_noRP1;
                print("attack input 1");
                break;
            case 2:
                curBaseAttackInput = BA_normal_noRP2;
                print("attack input 2");
                break;
            default:
                print("ERROR: No BA Input for EnemyState found");
                break;
        }
    }

    public override void CheckForFeral()
    {
        base.CheckForFeral();
    }

    public override void SetInput()
    {
        base.SetInput();
    }

    public override void ClearInput()
    {
        base.ClearInput();
    }
}
