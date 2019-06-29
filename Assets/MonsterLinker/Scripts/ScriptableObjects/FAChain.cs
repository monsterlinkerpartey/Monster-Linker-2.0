using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FA Chain", menuName = "Attack/FA Chain")]
public class FAChain : ScriptableObject
{
    public List<BaseAttack> ChainInputList;
    public List<Attack> NeededFeralArts;
}
