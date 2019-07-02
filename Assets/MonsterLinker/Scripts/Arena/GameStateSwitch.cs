using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateSwitch : MonoBehaviour
{
    public static GameStateSwitch Instance;

    public eGameState GameState;

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

    public Save curProfile; //TODO save file iwo her kriegen
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
        
    }

    void ConnectScripts()
    {
        //Connect scripts that need access to each other
        inputbarhandler.feralartcheck = feralartcheck;
        inputbarhandler.initiativecheck = initiativecheck;
        inputbarhandler.arenaui = arenaui;
        playerinput.inputbarhandler = inputbarhandler;
        playerinput.arenaui = arenaui;
        enemystatemachine.arenaui = arenaui;
        enemystatemachine.initiativecheck = initiativecheck;
        initiativecheck.arenaui = arenaui;
        initiativecheck.turnchanger = turnchanger;
        qtehandler.baeffectshandler = baeffectshandler;
        qtehandler.turnchanger = turnchanger;
        arenaui.inputbarhandler = inputbarhandler;
        feralartcheck.inputbarhandler = inputbarhandler;
        feralartcheck.arenaui = arenaui;
        attackroundhandler.baeffectshandler = baeffectshandler;
        attackroundhandler.qtehandler = qtehandler;
        enemystatemachine.baeffectshandler = baeffectshandler;
    }
    
    //will be called by other scripts, update the arenastate and then run functions from the scripts
    public void SwitchState(eGameState gamestate)
    {
        GameState = gamestate;

        switch (gamestate)
        {
            ///Blacklist und FA Loadout für Spieler
            ///Enemy Values laden und Attack Slot Setup für Enemy und Spieler
            case eGameState.Loadout:
                arenaui.FALoadout.SetActive(true);
                enemystatemachine.GetEnemyValues();
                attackslotspawn.Setup(curProfile.maxBaseAttackInputSlots, enemystatemachine.maxInputSlots);
                attackslotspawn.SpawnPlayerSlots();
                attackslotspawn.SpawnEnemySlots();
                arenaui.GetAttackSlots();
                GlobalVars.QTEfailed = false;

                baeffectshandler.curPlayerHP = GlobalVars.PlayerMaxHP;
                baeffectshandler.curEnemyHP = GlobalVars.EnemyMaxHP;
                inputbarhandler.maxBaseAttackInputSlots = curProfile.maxBaseAttackInputSlots;
                //enemystatemachine.SetEnemyType(curEnemy);
                break;
            ///Arena in cinematischer Cutscene vorstellen
            ///FA Loadout und alle scripts laden
            case eGameState.Intro:
                arenaui.FALoadout.SetActive(false);
                feralartcheck.LoadedFeralArts = curProfile.FALoadout;
                //feralartcheck.FeralArtLoadout(curProfile.FALoadout);
                StartCoroutine(WaitForIntro(IntroTime));
                break;
            ///Player Input enablen
            ///Enemy Input laden
            ///FA Check
            case eGameState.PlayerInput:
                arenaui.InputPanel.SetActive(true);
                arenaui.PlayerInputBar.SetActive(true);
                enemystatemachine.CheckEnemyState();
                enemystatemachine.SetInput();
                //-> Input Slot voll: nach FAs checken
                //-> Input Slot voll: Confirm enablen
                break;
            ///Speedwerte vergleichen um Ini festzulegen
            case eGameState.InitiativeCheck:

                baeffectshandler.GetAttackLists(feralartcheck.AttackList, enemystatemachine.curAttackInput);
                //baeffectshandler.GetAttackLists(inputbarhandler.PlayerAttackInput, enemystatemachine.curAttackInput);
                arenaui.InputPanel.SetActive(false);
                arenaui.PlayerInputBar.SetActive(true);
                arenaui.EnemyInputBar.SetActive(true);
                arenaui.InitiativeCheck.SetActive(true);
                initiativecheck.GetSpeedValues();
                StartCoroutine(initiativecheck.CompareSpeed());
                //Enemy Input einblenden
                //Vergleichen der Speedwerte, Turn anzeigen
                //Int Turn += 1; Bei Turn 2 zu NextRound wechseln
                break;
            case eGameState.QTEAttack:
                arenaui.InitiativeCheck.SetActive(false);
                arenaui.EnemyInputBar.SetActive(false);
                arenaui.PlayerInputBar.SetActive(true);
                                
                attackroundhandler.GetAttackList(feralartcheck.AttackList);

                qtehandler.SetType(eQTEType.Attack, attackslotspawn.NumberOfAttackSlotsPlayer);
                qtehandler.QTEStateSwitch(eQTEState.Waiting);


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
                //TODO: enemy attacks in Attack type umwandeln
                //attackroundhandler.GetAttackList(enemystatemachine.curAttackInput);
                qtehandler.SetType(eQTEType.Block, attackslotspawn.NumberOfAttackSlotsEnemy);
                qtehandler.QTEStateSwitch(eQTEState.Waiting);

                //Animation der Attacke des Gegners sowie Reaktion des Spielers triggern
                //QTE zum Blocken & für RP Gain
                //DMG bei Hit austeilen
                //=> Loop bis alle Attacken durch sind
                //Am Ende des Turns: RP Gain Summe, Total DMG Taken Count -> Check for Death
                break;
            case eGameState.NextRound:
                //Disable both Initiative Arrows
                arenaui.PlayerInitiativeArrow.enabled = false;
                arenaui.EnemyInitiativeArrow.enabled = false;
                //Reset Player Input und Enemy Input
                inputbarhandler.Reset();
                enemystatemachine.ClearInput();
                //Enemy Input Bar ausblenden
                arenaui.EnemyInputBar.SetActive(false);
                //Reset DMG counters for the end of each turn
                baeffectshandler.ResetDmgCount();

                //=> Check ob Temp. Extra BA Input Slot freigeschaltet wurde
                //=> Check ob Recovery FA freigeschaltet ist und noch nicht benutzt wurde in diesem Fight
                ///falls spieler einen 6. input slot kriegt:      
                //inputbarhandler.maxBaseAttackInputSlots += 1;
                //inputbarhandler.maxBaseAttackInputSlots = 5;
                //und updaten:
                //attackslotspawn.SpawnPlayerSlots();
                //arenaui.GetAttackSlots();

                //Go to Player Input State
                SwitchState(eGameState.PlayerInput);
                break;
            case eGameState.Result:
                //RESET EVERYTHING

                //Victory: Next Fight - Button
                //Defeat: Retry - Button
                //Immer: Back to Home - Button
                break;
        }
        print("arena state: " + GameState);
    }

    IEnumerator WaitForIntro(float waitingTime)
    {
        print("showing arena intro");
        yield return new WaitForSeconds(waitingTime);
        SwitchState(eGameState.PlayerInput);
    }    
}