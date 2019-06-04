using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Singleton Instance that handles all arena specific vars and the fight-state
/// </summary>
public class ArenaManager : MonoBehaviour
{
    public static ArenaManager Instance;

    [Header("UI elements for drag n drop")]
    public GameObject StatusUI;
    public GameObject ModeratorUI;
    public GameObject ComboInputUI;
    public GameObject PlayerOutputUI;
    public GameObject EnemyOutputUI;
    public GameObject AdvantageCheckUI;
    public GameObject QTEUI;
    public GameObject DamageDealUI;
    public GameObject ResultUI;
    public GameObject StartFightUI;

    public GameObject AttackConfirmButton;
    //public StatusUpdates statusUpdates;
    //public List<GameObject> activeMenus;

    public AttackIcon Mid;

    public Image PlayerHealthBarTick;
    public Image PlayerHealthBarFinal;
    public Image PlayerRageMeterTick;
    public Image PlayerRageMeterFinal;
    public Image EnemyHealthBarTick;
    public Image EnemyHealthBarFinal;
    public Image EnemyRageMeterTick;
    public Image EnemyRageMeterFinal;

    public Text RoundCounter;
    public Text PlayerLifeCounter;
    public Text PlayerRageCounter;
    public Text EnemyLifeCounter;
    public Text EnemyRageCounter;
    public Text PlayerName;
    public Text EnemyName;

    public Text WinOrLossText;
    public Text WinOrLossText2;
    public Text StyleRankText;
    public Text StyleRankText2;
    public Text FameWonText;
    public Text CreditsWonText;

    [Header("Colors")]
    public Color white;
    public Color grey;
    public Color black;
    public Color red;
    public Color green;
    public Color yellow;
    public Color pink;
    public Color turquoise;
    public Color violet;

    [Header("Enums")]
    public eAdvantage AdvantageCheck = eAdvantage.None;
    public eQTEResult Result = eQTEResult.Null;
    public eQTEType QTEType = eQTEType.None;
    public eTurn Turn = eTurn.Nobody;
    public eArena ArenaState;

    [Header("Integers")]
    public int MaxPlayerSlots;
    public int MaxEnemySlots;

    public int maxAttackRounds;
    public int curAttackRound;
    public int maxRounds;
    public int curRound;

    public int BaseDmg;
    public int EndDmg;
    public int PlayerMaxHealth;
    public int PlayerMaxRagePoints;
    public int PlayerHealth;
    public int PlayerRagePoints;
    public int EnemyMaxHealth;
    public int EnemyMaxRagePoints;
    public int EnemyHealth;
    public int EnemyRagePoints;


    public float timeStartedLerping;
    public float pastPlayerHealth;
    public float curPlayerHealth;
    public float pastEnemyHealth;
    public float curEnemyHealth;


    [Header("Bools")]
    public bool disableAttackInput;

    public bool playerHasAttacked;
    public bool enemyHasAttacked;
    public bool HPlerping = true;
    public bool RPlerping = true;
    public bool resultShowing = false;

    bool introrunning = false;
    public bool hasWon;
    [SerializeField] bool fadingIn;

    [Header("Other")]
    public CamShake camShake;
    public AttackIcon curAttack;

