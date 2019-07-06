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
    public Animator FAQTEAnim;

    [Header("Length of the different attack kuh-teh-eh animations")]
    public float Attack1Length;
    public float Attack2Length;
    public float Attack3Length;
    public float Attack4Length;
    public float Attack5Length;
    public float Attack6Length;

    [Header("Length of the different block kuh-teh-eh animations")]
    public float Block1Length;
    public float Block2Length;
    public float Block3Length;
    public float Block4Length;
    public float Block5Length;


    [Header("For the Button Input Randomizer")]
    public List<ButtonInput> Buttons = new List<ButtonInput>();
    public GameObject QTEButton;
    public Animator ButtonAnim;
    public Image ButtonImage;

    [Tooltip("Random generated no. for button")]
    [SerializeField] int ran;
    [Tooltip("Random generated no. for animation")]
    [SerializeField] int ranAnim;

    public eQTEState QTEState;
    
    [Header("QTE Times for Attack")]
    public QTE Attack;
    [Header("QTE Times for Block")]
    public QTE Block;

    [Header("Other stuff")]
    [Tooltip("Time til impact/startup of Monster animation, default 1.5s")]
    public float AnimStartup = 1.5f;
    //TODO get animation events for monster animations to check when they end/start
    [Tooltip("Time til QTE starts")]
    [SerializeField] float WaitingTime;
    //[SerializeField] float QTETimer;
    //[SerializeField] float QTEGood;
    //[SerializeField] float QTEPerfect;

    [SerializeField] Animator curQTEAnim;
    [SerializeField] QTE curQTEType;

    [SerializeField] int MaxSlots;
    [Tooltip("Bool to start QTE Timer")]
    [SerializeField] bool running;
    [SerializeField] string AnimString;

    public BAEffectsHandler baeffectshandler;
    public TurnChanger turnchanger;
    
    public void GetAnimClipTimes()
    {
        AnimationClip[] attackClips = AttackQTEAnim.runtimeAnimatorController.animationClips;
        AnimationClip[] blockClips = BlockQTEAnim.runtimeAnimatorController.animationClips;

        foreach (AnimationClip clip in attackClips)
        {
            switch (clip.name)
            {
                case "Attack1":
                    Attack1Length = clip.length;
                    break;
                case "Attack2":
                    Attack2Length = clip.length;
                    break;
                case "Attack3":
                    Attack3Length = clip.length;
                    break;
                case "Attack4":
                    Attack4Length = clip.length;
                    break;
                case "Attack5":
                    Attack5Length = clip.length;
                    break;
                case "Attack6":
                    Attack6Length = clip.length;
                    break;
                default:
                    print("not saving time of " + clip.name);
                    break;
            }
        }

        foreach (AnimationClip clip in blockClips)
        {
            switch (clip.name)
            {
                case "Block1":
                    Block1Length = clip.length;
                    break;
                case "Block2":
                    Block2Length = clip.length;
                    break;
                case "Block3":
                    Block3Length = clip.length;
                    break;
                case "Block4":
                    Block4Length = clip.length;
                    break;
                case "Block5":
                    Block5Length = clip.length;
                    break;
                default:
                    print("not saving time of " + clip.name);
                    break;
            }
        }
    }
    
    void Update()
    {
        if (running)
        {
            CheckForInput();
        }
    }

    public void SetType(eQTEType QTEType)
    {
        //GlobalVars.AttackRound = 1;
        //MaxSlots = maxSlots;

        QTEStateSwitch(eQTEState.Waiting);

        switch (QTEType)
        {
            case eQTEType.Attack:
                curQTEAnim = AttackQTEAnim;
                curQTEType = Attack;
                SetQTEAnim(curQTEType.Type);
                //AttackQTE.SetActive(true);
                break;
            case eQTEType.Block:
                curQTEAnim = BlockQTEAnim;
                curQTEType = Block;
                SetQTEAnim(curQTEType.Type);
                //BlockQTE.SetActive(true);
                break;
            case eQTEType.FAEndurance:
                break;
            case eQTEType.FA:
                break;
            default:
                print("ERROR: QTEType not found, check QTEHandler");
                break;
        }
        print("QTE Type: " + QTEType);
        GetAnimClipTimes();
        RandomButtonGenerator();
        SetQTEAnim(curQTEType.Type);
    }


    ///Randomizes the QTE Button Image and Input
    public void RandomButtonGenerator()
    {
        ran = Random.Range(0, Buttons.Count);
        print("Chosen Button: " + Buttons[ran].name);

        if (Buttons.Count <= 0)
        {
            Debug.LogError("No buttons in list, check QTE Handler");
        }
        else
        {
            ButtonImage.sprite = Buttons[ran].buttonSprite;
        }
    }

    void CheckForInput()
    {
        if (Input.anyKey)
        {
            curQTEAnim.speed = 0.0f;

            if (Input.GetButtonDown(Buttons[ran].inputString))
            {
                print("kuhteheh button pressed");
                ButtonAnim.Play("Highlighted");
                StartCoroutine(CheckQTEZone());
            }
            else
            {
                QTEAnimEvents.QTEZone = eQTEZone.Fail;
                StartCoroutine(CheckQTEZone());
            }
        }
    }

    //Called if player input happened
    //Or by QTEZoneReader if QTE has gone through without input
    public IEnumerator CheckQTEZone()
    {
        running = false;

        switch (QTEAnimEvents.QTEZone)
        {
            case eQTEZone.None:
                print("ERROR: QTE Zone auf None, check Animation Events");
                break;
            case eQTEZone.Fail:
                //trigger fail anim
                //do dmg stuff etc
                curQTEAnim.speed = 1.0f;
                curQTEAnim.Play(curQTEType.name + "_Fail");
                GlobalVars.QTEfailed = true;
                baeffectshandler.DMGModification(curQTEType.ModifierFail);
                print("fail QTE result");
                break;
            case eQTEZone.Good:
                //trigger good anim
                //do dmg stuff etc
                curQTEAnim.speed = 1.0f;
                curQTEAnim.Play(curQTEType.name + "_Good");
                baeffectshandler.DMGModification(curQTEType.ModifierGood);
                print("good QTE result");
                break;
            case eQTEZone.Perfect:
                //trigger perfect anim
                //do dmg stuff etc
                curQTEAnim.speed = 1.0f;
                curQTEAnim.Play(curQTEType.name + "_Perfect");
                baeffectshandler.DMGModification(curQTEType.ModifierPerfect);
                print("perfect QTE result");
                break;
            default:
                print("ERROR: Could not find QTEZone, check QTEHandler");
                baeffectshandler.DMGModification(curQTEType.ModifierFail);
                break;
        }
        //wait for result animation to play
        yield return new WaitForSeconds(0.5f);
        //start new qte or set qte done
        QTEStateSwitch(eQTEState.Done);
        //GlobalVars.AttackRound += 1;
        //print("attackround: " + GlobalVars.AttackRound);
        //SetQTEAnim(curQTEType.Type);
    }

    public void SetQTEAnim(string type)
    {

        switch (GameStateSwitch.Instance.GameState)
        {
            case eGameState.QTEAttack:

            //TODO animations randomizen
            //ranAnim = Random.Range(1, 6);
            ranAnim = 1;

                switch (ranAnim)
                {
                    case 1:
                        WaitingTime = AnimStartup - Attack1Length;
                        break;
                    case 2:
                        WaitingTime = AnimStartup - Attack2Length;
                        break;
                    case 3:
                        WaitingTime = AnimStartup - Attack3Length;
                        break;
                    case 4:
                        WaitingTime = AnimStartup - Attack4Length;
                        break;
                    case 5:
                        WaitingTime = AnimStartup - Attack5Length;
                        break;
                    case 6:
                        WaitingTime = AnimStartup - Attack6Length;
                        break;
                    default:
                        WaitingTime = 0f;
                        Debug.LogError("Could not set Wait Time, check QTEHandler");
                        break;
                }
                break;
            case eGameState.QTEBlock:

                //TODO animations randomizen
                //ranAnim = Random.Range(1, 5);
                ranAnim = 1;

                switch (ranAnim)
                {
                    case 1:
                        WaitingTime = AnimStartup - Block1Length;
                        break;
                    case 2:
                        WaitingTime = AnimStartup - Block2Length;
                        break;
                    case 3:
                        WaitingTime = AnimStartup - Block3Length;
                        break;
                    case 4:
                        WaitingTime = AnimStartup - Block4Length;
                        break;
                    case 5:
                        WaitingTime = AnimStartup - Block5Length;
                        break;
                    default:
                        WaitingTime = 0f;
                        Debug.LogError("Could not set Wait Time, check QTEHandler");
                        break;
                }
                break;
        }
        AnimString = type + ranAnim;        
    }

    public IEnumerator WaitForStart()
    {
        print("kuhteheh waiting for " + WaitingTime+" s");
        yield return new WaitForSeconds(WaitingTime);
        QTEStateSwitch(eQTEState.Running);
    }

    public void QTEStateSwitch(eQTEState qteState)
    {
        QTEState = qteState;

        switch (qteState)
        {
            case eQTEState.Waiting:
                AttackQTEAnim.Play("Wait");
                BlockQTEAnim.Play("Wait");
                QTEButton.SetActive(false);
                running = false;
                break;
            case eQTEState.Running:
                print("kuhteheh running");
                running = true;
                QTEButton.SetActive(true);
                curQTEAnim.SetTrigger(AnimString);
                break;
            case eQTEState.Done:
                print("kuhteheh done");
                curQTEAnim.Play("Wait");
                QTEButton.SetActive(false);                

                ////TODO this shit goes somewhere else
                //switch (GameStateSwitch.Instance.GameState)
                //{
                //    case eGameState.QTEAttack:
                //        baeffectshandler.ShowTotalDmg(baeffectshandler.TotalDmgDealt);
                //        baeffectshandler.CheckForDeath(baeffectshandler.curEnemyHP, turnchanger.Turns);

                //        break;
                //    case eGameState.QTEBlock:
                //        baeffectshandler.ShowTotalDmg(baeffectshandler.TotalDmgTaken);
                //        baeffectshandler.CheckForDeath(baeffectshandler.curPlayerHP, turnchanger.Turns);

                //        break;
                //}

                //if (turnchanger.Turns == eTurn.EnemyFirst)
                //{
                //    turnchanger.SwitchTurn(eTurn.PlayerSecond);
                //}
                //else if (turnchanger.Turns == eTurn.PlayerFirst)
                //{
                //    turnchanger.SwitchTurn(eTurn.EnemySecond);
                //}
                //else
                //{
                //    turnchanger.SwitchTurn(eTurn.BothDone);
                //}

                break;
            default:
                Debug.LogError("QTE state not found, check QTEHandler");
                break;
        }
    }


    ///=============================================================

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