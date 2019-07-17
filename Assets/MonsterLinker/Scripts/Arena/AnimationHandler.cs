using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    public Animator EnemyAnim;
    public Animator PlayerAnim;

    public void MoveToMiddle()
    {
        PlayerAnim.SetTrigger("walk");
        EnemyAnim.SetTrigger("walk");
    }

    public void JumpBack()
    {
        PlayerAnim.SetTrigger("jump");
        EnemyAnim.SetTrigger("jump");
    }

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

    public void HurtCheck()
    {
        switch(GameStateSwitch.Instance.GameState)
        {
            case eGameState.QTEAttack:
                print("enemy hurt");
                EnemyAnim.SetTrigger("hurt");
                break;
            case eGameState.QTEBlock:
                print("player hurt");
                PlayerAnim.SetTrigger("hurt");
                break;
        }
    }

    public void ResetToIdle()
    {
        EnemyAnim.SetBool("block", false);
        PlayerAnim.SetBool("block", false);
    }
}