    void Awake()
    {
        #region Singleton
        if (Instance == null)
        {
            //DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        #endregion

        print("waking up");
        camShake = FindObjectOfType<CamShake>();

        DisableAllMenus();
        resultShowing = false;
        ArenaState = eArena.StartFight;
        curRound = 0;
        GetStartValues();            
    }

    void GetStartValues()
    {
        //TODO use ToUpper on all strings in globalmanager?
        PlayerName.text = GlobalManager.Instance.Monstername.ToUpper();
        EnemyName.text = GlobalManager.Instance.curEnemyName.ToUpper();
        PlayerMaxHealth = GlobalManager.Instance.PlayerMaxHealth;
        PlayerMaxRagePoints = GlobalManager.Instance.PlayerMaxRagePoints;
        EnemyMaxHealth = GlobalManager.Instance.EnemyMaxHealth;
        EnemyMaxRagePoints = GlobalManager.Instance.EnemyMaxRagePoints;
        PlayerHealth = PlayerMaxHealth;
        EnemyHealth = EnemyMaxHealth;
        MaxPlayerSlots = GlobalManager.Instance.MaxPlayerSlots;
        MaxEnemySlots = GlobalManager.Instance.MaxEnemySlots;
    }

    void ResetAll()
    {
        print("resetting");
        DisableAllMenus();
        resultShowing = false;
        curRound = 0;
        GetStartValues();
    }

    //void GetNumbers()
    //{
    //    curAttackRound = 0;
    //    MaxPlayerSlots = 3;
    //    MaxEnemySlots = MaxPlayerSlots;
    //    maxAttackRounds = Mathf.Max(MaxPlayerSlots, MaxEnemySlots);
    //}

    void Update()
    {
        //if (HPlerping)
        //{
        //    PlayerHealthBarTick.fillAmount = Mathf.Lerp(PlayerHealthBarTick.fillAmount, curPlayerHealth, Time.deltaTime * 3);
        //    EnemyHealthBarTick.fillAmount = Mathf.Lerp(EnemyHealthBarTick.fillAmount, curEnemyHealth, Time.deltaTime * 3);
        //}
        //if (RPlerping)
        //{
        //    PlayerRageMeterTick.fillAmount = Mathf.Lerp(PlayerRageMeterTick.fillAmount, curPlayerHealth, Time.deltaTime * 3);
        //    EnemyRageMeterTick.fillAmount = Mathf.Lerp(EnemyRageMeterTick.fillAmount, curEnemyHealth, Time.deltaTime * 3);
        //}

        UpdateHP();
        UpdateRP();

        //HACK to do a dirty jump to result
        void HardReset()
        {
            print("reset hack");
            GlobalManager.Instance.News.text = ("Hard reset");
            ComboInputUI.SetActive(true);
            ComboHandler.Add(Mid);
            ComboHandler.Add(Mid);
            ComboHandler.Add(Mid);
            ArenaManager.Instance.ArenaState = eArena.EnemyInput;
            EnemyHealth = 0;
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }

        switch (ArenaState)
        {
            case eArena.StartFight:

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    HardReset();
                }

                StartFightUI.SetActive(true);
                ResetAll();

                if (Input.GetButtonDown("A"))
                {
                    print("A button pressed");
                }

                break;
            case eArena.Intro:
                StartFightUI.SetActive(false);
                //UpdateHealth();
                //UpdateRage();
                StartCoroutine(Intro(2f));

                ModeratorUI.SetActive(true);
                //activeMenus.Add(ModeratorUI);
                //ActivateMenus(activeMenus);
                break;
            case eArena.PlayerInput:
                RoundCounter.text = ("ROUND " + (curRound + 1)+"");
                RoundCounter.color = turquoise;

                //AnimationControl.SetActive(false);
                StatusUI.SetActive(true);
                ComboInputUI.SetActive(true);
                PlayerOutputUI.SetActive(true);
                EnemyOutputUI.SetActive(false);
                //activeMenus.Clear();
                //activeMenus.Add(StatusUI);
                //activeMenus.Add(ComboInputUI);
                //activeMenus.Add(PlayerOutputUI);
                //ActivateMenus(activeMenus);
                
                if (ComboHandler.attacks.Count == MaxPlayerSlots)
                {
                    disableAttackInput = true;
                }

                break;
            case eArena.EnemyInput:
                PlayerOutputUI.SetActive(true);
                ComboInputUI.SetActive(false);
                EnemyOutputUI.SetActive(true);
                break;
            case eArena.AdvantageCheck:
                PlayerOutputUI.SetActive(true);
                EnemyOutputUI.SetActive(true);
                AdvantageCheckUI.SetActive(true);
                break;
            case eArena.QTEAttack:
                DamageDealUI.SetActive(false);
                AdvantageCheckUI.SetActive(false);
                //AnimationControl.SetActive(true);
                QTEUI.SetActive(true);
                break;
            case eArena.DealDamage:
                //LerpHP();
                //LerpRP();
                //statusUpdates.enabled = true;
                //AnimationControl.SetActive(true);
                QTEUI.SetActive(false);
                DamageDealUI.SetActive(true);

                //check if someone died 
                CheckForDeath();
                break;

            case eArena.NextAttack:
                               
                //check if all attacks are done
                if (curAttackRound < maxAttackRounds)
                {
                    //AnimationControl.SetActive(false);
                    ArenaState = eArena.AdvantageCheck;
                }
                else
                {
                    curRound += 1;
                    curAttackRound = 0;
                    RoundCounter.text = ("ROUND " + curRound + 1);

                    ArenaState = eArena.NextRound;

                    //HACK for gate 1: if there are rounds to be played
                    //if (curRound < maxRounds)
                    //{
                    //    GetNumbers();
                    //    ComboHandler.Reset();
                    //    ArenaState = eArena.PlayerInput;
                    //}
                    //else
                    //    ArenaState = eArena.Result;
                }
                break;
            case eArena.NextRound:
                //AnimationControl.SetActive(true);
                DisableAllMenus();
                ResetAllStates();

                ArenaState = eArena.PlayerInput;

                break;
            case eArena.Death:
                StartCoroutine(GetResult(hasWon));

                break;
            case eArena.Result:
                ResultUI.SetActive(true);
                if (Input.GetButtonDown("A"))
                {
                    print("A button pressed");
                }
                    break;

        }

        switch (Turn)
        {
            case eTurn.Nobody:
                playerHasAttacked = false;
                enemyHasAttacked = false;
                break;
            case eTurn.Player:
                playerHasAttacked = true;
                break;
            case eTurn.Enemy:
                enemyHasAttacked = true;
                break;
        }
    }

