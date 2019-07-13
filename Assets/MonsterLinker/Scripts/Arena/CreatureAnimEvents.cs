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
        print("animation starts, call qte");
        StartCoroutine(qtehandler.WaitForStart());
    }

    public void FAQTEcall()
    {
        StartCoroutine(qtehandler.WaitForStart());
    }

    public void HitImpact()
    {
        print("impact, calling animation");
        animationhandler.HurtCheck();
    }

    public void AttackAnimEnd()
    {
        print("animation end");
        attackroundhandler.NextAttack();
        animationhandler.ResetToIdle();
    }
}
