﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// QTE Type enum
/// different ui objects for the different types
/// different animations for the different lengths
/// 
public class QTEHandler : MonoBehaviour
{
    [Header("The different Types of QTE")]
    public Animator AttackQTEAnim;
    public Animator BlockQTEAnim;

    [Header("QTE Times for Attack")]
    public QTE Attack;

    [Header("QTE Times for Block")]
    public QTE Block;

    [Header("For the Button Input Randomizer")]
    public List<ButtonInput> Buttons = new List<ButtonInput>();
    public GameObject QTEButton;
    public Animator ButtonAnim;
    public Image ButtonImage;

    [Tooltip("Random generated no. for button input")]
    [SerializeField] int ran;

    [Header("Other stuff")]
    [Tooltip("Time til startup of the animation, default 2s")]
    public float AnimStartup = 2.0f;
    [Tooltip("Time til QTE starts")]
    [SerializeField] float WaitingTime;
    //[SerializeField] float QTETimer;
    //[SerializeField] float QTEGood;
    //[SerializeField] float QTEPerfect;

    [SerializeField] Animator curQTEAnim;
    [SerializeField] int curAttackSlotNo;
    [SerializeField] QTE curQTEType;
    [SerializeField] int MaxSlots;
    [Tooltip("Bool to start QTE Timer")]
    [SerializeField] bool running;
    [SerializeField] string AnimString;

    public ReadQTETimes readqtetimes;

    public void Update()
    {
        if (running)
        {
            //QTETimer -= Time.deltaTime;
            CheckForInput();
        }
    }

    public void CheckForInput()
    {
        if (Input.GetButtonDown(Buttons[ran].inputString))
        {
            print("qte button pressed");
            ButtonAnim.Play("Highlighted");
            curQTEAnim.speed = 0.0f;
            CheckQTEZone();
        }
    }

    void CheckQTEZone()
    {
        switch (readqtetimes.QTEZone)
        {
            case eQTEZone.None:
                print("ERROR: No QTE Zone set, check Animation Events");
                break;
            case eQTEZone.Fail:
                //trigger fail anim
                //do dmg stuff etc
                print("fail QTE result");
                GlobalVars.QTEfailed = true;
                break;
            case eQTEZone.Good:
                //trigger good anim
                //do dmg stuff etc
                print("good QTE result");
                break;
            case eQTEZone.Perfect:
                //trigger perfect anim
                //do dmg stuff etc
                print("perfect QTE result");
                break;
            default:
                print("ERROR: Could not find QTEZone, check QTEHandler");
                break;
        }
        SetQTEAnim(curQTEType.Type);
    }

    ///Called by GameStateSwitch for Attack/Block QTEs
    ///Sets QTE Animator, resets curAttackSlotNo to Zero, sets MaxSlotNo
    public void SetType(eQTEType QTEType, int maxSlots)
    {
        curAttackSlotNo = 1;
        MaxSlots = maxSlots;
        RandomButtonGenerator();

        switch (QTEType)
        {
            case eQTEType.Attack:
                curQTEAnim = AttackQTEAnim;
                curQTEType = Attack;
                //AttackQTE.SetActive(true);
                SetQTEAnim(curQTEType.Type);
                break;
            case eQTEType.Block:
                curQTEAnim = BlockQTEAnim;
                //BlockQTE.SetActive(true);
                SetQTEAnim(curQTEType.Type);
                break;
            //case eQTEType.FAEndurance:
            //    break;
            //case eQTEType.FA:
            //    break;
            default:
                print("ERROR: QTEType not found, check QTEHandler");
                break;
        }

        print("QTE Type: " + QTEType);
    }

