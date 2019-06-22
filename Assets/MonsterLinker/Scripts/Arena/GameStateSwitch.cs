using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateSwitch : MonoBehaviour
{
    public static GameStateSwitch Instance;

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
    public ReadQTETimes readqtetimes;

    public Save curProfile; //TODO save file iwo her kriegen
    public Enemy curEnemy;

    //HACK: for testing temporary feral art loadout
    public List<FeralArt> LoadedFeralArts = new List<FeralArt>();


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
        readqtetimes = GetComponentInChildren<ReadQTETimes>();
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
        qtehandler.readqtetimes = readqtetimes;
    }
    
    //will be called by other scripts, update the arenastate and then run functions from the scripts
    public void SwitchState(eGameState GameState)
    {
        switch (GameState)
        {
            ///Blacklist und FA Loadout für Spieler
            ///Enemy Values laden und Attack Slot Setup für Enemy und Spieler
            case eGameState.Loadout:
                arenaui.BlackListPanel.SetActive(true);
                enemystatemachine.GetEnemyValues();
                attackslotspawn.Setup(curProfile.maxBaseAttackInputSlots, enemystatemachine.maxInputSlots);
                attackslotspawn.SpawnPlayerSlots();
                attackslotspawn.SpawnEnemySlots();
                arenaui.GetAttackSlots();
                GlobalVars.QTEfailed = false;
                //enemystatemachine.SetEnemyType(curEnemy);
                break;
            ///Arena in cinematischer Cutscene vorstellen
            ///FA Loadout und alle scripts laden
            case eGameState.Intro:
                arenaui.BlackListPanel.SetActive(false);
                feralartcheck.FeralArtLoadout(curProfile.FALoadout);
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
                
                qtehandler.QTEStateSwitch(eQTEState.Waiting);
                qtehandler.SetType(eQTEType.Attack, attackslotspawn.NumberOfAttackSlotsPlayer);


                //Animation der Attacke des Spielers sowie Reaktion des Gegners triggern
                //QTE zu den Attacken
                //DMG bei Hit austeilen
                //HP Gain während der Attacke
                //=> Loop bis alle Attacken durch sind
                //Am Ende des Turns: RP Gain Summe, Total DMG Dealt Count -> Check for Death
                break;
            case eGameState.QTEBlock:
                arenaui.InitiativeCheck.SetActive(false);
                arenaui.InputPanel.SetActive(true);
                arenaui.EnemyInputBar.SetActive(true);

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

                ///falls spieler einen 6. input slot kriegt:
                //attackslotspawn.SpawnPlayerSlots();
                //arenaui.GetAttackSlots();

                //=> Check ob Temp. Extra BA Input Slot freigeschaltet wurde
                //=> Check ob Recovery FA freigeschaltet ist und noch nicht benutzt wurde in diesem Fight
                //Go to Player Input State
                break;
            case eGameState.Result:
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