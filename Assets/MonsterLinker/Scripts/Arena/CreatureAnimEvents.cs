using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureAnimEvents : MonoBehaviour
{
    public AnimationHandler animationhandler;
    public AttackRoundHandler attackroundhandler;
    public QTEHandler qtehandler;

    //QTEStateSwitch(eQTEState QTEState)

    public void AttackAnimStart()
    {        
        StartCoroutine(qtehandler.WaitForStart());
    }

    public void HitEnemy()
    {
        animationhandler.EnemyAnim.SetTrigger("hurt");
    }

    public void HitPlayer()
    {
        animationhandler.PlayerAnim.SetTrigger("hurt");
    }

    public void AttackAnimEnd()
    {
        attackroundhandler.NextAttack();
    }
}
