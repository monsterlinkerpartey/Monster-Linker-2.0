using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusBarHandler : MonoBehaviour
{
    public RectTransform P_HPnormalTransform;
    public RectTransform P_HPhurtTransform;

    //y Pos is the same for Player and Enemy
    float cachedY;
    //these values need to be *-1 for the Enemy
    public float minXValue;
    public float maxXValue;
    int curP_HP;

    private int CurP_HP
    {
        get { return Mathf.RoundToInt(GameStateSwitch.Instance.baeffectshandler.curPlayerHP); }
        set { curP_HP = value; }
    }

    //get max health, curhealth, rp from calling function ? -> Round to Int

    public void SetStatusBarPositions()
    {
        cachedY = P_HPnormalTransform.position.y;

    }

    //Calculates position of bar
    private float MapValues(float x, float inMin, float inMax, float outMin, float outMax)
    {
        return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }

    public void LerpPlayerHP()
    {
        float currentXValue = MapValues(curP_HP, 0, Mathf.RoundToInt(GameStateSwitch.Instance.baeffectshandler.maxPlayerHP), minXValue, maxXValue);

        P_HPnormalTransform.position = new Vector3(currentXValue, cachedY);

    }

    
}