    public void SetQTEAnim(string type)
    {
        if (curAttackSlotNo > MaxSlots)
        {
            QTEStateSwitch(eQTEState.Done);
            return;
        }

        AnimString = type + curAttackSlotNo;
        print("Choosing an attack QTE for slot " + curAttackSlotNo + "\n Animation: " + AnimString);

        switch (curAttackSlotNo)
        {
            case 1:
                WaitingTime = AnimStartup - curQTEType.QTEAnimationLength1;
                break;
            case 2:
                WaitingTime = AnimStartup - curQTEType.QTEAnimationLength2;
                break;
            case 3:
                WaitingTime = AnimStartup - curQTEType.QTEAnimationLength3;
                break;
            case 4:
                WaitingTime = AnimStartup - curQTEType.QTEAnimationLength4;
                break;
            case 5:
                WaitingTime = AnimStartup - curQTEType.QTEAnimationLength5;
                break;
            case 6:
                WaitingTime = AnimStartup - curQTEType.QTEAnimationLength6;
                break;
            default:
                WaitingTime = 0f;
                print("ERROR: Could not set Wait Time, check QTEHandler");
                break;
        }
        curAttackSlotNo += 1;
        StartCoroutine(WaitForStart());
    }

    public IEnumerator WaitForStart()
    {
        yield return new WaitForSeconds(WaitingTime);
        QTEStateSwitch(eQTEState.Running);
    }

    //public void GetTimes(QTE curSlot)
    //{
    //    //WaitingTime = AnimStartup - curSlot.QTEFullTime;
    //    //QTETimer = curSlot.QTEFullTime;
    //    //QTEGood = curSlot.QTEGood;
    //    //QTEPerfect = curSlot.QTEPerfect;
    //}

    ///Randomizes the QTE Button Image and Input
    public void RandomButtonGenerator()
    {
        ran = Random.Range(0, Buttons.Count - 1);
        print("Chosen Button: " + Buttons[ran].name);

        if (Buttons.Count <= 0)
        {
            Debug.LogError("ERROR: No buttons in list, check QTE Handler");
        }
        else
        {
            ButtonImage.sprite = Buttons[ran].buttonSprite;
        }
    }

    public void QTEStateSwitch(eQTEState QTEState)
    {
        switch (QTEState)
        {
            case eQTEState.Waiting:
                running = false;
                break;
            case eQTEState.Running:
                running = true;
                QTEButton.SetActive(true);
                curQTEAnim.Play(AnimString);                               
                break;
            case eQTEState.Done:
                print("QTEs done");
                //call turnchanger to see if theres another turn or nextround
                break;
            default:
                print("QTE state not found, check QTEHandler");
                break;
        }
    }


    //public eQTEState QTEState;

    //[Tooltip("Current attack for animation length")]
    //public Attack curAttack;
    //public float AnimSpeedModifier;
    //public Animator QTEAnim;

    //[Tooltip("Image of the centered Button")]
    //public Image QTEButtonSprite;
    //public Animator QTEButtonAnim;

    //[Header("Time til the QTE starts")]
    //public float WaitForQTEStart;
    //[Tooltip("The running timer for the QTE length, influenced by AnimationSpeedModifier")]
    //public float QTETimer;
    //[Header("QTE Result times")]
    //public float QTEFullTime = 120f;
    //public float QTEGoodTime = 60f;
    //public float QTEPerfectTime = 11f;

    //[Header("QTE Button List, drag n drop")]
    //[Tooltip("List of buttons to use randomly for this QTE")]
    //public List<ButtonInput> Buttons = new List<ButtonInput>();

    //[Tooltip("For the button randomizer")]
    //int ran;
    //public int AttackRound;
    //bool runTimer = false;

    /////QTETimer -= delta time
    /////if QTETimer > QTEFullTime-QTEGoodTime = early fail
    /////if QTETimer <= QTEFullTime-QTEGoodTime && > QTEPerfectTime = good result
    /////if QTETimer <=QTEPerfectTime && != 0 = perfect result

    //void Update()
    //{
    //    if (runTimer)
    //    {
    //        QTETimer -= Time.deltaTime;
    //        GetButtonInput();

    //        if (QTETimer <= 0.0f)
    //        {
    //            print("QTE late fail");
    //            QTEStateSwitch(eQTEState.Fail);
    //        }
    //    }
    //}