    public void CheckForDeath()
    {
        //check if someone died
        if (PlayerHealth <= 0)
        {
            hasWon = false;
            ArenaState = eArena.Death;
        }
        if (EnemyHealth <= 0)
        {
            hasWon = true;
            ArenaState = eArena.Death;
        }
    }

    void ActivateMenus(List<GameObject> menus)
    {
        foreach (GameObject menu in menus)
        {
            CanvasGroup menuCanvas = menu.GetComponent<CanvasGroup>();
            menu.SetActive(true);
            StartCoroutine(FadeInMenu(menuCanvas, 0.5f));
        }
    }

    IEnumerator FadeInMenu(CanvasGroup menuCanvas, float t)
    {
        menuCanvas.alpha = 0f;

        while (menuCanvas.alpha < 255f)
        {
            menuCanvas.alpha = menuCanvas.alpha + (Time.deltaTime / t);
            yield return null;
        }
    }

    void DisableAllMenus()
    {
        ComboInputUI.SetActive(false);
        AttackConfirmButton.SetActive(false);
        PlayerOutputUI.SetActive(false);
        EnemyOutputUI.SetActive(false);
        AdvantageCheckUI.SetActive(false);
        QTEUI.SetActive(false);
        DamageDealUI.SetActive(false);
        ResultUI.SetActive(false);
        //PlayerDmg.SetActive(false);
        //EnemyDmg.SetActive(false);
    }

    public void UpdateHP()
    {
        curPlayerHealth = (float)PlayerHealth / (float)PlayerMaxHealth;
        curEnemyHealth = (float)EnemyHealth / (float)EnemyMaxHealth;

        if (PlayerHealth < 0)
        {
            PlayerHealth = 0;
            curPlayerHealth = 0.0f;
        }

        if (EnemyHealth < 0)
        {
            EnemyHealth = 0;
            curEnemyHealth = 0.0f;
        }

        PlayerLifeCounter.text = "" + PlayerHealth;
        EnemyLifeCounter.text = "" + EnemyHealth;

        PlayerHealthBarTick.fillAmount = Mathf.Lerp(PlayerHealthBarTick.fillAmount, curPlayerHealth, Time.deltaTime * 3);
        EnemyHealthBarTick.fillAmount = Mathf.Lerp(EnemyHealthBarTick.fillAmount, curEnemyHealth, Time.deltaTime * 3);
    }

