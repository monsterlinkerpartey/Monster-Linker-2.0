using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New QTEInfo", menuName = "QTEInfo")]
public class QTEInfo : ScriptableObject
{
    [Tooltip("QTE Countdown")]
    public float CountdownTimer;
    [Tooltip("Good result time")]
    public float GoodTime;
    [Tooltip("Perfect result time")]
    public float PerfectTime;
    [Tooltip("Late result time")]
    public float LateTime;
    [Tooltip("Prefab of the QTE that will be instantiated")]
    public GameObject QTEPrefab;
}
