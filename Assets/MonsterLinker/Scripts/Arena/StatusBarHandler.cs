using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusBarHandler : MonoBehaviour
{
    [Header("Transform Objects of the bars")]
    public RectTransform RPnormalTransform;
    public RectTransform HPnormalTransform;
    public RectTransform HPhurtTransform;

    [Header("Values for xPos of the bar")]
    [Tooltip("X Position for HP = 100%")]
    public float minXValueHP;
    [Tooltip("X Position for HP = 0%")]
    public float maxXValueHP;
    [Tooltip("X Position for RP = 100%")]
    public float minXValueRP;
    [Tooltip("X Position for RP = 0%")]
    public float maxXValueRP;

    [Header("Positions for the change")]
    [Tooltip("Current Y Pos., grab and does not change")]
    public float cur_yPos;
    [Tooltip("Current X Pos., grab at each call")]
    public float cur_xPos;
    [Tooltip("Desired X Pos., where to move the bar to")]
    public float desired_xPos;

    [Tooltip("How quick the second healthbar 'lerps' down")] 
    public int LerpSpeed = 3;
    [Tooltip("How long the second healthbar 'lerps' down")] 
    public int LerpTime = 3;

    [Header("HP and RP values")]
    [Tooltip("Max HP in this fight")]
    public float maxHP;
    [Tooltip("Max RP in this fight, usually 100")]
    public float maxRP;
    [Tooltip("Current RP")]
    public float curRP;
    [Tooltip("Current HP")]
    public float curHP;
    [Tooltip("HP after reduction")]
    public float futureHP;
    [Tooltip("RP after reduction")]
    public float futureRP;

    [SerializeField] bool lerping;
    Vector2 curPos;
    Vector2 newPos;

    public void Update()
    {
        while (lerping)
            HPhurtTransform.anchoredPosition = Vector2.Lerp(curPos, newPos, Time.deltaTime * LerpSpeed);
    }

    public void GetValues(float maxHitPoints, float maxRagePoints,  float MaxXValueHP, float MaxXValueRP, float MinXValueRP, float MinXValueHP)
    {
        maxHP = Mathf.Round(maxHitPoints);
        maxRagePoints = Mathf.Round(100f);
        maxRP = 100.0f;

        minXValueHP = Mathf.Round(MinXValueHP);
        maxXValueHP = Mathf.Round(MaxXValueHP);

        minXValueRP = Mathf.Round(MinXValueRP);
        maxXValueRP = Mathf.Round(MaxXValueRP);
    }

    public void HPTick(float CurHP)
    {
        print("tick hp");
        futureHP = CurHP;

        //set current position of the bar
        cur_yPos = HPnormalTransform.anchoredPosition.y;
        cur_xPos = HPnormalTransform.anchoredPosition.x;

        //percentage values
        float curPercent = 100 - ((futureHP / maxHP) * 100);
        desired_xPos = maxXValueHP * (curPercent / 100);
                       
        HPnormalTransform.anchoredPosition = new Vector2(desired_xPos, cur_yPos);

        //TODO remove when lerp is working?
        curHP = futureHP;
    }

    public void RPTick(int CurRP)
    {
        print("tick rp");
        futureRP = (float)CurRP;

        //set current position of the bar
        cur_yPos = RPnormalTransform.anchoredPosition.y;
        cur_xPos = RPnormalTransform.anchoredPosition.x;

        //percentage values
        float curPercent = 100 - ((futureRP / maxRP) * 100);
        desired_xPos = maxXValueRP * (curPercent / 100);

        RPnormalTransform.anchoredPosition = new Vector2(desired_xPos, cur_yPos);

        curRP = futureRP;
    }

    //TODO: not working
    public void HPLerp()
    {
        print("lerp hp");
        //set current position of the bar
        cur_yPos = HPhurtTransform.anchoredPosition.y;
        cur_xPos = HPhurtTransform.anchoredPosition.x;

        //percentage values
        float curPercent = 100 - ((futureHP / maxHP) * 100);
        desired_xPos = maxXValueHP * (curPercent / 100);
        
        curPos = new Vector2(cur_xPos, cur_yPos);
        newPos = new Vector2(desired_xPos, cur_yPos);

        StartCoroutine(WaitForLerp());
        curHP = futureHP;
    }

    public IEnumerator WaitForLerp()
    {
        lerping = true;
        yield return new WaitForSeconds(LerpTime);
        lerping = false;
    }

    //private int CurP_HP
    //{
    //    get { return Mathf.RoundToInt(GameStateSwitch.Instance.baeffectshandler.curPlayerHP); }
    //    set { curP_HP = value; }
    //}

    ////get max health, curhealth, rp from calling function ? -> Round to Int

    //public void SetStatusBarPositions()
    //{
    //    cachedY = P_HPnormalTransform.position.y;

    //}

    ////Calculates position of bar
    //private float MapValues(float x, float inMin, float inMax, float outMin, float outMax)
    //{
    //    return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    //}

    //public void LerpPlayerHP()
    //{
    //    float currentXValue = MapValues(curP_HP, 0, Mathf.RoundToInt(GameStateSwitch.Instance.baeffectshandler.maxPlayerHP), minXValue, maxXValue);

    //    P_HPnormalTransform.position = new Vector3(currentXValue, cachedY);

    //}    
}