    public void UpdateRP()
    {
        //TODO RP cannot drop beneath 0
        //TODO Ferals cannot be used when RP is too low

        float curPlayerRP = (float)PlayerRagePoints / (float)PlayerMaxRagePoints;
        float curEnemyRP = (float)EnemyRagePoints / (float)EnemyMaxRagePoints;

        if (PlayerRagePoints < 0)
            curPlayerRP = 0.0f;

        if (EnemyRagePoints < 0)
            curEnemyRP = 0.0f;

        PlayerRageCounter.text = "" + PlayerRagePoints;
        EnemyRageCounter.text = "" + EnemyRagePoints;

        PlayerRageMeterTick.fillAmount = Mathf.Lerp(PlayerRageMeterTick.fillAmount, curPlayerRP, Time.deltaTime * 4);
        EnemyRageMeterTick.fillAmount = Mathf.Lerp(EnemyRageMeterTick.fillAmount, curEnemyRP, Time.deltaTime * 4);
    }

    //void StartLerping()
    //{
    //    timeStartedLerping = Time.deltaTime;
    //    HPlerping = true;
    //}

    //public void StartHPLerp()
    //{
    //    //timeStartedLerping = Time.deltaTime;
    //    HPlerping = true;
    //}

    //public float LerpHP(float start, float end, float lerpTime)
    //{
    //    //float timeSinceStarted = Time.deltaTime - timeStartedLerping;
    //    //float percentageComplete = timeSinceStarted / lerpTime;
    //    var result = Mathf.Lerp(start, end, Time.deltaTime*lerpTime);

    //    //float result = Mathf.Lerp(start, end, t);
    //    print("result :"+result);

    //    return result;
    //}

    //public IEnumerator LerpHP()
    //{
    //    yield return new WaitForSeconds(0.2f);
    //    HPlerping = true;
    //    //PlayerHealthBarFinal.fillAmount = Mathf.Lerp(pastPlayerHealth, curPlayerHealth, Time.deltaTime * 3);
    //    //EnemyHealthBarFinal.fillAmount = Mathf.Lerp(pastEnemyHealth, curEnemyHealth, Time.deltaTime * 3);
    //}

    //public void HPTick()
    //{
    //    pastPlayerHealth = PlayerHealthBarFinal.fillAmount;
    //    pastEnemyHealth = EnemyHealthBarFinal.fillAmount;

    //    curPlayerHealth = (float)PlayerHealth / (float)PlayerMaxHealth;
    //    curEnemyHealth = (float)EnemyHealth / (float)EnemyMaxHealth;

    //    //PlayerHealthBarTick.fillAmount = curPlayerHealth;
    //    //EnemyHealthBarTick.fillAmount = curEnemyHealth;

    //    //StartCoroutine(LerpHP());
    //}

    //public void RPTick()
    //{
    //    pastPlayerHealth = PlayerHealthBarFinal.fillAmount;
    //    pastEnemyHealth = EnemyHealthBarFinal.fillAmount;

    //    curPlayerHealth = (float)PlayerHealth / (float)PlayerMaxHealth;
    //    curEnemyHealth = (float)EnemyHealth / (float)EnemyMaxHealth;

    //    //HPlerping = true;

    //    PlayerHealthBarTick.fillAmount = curPlayerHealth;
    //    EnemyHealthBarTick.fillAmount = curEnemyHealth;

    //    //StartCoroutine(LerpHP());
    //}

    //public IEnumerator UpdateHealth()
    //{
    //    float curPlayerHealth = (float)PlayerHealth / (float)PlayerMaxHealth; 
    //    float curEnemyHealth = (float)EnemyHealth / (float)EnemyMaxHealth;

