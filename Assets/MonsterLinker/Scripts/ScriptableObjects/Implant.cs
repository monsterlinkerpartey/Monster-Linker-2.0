using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Implant", menuName = "Other/Implant")]
public class Implant : ScriptableObject
{
    public string ImplantName;
    public eImplant ImplantType;
    public int RPCost;
    public List<BaseAttack> FAInput;
}
