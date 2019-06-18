using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Very Easy Enemy", menuName = "Enemy/Very Easy Enemy")]
public class EnemyVeryEasy : Enemy
{
    [Header("BA Inputs for Normal state, 5 Slots")]
    public List<BaseAttack> BA_normal1;
    public List<BaseAttack> BA_normal2;
    public List<BaseAttack> BA_normal3;    
    
}