    //    PlayerHealthBarTick.fillAmount = curPlayerHealth;
    //    EnemyHealthBarTick.fillAmount = curPlayerHealth;

    //    yield return new WaitForSeconds(0.2f);

    //    HPlerping = true;

    //    PlayerLifeCounter.text = "" + PlayerHealth;
    //    EnemyLifeCounter.text = "" + EnemyHealth;        
    //}

    //public void LerpHP()
    //{
    //    float curPlayerHealth = (float)PlayerHealth / (float)PlayerMaxHealth; ;
    //    float curEnemyHealth = (float)EnemyHealth / (float)EnemyMaxHealth;

    //    if (HPlerping)
    //    {
    //        PlayerHealthBarFinal.fillAmount = Mathf.Lerp(PlayerHealthBarFinal.fillAmount, curPlayerHealth, Time.deltaTime * 3);
    //        EnemyHealthBarFinal.fillAmount = Mathf.Lerp(EnemyHealthBarFinal.fillAmount, curEnemyHealth, Time.deltaTime * 3);
    //    }
    //}

    //public IEnumerator UpdateRage()
    //{
    //    float curPlayerRage = (float)PlayerRagePoints / (float)PlayerMaxRagePoints;
    //    float curEnemyRage = (float)EnemyRagePoints / (float)EnemyMaxRagePoints;

    //    PlayerRageMeterTick.fillAmount = curPlayerRage;
    //    EnemyRageMeterTick.fillAmount = curEnemyRage;

    //    yield return new WaitForSeconds(0.2f);

    //    RPlerping = true;

    //    PlayerRageCounter.text = PlayerRagePoints + " %";
    //    EnemyRageCounter.text = EnemyRagePoints + " %";
    //}

    //public void LerpRP()
    //{
    //    float curPlayerHealth = (float)PlayerHealth / (float)PlayerMaxHealth; ;
    //    float curEnemyHealth = (float)EnemyHealth / (float)EnemyMaxHealth;

    //    if (HPlerping)
    //    {
    //        PlayerHealthBarFinal.fillAmount = Mathf.Lerp(PlayerHealthBarFinal.fillAmount, curPlayerHealth, Time.deltaTime * 3);
    //        EnemyHealthBarFinal.fillAmount = Mathf.Lerp(EnemyHealthBarFinal.fillAmount, curEnemyHealth, Time.deltaTime * 3);
    //    }
    //}

    void ResetAllStates()
    {
        Turn = eTurn.Nobody;
        AdvantageCheck = eAdvantage.None;
        Result = eQTEResult.Null;
    }

    IEnumerator Intro(float IntroTime)
    {
        if (introrunning)
            yield break;

        introrunning = true;
        GlobalManager.Instance.News.text = ("waiting for " + IntroTime + " seconds\n");
        yield return new WaitForSeconds(IntroTime);
        GlobalManager.Instance.News.text += ("intro done\n");
        ArenaState = eArena.PlayerInput;
    }

    IEnumerator GetResult(bool playerWin)
    {
        if (resultShowing)
            yield break;

        resultShowing = true;
        //ModeratorDialogue.resultHack = true;

        DisableAllMenus();
        yield return new WaitForSeconds(1f);
        StatusUI.SetActive(false);

        yield return new WaitForSeconds(2.5f);

        ModeratorUI.SetActive(false);

        if (hasWon)
        {
            WinOrLossText.text = "VICTORY";
            StyleRankText.text = "S";
            FameWonText.text = "300  x  3  = 900";
            CreditsWonText.text = "150  x  3  =  450";
        }
        else
        {
            WinOrLossText.text = "DEFEAT";
            StyleRankText.text = "B";
            FameWonText.text = "300  x  1,5  = 450";
            CreditsWonText.text = "150  x  1,5  =  225";
        }
        WinOrLossText2.text = WinOrLossText.text;
        StyleRankText2.text = StyleRankText.text;

        ArenaState = eArena.Result;
    }
}
