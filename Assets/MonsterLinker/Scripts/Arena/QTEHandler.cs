using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QTEHandler : MonoBehaviour
{
    public eQTEState QTEState;

    [Tooltip("Current attack for animation length")]
    public Attack curAttack;
    public float AnimSpeedModifier;
    public Animator QTEAnim;

    [Tooltip("Image of the centered Button")]
    public Image QTEButtonSprite;
    public Animator QTEButtonAnim;
    
    [Header("Time til the QTE starts")]
    public float WaitForQTEStart;
    [Tooltip("The running timer for the QTE length, influenced by AnimationSpeedModifier")]
    public float QTETimer;
    [Header("QTE Result times")]
    public float QTEFullTime = 120f;
    public float QTEGoodTime = 60f;
    public float QTEPerfectTime = 11f;

    [Header("QTE Button List, drag n drop")]
    [Tooltip("List of buttons to use randomly for this QTE")]
    public List<ButtonInput> Buttons = new List<ButtonInput>();
    
    [Tooltip("For the button randomizer")]
    int ran;
    public int AttackRound;
    bool runTimer = false;

    ///QTETimer -= delta time
    ///if QTETimer > QTEFullTime-QTEGoodTime = early fail
    ///if QTETimer <= QTEFullTime-QTEGoodTime && > QTEPerfectTime = good result
    ///if QTETimer <=QTEPerfectTime && != 0 = perfect result

    void Update()
    {
        if (runTimer)
        {
            QTETimer -= Time.deltaTime;
            GetButtonInput();

            if (QTETimer <= 0.0f)
            {
                print("QTE late fail");
                QTEStateSwitch(eQTEState.Fail);
            }
        }
    }

    public void QTEStateSwitch(eQTEState QTEState)
    {
        switch (QTEState)
        {
            case eQTEState.Waiting:
                SetButton();
                SetAnimationTimes();
                //run coroutine that waits for the specified amount of time
                StartCoroutine(Wait());        
                break;
            case eQTEState.Running:
                //start animation
                QTEAnim.Play("runQTE");
                runTimer = true;
                break;
            case eQTEState.Fail:
                QTEAnim.speed = 0;
                //do dmg stuff?
                QTEStateSwitch(eQTEState.Done);
                break;
            case eQTEState.Good:
                QTEAnim.speed = 0;
                //do dmg stuff?
                QTEStateSwitch(eQTEState.Done);
                break;
            case eQTEState.Perfect:
                QTEAnim.speed = 0;
                //do dmg stuff?
                QTEStateSwitch(eQTEState.Done);
                break;
            case eQTEState.Done:
                runTimer = false;
                //finishes qte, resets all values
                break;
            default:
                print("QTE state not found, check QTEHandler");
                break;
           
        }
    }        

    public IEnumerator Wait()
    {
        print("waiting for " + WaitForQTEStart+" s");
        yield return new WaitForSeconds(WaitForQTEStart);
        QTEStateSwitch(eQTEState.Running);
    }

    ////called by gamestateswitch
    //public void GetQTETimes(float fullLength, float goodTime, float PerfectTime)
    //{
    //    QTEFullTime = fullLength;
    //    QTEGoodTime = goodTime;
    //    QTEPerfectTime = PerfectTime;
    //}

    public void SetAnimationTimes()
    {
        //read out which attackround -> switch
        switch(AttackRound)
        {
            case 1:
                AnimSpeedModifier = 1.1f;
                break;
            case 2:
                AnimSpeedModifier = 1.2f;
                break;
            case 3:
                AnimSpeedModifier = 1.3f;
                break;
            case 4:
                AnimSpeedModifier = 1.4f;
                break;
            case 5:
                AnimSpeedModifier = 1.5f;
                break;
            default:
                AnimSpeedModifier = 1.0f;
                print("AttackRound not correctly set, check QTE Handler");
                break;
        }

        print("AnimationSpeed = " + AnimSpeedModifier);
        //get time of full length and animation speed
        QTEAnim.SetFloat("animSpeed", AnimSpeedModifier);

        //get times of good and perfect result

        WaitForQTEStart = 2.5f - QTEFullTime;
        //get waiting time => curAttack.TimeTilStartup - QTEFullTime 
    }

    public void SetButton()
    {
        ran = 0;
        
        if (Buttons.Count <= 0)
        {            
            Debug.LogError("ERROR: No buttons in list, check QTE Handler");
        }
        else
        {
            QTEButtonSprite.sprite = Buttons[ran].buttonSprite;
        }
    }

    bool GetButtonInput()
    {
        if (Input.GetButtonDown(Buttons[ran].inputString))
        {
            print("qte button pressed");
            QTEButtonAnim.Play("Highlighted");
            QTEResult(QTETimer);
            return true;
        }
        else
            return false;
    }

    void QTEResult(float curTime)
    {
        print("curTime :"+curTime);

        if (curTime > (QTEFullTime - QTEGoodTime))
        {
            print("QTE too early");
            QTEStateSwitch(eQTEState.Fail);
        }
        else if (curTime <= (QTETimer-QTEGoodTime) && curTime > (QTETimer - QTEPerfectTime))
        {
            print("QTE good");
            QTEStateSwitch(eQTEState.Good);
        }
        else if (curTime <= (QTETimer - QTEPerfectTime))
        {
            print("QTE perfect");
            QTEStateSwitch(eQTEState.Perfect);
        }
    }
}