    //public void QTEStateSwitch(eQTEState QTEState)
    //{
    //    switch (QTEState)
    //    {
    //        case eQTEState.Waiting:
    //            SetButton();
    //            SetAnimationTimes();
    //            //run coroutine that waits for the specified amount of time
    //            StartCoroutine(Wait());        
    //            break;
    //        case eQTEState.Running:
    //            //start animation
    //            QTEAnim.Play("runQTE");
    //            runTimer = true;
    //            break;
    //        case eQTEState.Fail:
    //            QTEAnim.speed = 0;
    //            //do dmg stuff?
    //            QTEStateSwitch(eQTEState.Done);
    //            break;
    //        case eQTEState.Good:
    //            QTEAnim.speed = 0;
    //            //do dmg stuff?
    //            QTEStateSwitch(eQTEState.Done);
    //            break;
    //        case eQTEState.Perfect:
    //            QTEAnim.speed = 0;
    //            //do dmg stuff?
    //            QTEStateSwitch(eQTEState.Done);
    //            break;
    //        case eQTEState.Done:
    //            runTimer = false;
    //            //finishes qte, resets all values
    //            break;
    //        default:
    //            print("QTE state not found, check QTEHandler");
    //            break;           
    //    }
    //}        

    //public IEnumerator Wait()
    //{
    //    print("waiting for " + WaitForQTEStart+" s");
    //    yield return new WaitForSeconds(WaitForQTEStart);
    //    QTEStateSwitch(eQTEState.Running);
    //}

    //////called by gamestateswitch
    ////public void GetQTETimes(float fullLength, float goodTime, float PerfectTime)
    ////{
    ////    QTEFullTime = fullLength;
    ////    QTEGoodTime = goodTime;
    ////    QTEPerfectTime = PerfectTime;
    ////}

    //public void SetAnimationTimes()
    //{
    //    //read out which attackround -> switch
    //    switch(AttackRound)
    //    {
    //        case 1:
    //            AnimSpeedModifier = 1.1f;
    //            break;
    //        case 2:
    //            AnimSpeedModifier = 1.2f;
    //            break;
    //        case 3:
    //            AnimSpeedModifier = 1.3f;
    //            break;
    //        case 4:
    //            AnimSpeedModifier = 1.4f;
    //            break;
    //        case 5:
    //            AnimSpeedModifier = 1.5f;
    //            break;
    //        default:
    //            AnimSpeedModifier = 1.0f;
    //            print("AttackRound not correctly set, check QTE Handler");
    //            break;
    //    }

    //    print("AnimationSpeed = " + AnimSpeedModifier);
    //    //get time of full length and animation speed
    //    QTEAnim.SetFloat("animSpeed", AnimSpeedModifier);

    //    //get times of good and perfect result

    //    WaitForQTEStart = 2.5f - QTEFullTime;
    //    //get waiting time => curAttack.TimeTilStartup - QTEFullTime 
    //}

    //public void SetButton()
    //{
    //    ran = 0;

    //    if (Buttons.Count <= 0)
    //    {            
    //        Debug.LogError("ERROR: No buttons in list, check QTE Handler");
    //    }
    //    else
    //    {
    //        QTEButtonSprite.sprite = Buttons[ran].buttonSprite;
    //    }
    //}

    //bool GetButtonInput()
    //{
    //    if (Input.GetButtonDown(Buttons[ran].inputString))
    //    {
    //        print("qte button pressed");
    //        QTEButtonAnim.Play("Highlighted");
    //        QTEResult(QTETimer);
    //        return true;
    //    }
    //    else
    //        return false;
    //}

    //void QTEResult(float curTime)
    //{
    //    print("curTime :"+curTime);

    //    if (curTime > (QTEFullTime - QTEGoodTime))
    //    {
    //        print("QTE too early");
    //        QTEStateSwitch(eQTEState.Fail);
    //    }
    //    else if (curTime <= (QTETimer-QTEGoodTime) && curTime > (QTETimer - QTEPerfectTime))
    //    {
    //        print("QTE good");
    //        QTEStateSwitch(eQTEState.Good);
    //    }
    //    else if (curTime <= (QTETimer - QTEPerfectTime))
    //    {
    //        print("QTE perfect");
    //        QTEStateSwitch(eQTEState.Perfect);
    //    }
    //}
    //}
}