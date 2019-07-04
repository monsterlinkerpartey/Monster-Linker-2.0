using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    public Animator EnemyAnim;
    public Animator PlayerAnim;

    public void PlayerAttack(string animString)
    {
        print("starting player attack: " + animString);
        PlayerAnim.SetTrigger(animString);
    }
    public void EnemyAttack(string animString)
    {
        print("starting enemy attack: " + animString);
        EnemyAnim.SetTrigger(animString);
    }



}
