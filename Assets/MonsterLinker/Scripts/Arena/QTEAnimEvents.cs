using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Animation events for the QTE Zones
/// Attached to the QTE Animator
/// </summary>
public class QTEAnimEvents : MonoBehaviour
{
    public static eQTEZone QTEZone;
    [Tooltip("Drag n drop")]
    public QTEHandler qtehandler;

    bool start = false;
    float timer;

    //HACK temporary timer to read out time zones
    public void Update()
    {
        if (start)
        {
            timer += Time.deltaTime;
        }
    }

    public void ZStartQTE()
    {
        start = true;

        QTEZone = eQTEZone.Fail;
        print("cur Zone: " + QTEZone);
    }

    public void ZGoodTime()
    {
        QTEZone = eQTEZone.Good;
        print("cur Zone: " + QTEZone);
    }

    public void ZPerfectTime()
    {
        QTEZone = eQTEZone.Perfect;
        print("cur Zone: " + QTEZone);
    }

    public void ZEndQTE()
    {
        QTEZone = eQTEZone.Fail;
        StartCoroutine(qtehandler.CheckQTEZone());
        print("cur Zone: " + QTEZone);

        print("Full length: "+timer);
        start = false;
        timer = 0f;
    }
}
