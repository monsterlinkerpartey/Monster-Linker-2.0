using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// temporary anim event for qtes
/// </summary>
public class ReadQTETimes : MonoBehaviour
{
    //bool start = false;
    //float timer = 0f;
    //int round;

    public eQTEZone QTEZone;

    public void Update()
    {
        //if (start)
        //{
        //    timer += Time.deltaTime;
        //}
    }

    ///sets zone to early = fail
    public void ZChangeBool()
    {
        //start = !start;
        QTEZone = eQTEZone.Fail;
    }

    public void ZGoodTime()
    {
        //round += 1;
        //print("QTE no " + round);
        //print("Good time: " + timer);
        QTEZone = eQTEZone.Good;
    }

    public void ZPerfectTime()
    {
        //print("Perfect time: " + timer);
        QTEZone = eQTEZone.Perfect;
    }

    public void ZEndQTE()
    {
        //print("End time: " + timer);
        //timer = 0f;
        //ZChangeBool();
        QTEZone = eQTEZone.Fail;
    }
}
