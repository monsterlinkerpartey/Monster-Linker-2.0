﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateSwitch : MonoBehaviour
{
    public static GameStateSwitch Instance;

    public eGameState GameState;
    public eFightResult FightResult;
    public eImplant Implant;

    public float IntroTime = 1f;
    //TODO: List of all scripts in the arena scene
    public FeralArtCheck feralartcheck;
    public ArenaPlayerInput playerinput;
    public InputBarHandler inputbarhandler;
    public ArenaUIHandler arenaui;
    public AttackSlotSpawn attackslotspawn;
    public InitiativeCheck initiativecheck;
    public EnemyStateMachine enemystatemachine;
    public TurnChanger turnchanger;
    public QTEHandler qtehandler;
    public BAEffectsHandler baeffectshandler;
    public AttackRoundHandler attackroundhandler;
    public AnimationHandler animationhandler;
    public QTEAnimEvents qteanimevents;
    public LoadoutButtons loadoutbuttons;
    public FAInfoWindow fainfowindow;
    public StatusBarHandler enemystatusbar;
    public StatusBarHandler playerstatusbar;
    public CreatureAnimEvents playerCreatureanimevents;
    public CreatureAnimEvents enemyCreatureanimevents;

    public Save curProfile; 
    public Enemy curEnemy;

    
    void Start()
    {
        #region Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        #endregion

        GetAllScripts();
        ConnectScripts();
        SwitchState(eGameState.Loadout);
    }

    void GetAllScripts()
    {
        //TODO getcomponentinchildren, get each script at the start of scene
        loadoutbuttons = GetComponentInChildren<LoadoutButtons>();
        feralartcheck = GetComponentInChildren<FeralArtCheck>();
        playerinput = GetComponentInChildren<ArenaPlayerInput>();
        inputbarhandler = GetComponentInChildren<InputBarHandler>();
        arenaui = GetComponentInChildren<ArenaUIHandler>();
        attackslotspawn = GetComponentInChildren<AttackSlotSpawn>();
        initiativecheck = GetComponentInChildren<InitiativeCheck>();
        enemystatemachine = GetComponentInChildren<EnemyStateMachine>();
        turnchanger = GetComponentInChildren<TurnChanger>();
        qtehandler = GetComponentInChildren<QTEHandler>();
        baeffectshandler = GetComponentInChildren<BAEffectsHandler>();
        attackroundhandler = GetComponentInChildren<AttackRoundHandler>();
        animationhandler = GetComponentInChildren<AnimationHandler>();    
        qteanimevents = FindObjectOfType<QTEAnimEvents>();        
        fainfowindow = FindObjectOfType<FAInfoWindow>();
        enemystatusbar = FindObjectOfType<EnemyStatusBar>();
        playerstatusbar = FindObjectOfType<PlayerStatusBar>();
    }

    void ConnectScripts()
    {
        //Connect scripts that need access to each other
        inputbarhandler.feralartcheck = feralartcheck;
        inputbarhandler.initiativecheck = initiativecheck;
        inputbarhandler.arenaui = arenaui;
        playerinput.inputbarhandler = inputbarhandler;
        playerinput.arenaui = arenaui;
        playerinput.feralartcheck = feralartcheck;
        enemystatemachine.arenaui = arenaui;
        enemystatemachine.initiativecheck = initiativecheck;
        initiativecheck.arenaui = arenaui;
        initiativecheck.turnchanger = turnchanger;
        qtehandler.baeffectshandler = baeffectshandler;
        qtehandler.turnchanger = turnchanger;
        qtehandler.attackroundhandler = attackroundhandler;
        arenaui.inputbarhandler = inputbarhandler;
        feralartcheck.inputbarhandler = inputbarhandler;
        feralartcheck.arenaui = arenaui;
        feralartcheck.baeffectshandler = baeffectshandler;
        attackroundhandler.baeffectshandler = baeffectshandler;
        attackroundhandler.qtehandler = qtehandler;
        attackroundhandler.animationhandler = animationhandler;
        attackroundhandler.turnchanger = turnchanger;
        enemystatemachine.baeffectshandler = baeffectshandler;
        playerCreatureanimevents.animationhandler = animationhandler;
        playerCreatureanimevents.attackroundhandler = attackroundhandler;
        playerCreatureanimevents.qtehandler = qtehandler;
        enemyCreatureanimevents.animationhandler = animationhandler;
        enemyCreatureanimevents.attackroundhandler = attackroundhandler;
        enemyCreatureanimevents.qtehandler = qtehandler;
        qteanimevents.qtehandler = qtehandler;
        baeffectshandler.arenaui = arenaui;
        baeffectshandler.enemystatusbar = enemystatusbar;
        baeffectshandler.playerstatusbar = playerstatusbar;
    }
    
    //will be called by other scripts, update the arenastate and then run functions from the scripts
    public void SwitchState(eGameState gamestate)
    {
        GameState = gamestate;
        print("arena state: " + GameState);

        switch (gamestate)
        {
            ///Blacklist und FA Loadout für Spieler
            ///Enemy Values laden und Attack Slot Setup für Enemy und Spieler
            case eGameState.Loadout:
                loadoutbuttons.WindowSwitch(eLoadout.LoadoutOnly);
                loadoutbuttons.SetInitialTexts();
                arenaui.StatusBars.SetActive(false);
                arenaui.ResultPanel.SetActive(false);
                arenaui.FALoadout.SetActive(true);
                arenaui.QTEPanel.SetActive(false);
                enemystatemachine.GetEnemyValues();
                baeffectshandler.StartHpandRPValues(curProfile.MaxHitPoints, 0, curEnemy.MaxHitPoints, 0); 
                inputbarhandler.maxBaseAttackInputSlots = curProfile.maxBaseAttackInputSlots;
                //enemystatemachine.SetEnemyType(curEnemy);
                break;
            ///Arena in cinematischer Cutscene vorstellen
            ///FA Loadout und alle scripts laden
            case eGameState.Intro:
                arenaui.GetAttackSlots();
                arenaui.StatusBars.SetActive(false);
                arenaui.FALoadout.SetActive(false);
                arenaui.ResultPanel.SetActive(false);
                arenaui.FALoadout.SetActive(false);
                arenaui.SetEnemyHPandRP(Mathf.RoundToInt(baeffectshandler.curEnemyHP), Mathf.RoundToInt(baeffectshandler.curEnemyRP));
                arenaui.SetPlayerHPandRP(Mathf.RoundToInt(baeffectshandler.curPlayerHP), Mathf.RoundToInt(baeffectshandler.curPlayerRP));
                curProfile.SetCheapestFAcost();
                feralartcheck.LoadedFeralArts = curProfile.FALoadout;
                fainfowindow.WriteFAData();
                fainfowindow.SetSI();
                //feralartcheck.FeralArtLoadout(curProfile.FALoadout);
                attackroundhandler.QTEfailed = false;
                StartCoroutine(WaitForIntro(IntroTime));
                break;
            ///Player Input enablen
            ///Enemy Input laden
            ///FA Check
            case eGameState.PlayerInput:
                //get player values for status bars
                playerstatusbar.GetValues(curProfile.MaxHitPoints, 100.0f, -685.0f, -290.0f, 0.0f, 0.0f);
                enemystatusbar.GetValues(curEnemy.MaxHitPoints, 100.0f, 685.0f, 290.0f, 0.0f, 0.0f);

                baeffectshandler.PlayerRPatAttackStart = baeffectshandler.curPlayerRP;
                arenaui.StatusBars.SetActive(true);
                attackroundhandler.QTEfailed = false;
                arenaui.ResultPanel.SetActive(false);
                arenaui.EnemyInputBar.SetActive(false);
                arenaui.InputPanel.SetActive(true);
                arenaui.SetConfirmButtonStatus(false);
                arenaui.SetInputButtonsStatus(true);
                arenaui.PlayerInputBar.SetActive(true);
                enemystatemachine.CheckEnemyState();
                enemystatemachine.SetInput();
                //-> Input Slot voll: nach FAs checken
                //-> Input Slot voll: Confirm enablen
                break;
            ///Speedwerte vergleichen um Ini festzulegen
            case eGameState.InitiativeCheck:

                //baeffectshandler.GetAttackLists(feralartcheck.AttackList, enemystatemachine.curAttackInput);
                //baeffectshandler.GetAttackLists(inputbarhandler.PlayerAttackInput, enemystatemachine.curAttackInput);
                arenaui.InputPanel.SetActive(false);
                arenaui.PlayerInputBar.SetActive(true);
                arenaui.EnemyInputBar.SetActive(true);
                arenaui.InitiativeCheck.SetActive(true);
                initiativecheck.GetSpeedValues();
                StartCoroutine(initiativecheck.CompareSpeed());
                arenaui.QTEPanel.SetActive(true);
                //Enemy Input einblenden
                //Vergleichen der Speedwerte, Turn anzeigen
                //Int Turn += 1; Bei Turn 2 zu NextRound wechseln
                break;
            case eGameState.QTEAttack:
                arenaui.InitiativeCheck.SetActive(false);
                arenaui.EnemyInputBar.SetActive(false);
                arenaui.PlayerInputBar.SetActive(true);
                                
                attackroundhandler.GetAttackList(feralartcheck.AttackList);


                //qtehandler.SetType(eQTEType.Attack, attackslotspawn.NumberOfAttackSlotsPlayer);
                //qtehandler.QTEStateSwitch(eQTEState.Waiting);

                //Animation der Attacke des Spielers sowie Reaktion des Gegners triggern
                //QTE zu den Attacken
                //DMG bei Hit austeilen
                //HP Gain während der Attacke
                //=> Loop bis alle Attacken durch sind
                //Am Ende des Turns: RP Gain Summe, Total DMG Dealt Count -> Check for Death
                break;
            case eGameState.QTEBlock:
                arenaui.InitiativeCheck.SetActive(false);
                arenaui.PlayerInputBar.SetActive(false);
                arenaui.EnemyInputBar.SetActive(true);

                baeffectshandler.SetMashValue(0);
                attackroundhandler.GetAttackList(enemystatemachine.curAttackInput);
                //attackroundhandler.SetEffectValues();
                //attackroundhandler.StartAttack();

                //Animation der Attacke des Gegners sowie Reaktion des Spielers triggern
                //QTE zum Blocken & für RP Gain
                //DMG bei Hit austeilen
                //=> Loop bis alle Attacken durch sind
                //Am Ende des Turns: RP Gain Summe, Total DMG Taken Count -> Check for Death
                break;
            case eGameState.NextRound:
                arenaui.QTEPanel.SetActive(false);
                //Disable both Initiative Arrows
                arenaui.StatusBars.SetActive(false);
                arenaui.PlayerInitiativeArrow.enabled = false;
                arenaui.EnemyInitiativeArrow.enabled = false;
                initiativecheck.ResetSpeedValues();
                //Reset Player Input und Enemy Input
                inputbarhandler.Reset();
                feralartcheck.ResetLists();
                enemystatemachine.ClearInput();
                enemystatemachine.curAttackInput.Clear();
                //Reset DMG counters for the end of each turn
                baeffectshandler.ResetDmgCount();

                //HACK: zum Test von Temp Input Slot
                //attackroundhandler.QTEfailed = false;

                //check which implant is active
                switch (Implant)
                {
                    case eImplant.UnleashedMode:
                        print("unleash mode");
                        break;
                    case eImplant.SuperFA:
                        print("super duper fa");
                        //=> Check ob Recovery FA freigeschaltet ist und noch nicht benutzt wurde in diesem Fight
                        break;
                    case eImplant.TempInputSlot:
                        print("temporary input slot");
                        //=> Check ob Temp. Extra BA Input Slot freigeschaltet wurde
                        if (!attackroundhandler.QTEfailed)
                        {
                            print("no qte fails, giving 6th slot");
                            inputbarhandler.maxBaseAttackInputSlots = 6;

                            if (arenaui.playerSlots.Count < 6)
                            {
                                attackslotspawn.SpawnTemporarySlot();
                                arenaui.GetAttackSlots();
                            }
                        }
                        else
                        {
                            print("you failed, no 6th slot");
                            inputbarhandler.maxBaseAttackInputSlots = 5;
                            if (arenaui.playerSlots.Count > 5)
                            {
                                print("deleting 6th slot");
                                attackslotspawn.DestroyTemporarySlot();
                                arenaui.GetAttackSlots();
                            }
                            attackroundhandler.QTEfailed = false;
                        }
                        break;
                    case eImplant.RisingRage:
                        break;
                    default:
                        Debug.LogError("implant not found");
                        break;
                }

                //Go to Player Input State
                SwitchState(eGameState.PlayerInput);
                break;
            case eGameState.Result:
                arenaui.StatusBars.SetActive(false);
                arenaui.EnemyInputBar.SetActive(false);
                arenaui.PlayerInputBar.SetActive(false);
                               
                switch (FightResult)
                {
                    case eFightResult.None:
                        Debug.LogError("No Result set!");
                        break;
                    case eFightResult.Victory:
                        arenaui.NextButton.SetActive(true);
                        arenaui.RetryButton.SetActive(false);
                        arenaui.LoadoutButton.SetActive(false);
                        arenaui.ResultText.text = "WINNER";

                        break;
                    case eFightResult.Defeat:
                        arenaui.NextButton.SetActive(false);
                        arenaui.RetryButton.SetActive(true);
                        arenaui.LoadoutButton.SetActive(true);

                        arenaui.ResultText.text = "LOSER";
                        break;
                }
                arenaui.ResultPanel.SetActive(true);
                break;
        }
    }

    public void ResetFight()
    {
        baeffectshandler.curEnemyHP = enemystatemachine.maxHitPoints;
        baeffectshandler.curPlayerHP = curProfile.MaxHitPoints;
        baeffectshandler.curEnemyRP = 0;
        baeffectshandler.curPlayerRP = 0;

        //reset player and enemy inputs
        inputbarhandler.Reset();
        enemystatemachine.ClearInput();

        //reset all special implants but keep choice
        //keep fa list choice
    }


    IEnumerator WaitForIntro(float waitingTime)
    {
        print("showing arena intro");
        yield return new WaitForSeconds(waitingTime);
        SwitchState(eGameState.PlayerInput);
    }
}