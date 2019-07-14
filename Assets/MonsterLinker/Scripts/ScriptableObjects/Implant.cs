using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Implant", menuName = "Other/Implant")]
public class Implant : ScriptableObject
{
    public string ImplantName;
    public eImplant implant;
    public int RPCost;
    public List<BaseAttack> FAInput;
}
