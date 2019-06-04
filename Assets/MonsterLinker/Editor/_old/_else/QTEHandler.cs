using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QTEHandler : MonoBehaviour
{
    //[Tooltip("Object with a collider that moves with the circle towards the center")]
    //public GameObject CircleCol;
    [Tooltip("Circle + Zones Graphics")]
    public GameObject QTEGraphics;
    public Image CenterButton;
    public GameObject BlockImg;
    public GameObject AttackImg;
    public Animator ButtonAnim;
    public Animator CircleAnim;
    public Image Circle;
    public Text QTEResultText;
    //public Text ResultText;

    //[Tooltip("Start Position of the circle collider")]
    //public Vector2 StartPos;
    //[Tooltip("End Position of the circle collider")]
    //public Vector2 EndPos;

    public Color normal;
    //public Color circle;
    public Color failed;
    public Color good;
    public Color perfect;

    public eQTEState State = eQTEState.Ready;
    public eQTEZones Zone = eQTEZones.Early;

    public List <ButtonIcon> Buttons = new List<ButtonIcon>();

    [Tooltip("Delay before the QTE is started")]
    [SerializeField] float delay;

    [SerializeField] float BasicStartupTime;
    [Tooltip("Time of the circle animation")]
    public float QTETimer;
    [Tooltip("Time the result counts as good")]
    public float QTEGood;
    [Tooltip("Time the result counts as perfect")]
    public float QTEPerfect;
    [Tooltip("Time the result counts as late")]
    public float QTELate;

    [SerializeField] bool finishing;
    [SerializeField] bool started;

    [SerializeField] AttackIcon curAttack;

    [SerializeField] bool isRandom = true;
    private int ran = 0;
    
    private void OnEnable()
    {      
        normal = ArenaManager.Instance.white;
        //circle = ArenaManager.Instance.white;
        failed = ArenaManager.Instance.red;
        good = ArenaManager.Instance.yellow;
        perfect = ArenaManager.Instance.green;

        //ResultText.text = "";
        QTEGraphics.SetActive(false);
        ArenaManager.Instance.Result = eQTEResult.Null;
        State = eQTEState.Ready;

        //Circle.color = circle;
        GetTimes();
        SetType();
        SetButton();

        Zone = eQTEZones.Early;
        State = eQTEState.Delayed;         
    }

    //void GetQTEClipTime()
    //{
    //    AnimationClip[] clips = CircleAnim.runtimeAnimatorController.animationClips;
    //    foreach (AnimationClip clip in clips)
    //    {
    //        switch (clip.name)
    //        {
    //            case "qte":
    //                CircleAnimClipTime = clip.length;
    //                break;
    //        }
    //    }
    //}


    void GetTimes()
    {
        curAttack = ComboHandler.attacks[ArenaManager.Instance.curAttackRound];

        //TODO set attackcliptime from curattack
        BasicStartupTime = 60 / 60f;

        QTEGood = 30 / 60f;
        QTEPerfect = 9 / 60f;
        QTELate = 0.0f;
        QTETimer = 60 / 60f;              

        delay = BasicStartupTime - QTETimer;

        CircleAnim.speed = QTETimer;
    }

    void SetButton()
    {
        //check if button is chosen random from the list
        if (isRandom)
        {
            ran = Random.Range(0, Buttons.Count);
        }
        else
        {
            ran = 0;
        }
        //check if there are buttons in the list to choose from
        if (Buttons.Count <= 0)
        {
            GlobalManager.Instance.News.text += ("No buttons added\n");
            Debug.LogError("No buttons added");
        }
        else
        {
            CenterButton.sprite = Buttons[ran].buttonSprite;
        }
    }

    void SetType()
    {        
        if (ArenaManager.Instance.Turn == eTurn.Player)
            ArenaManager.Instance.QTEType = eQTEType.Attack;
        else if (ArenaManager.Instance.Turn == eTurn.Enemy)
            ArenaManager.Instance.QTEType = eQTEType.Block;
    }

    private void OnDisable()
    {
        QTEResultText.transform.localScale = new Vector3(0f, 0f, 0f);
        AttackImg.SetActive(false);
        BlockImg.SetActive(false);
        ChangeTypeImgColor(normal);
        delay = 0f;
        BasicStartupTime = 0f;
        QTEGood = 0f;
        QTEPerfect = 0f;
        QTELate = 0f;
        QTETimer = 0f;
        Circle.color = normal;
        CircleAnim.speed = 1;
        finishing = false;
        started = false;
    }

    void Update()
    {
        switch (State)
        {
            case eQTEState.Delayed:
                QTEGraphics.SetActive(false);

                if (ArenaManager.Instance.QTEType == eQTEType.Attack)
                    AttackImg.SetActive(true);
                if (ArenaManager.Instance.QTEType == eQTEType.Block)
                    BlockImg.SetActive(true);

                delay -= Time.deltaTime;
                if (delay <= 0f)
                    State = eQTEState.Running;
                break;
            case eQTEState.Running:
                QTEGraphics.SetActive(true);
                RunTimer();
                //StartQTEAnim();
                GetButtonInput();
                QTEResult(GetButtonInput());
                SetQTEState();

                break;
            case eQTEState.Done:

                //switch (ArenaManager.Instance.Result)
                //{
                //    case eQTEResult.Null:
                //        //Circle.color = normal;
                //        break;
                //    case eQTEResult.Fail:
                //        //ResultText.color = failed;
                //        //ResultText.text = "FAIL!";
                //        //Circle.color = failed;
                //        break;
                //    case eQTEResult.Good:
                //        //ResultText.color = good;
                //        //ResultText.text = "GOOD!";
                //        //Circle.color = good;
                //        break;
                //    case eQTEResult.Perfect:
                //        //ResultText.color = perfect;
                //        //ResultText.text = "PERFECT!";
                //        //Circle.color = perfect;
                //        break;
                //}
                ButtonAnim.Play("Normal");
                StartCoroutine(FinishQTE(0.5f));            
                break;                  
        }
    }    

    void RunTimer()
    {
        State = eQTEState.Running;
        QTETimer -= Time.deltaTime;
        //print("Timer\n" + System.Math.Round(QTETimer, 2));

        if (!started)
            StartQTEAnim();
    }

    void StartQTEAnim()
    {
        if (!CircleAnim.GetCurrentAnimatorStateInfo(0).IsName("qte"))
        {
            CircleAnim.Play("qte");
            //MoveCircleCol();
            started = true;
        }
    }

    //void MoveCircleCol()
    //{
    //    if (!started)
    //    {
    //        CircleCol.transform.position = Vector2.Lerp(StartPos, EndPos, CircleAnimClipTime);
    //    }
    //}

    bool GetButtonInput()
    {
        if (Input.GetButtonDown(Buttons[ran].inputString))
        {
            ButtonAnim.Play("Highlighted");
            return true;
        }
        else
            return false;
    }

    void SetQTEState()
    {
        if (QTETimer <= QTEGood && QTETimer >= QTEPerfect)
            Zone = eQTEZones.Good;
        if (QTETimer <= QTEPerfect && QTETimer >= QTELate)
            Zone = eQTEZones.Perfect;        
        if (QTETimer <= QTELate)
            Zone = eQTEZones.Late;
        if (QTETimer <= 0.0f)
        {
            ArenaManager.Instance.Result = eQTEResult.Fail;
            ChangeQTEResultText("FAIL", failed);
        }
        if (ArenaManager.Instance.Result != eQTEResult.Null)
        {
            State = eQTEState.Done;
        }
    }

    void QTEResult(bool pressed)
    {
        if (pressed)
        {
            switch (Zone)
            {
                case eQTEZones.Early:
                    ArenaManager.Instance.Result = eQTEResult.Fail;
                    //ChangeTypeImgColor(failed);
                    ChangeQTEResultText("FAIL", failed);
                    break;
                case eQTEZones.Good:
                    ArenaManager.Instance.Result = eQTEResult.Good;
                    //ChangeTypeImgColor(good);
                    ChangeQTEResultText("GOOD", good);
                    break;
                case eQTEZones.Perfect:
                    ArenaManager.Instance.Result = eQTEResult.Perfect;
                    //ChangeTypeImgColor(perfect);
                    ChangeQTEResultText("PERFECT", perfect);
                    break;
                case eQTEZones.Late:
                    ArenaManager.Instance.Result = eQTEResult.Fail;
                    //ChangeTypeImgColor(failed);
                    ChangeQTEResultText("FAIL", failed);
                    break;
            }

            //if (QTETimer > QTEGood)
            //    ArenaManager.Instance.Result = eQTEResult.Fail;
            //else if (QTETimer <= QTEGood && QTETimer > QTEPerfect)
            //    ArenaManager.Instance.Result = eQTEResult.Good;
            //else if (QTETimer <= QTEPerfect && QTETimer > 0f)
            //    ArenaManager.Instance.Result = eQTEResult.Perfect;

            CircleAnim.speed = 0;
        }
        //else if (QTETimer <= 0)
        //{
        //    ArenaManager.Instance.Result = eQTEResult.Fail;
        //}
    }

    void ChangeTypeImgColor(Color col)
    {
        //AttackImg.GetComponent<Image>().color = col;
        //BlockImg.GetComponent<Image>().color = col;
    }

    void ChangeQTEResultText(string result, Color thiscolor)
    {
        QTEResultText.text = result;
        QTEResultText.color = thiscolor;
    }

    IEnumerator FinishQTE(float t)
    {
        if (finishing)
            yield break;

        finishing = true;

        print("showing result text");

        int Endsize = 1;
        QTEResultText.transform.localScale = new Vector3(0f, 0f, 0f);

        while (QTEResultText.transform.localScale.x < Endsize)
        {
            QTEResultText.transform.localScale = new Vector3(
                 QTEResultText.transform.localScale.x + (Time.deltaTime / t),
                 QTEResultText.transform.localScale.y + (Time.deltaTime / t),
                 QTEResultText.transform.localScale.z + (Time.deltaTime / t));
            yield return null;
        }

        GlobalManager.Instance.News.text += ("QTE Result: "+ArenaManager.Instance.Result+"\n");
        yield return new WaitForSeconds(0.2f);
        GlobalManager.Instance.News.text += ("Damage Dealing Status triggered \n");
        ArenaManager.Instance.ArenaState = eArena.DealDamage;
    }

    //public static IEnumerator ZoomInImg(Image img, Vector3 newScale, float Endsize, float t)
    //{
    //    img.transform.localScale = newScale;

    //    while (img.transform.localScale.x < Endsize)
    //    {
    //        img.transform.localScale = new Vector3(
    //             img.transform.localScale.x + (Time.deltaTime / t),
    //             img.transform.localScale.y + (Time.deltaTime / t),
    //             img.transform.localScale.z + (Time.deltaTime / t));
    //        yield return null;
    //    }
    //}
}