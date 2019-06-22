﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VeryEasyEnemyType : EnemyStateMachine
{
    [Header("Values for only this type of enemy")]
    [Tooltip("Drag n drop enemy for the arena here TOO")]
    public EnemyVeryEasy curEnemy;
    [Tooltip("Base Attack Pattern, 5 Slots")]
    [SerializeField] List<BaseAttack> BA_normal1;
    [Tooltip("Base Attack Pattern, 5 Slots")]
    [SerializeField] List<BaseAttack> BA_normal2;
    [Tooltip("Base Attack Pattern, 5 Slots")]
    [SerializeField] List<BaseAttack> BA_normal3;

    [SerializeField] int normalState = 0;

    public override void GetEnemyValues()
    {
        base.GetEnemyValues();
        
        BA_normal1 = curEnemy.BA_normal1;
        BA_normal2 = curEnemy.BA_normal2;
        BA_normal3 = curEnemy.BA_normal3;
    }

    public override void CheckEnemyState()
    {
        base.CheckEnemyState();

        EnemyState(eEnemyState.Normal);
    }

    public override void EnemyState(eEnemyState EnemyState)
    {
        switch (EnemyState)
        {
            case eEnemyState.Normal:
                if (normalState > 2)
                    normalState = 0;

                ChooseAttack();
                break;
        }

        base.EnemyState(EnemyState);
    }

    public override void ChooseAttack()
    {
        base.ChooseAttack();   
        
        normalState += 1;

        switch(normalState)
        {
            case 1:
                curAttackInput = BA_normal1;
                print("attack input 1");
                break;
            case 2:
                curAttackInput = BA_normal2;
                print("attack input 2");
                break;
            case 3:
                curAttackInput = BA_normal3;
                print("attack input 3");
                break;
            default:
                print("ERROR: No BA Input for EnemyState found");
                break;
        }
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