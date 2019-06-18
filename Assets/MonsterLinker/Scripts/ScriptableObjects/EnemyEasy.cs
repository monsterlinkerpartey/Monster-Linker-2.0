using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Easy Enemy", menuName = "Enemy/Easy Enemy")]
public class EnemyEasy : Enemy
{
    [Header("BA Inputs for Normal state, 5 Slots")]
    public List<BaseAttack> BA_normal1;
    public List<BaseAttack> BA_normal2;
    public List<BaseAttack> BA_normal3;

    [Header("BA Inputs for Low HP state, 5 Slots")]
    public List<BaseAttack> BA_lowHP1;
    public List<BaseAttack> BA_lowHP2;
    public List<BaseAttack> BA_lowHP3;
}
