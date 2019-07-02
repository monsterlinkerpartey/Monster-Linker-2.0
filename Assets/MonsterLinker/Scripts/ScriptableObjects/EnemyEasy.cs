using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Easy Enemy", menuName = "Enemy/Easy Enemy")]
public class EnemyEasy : Enemy
{
    [Header("BA Inputs for Normal + no RP state, 5 Slots")]
    public List<BaseAttack> BA_normal_noRP1;
    public List<BaseAttack> BA_normal_noRP2;

    [Header("BA Inputs for Normal state + FA use state, 5 Slots")]
    public List<BaseAttack> BA_normal_FA1;

    [Header("BA Inputs for Low HP + FA use state, 5 Slots")]
    public List<BaseAttack> BA_lowHP_FA2;

    [Header("BA Inputs for Low HP + no RP state, 5 Slots")]
    public List<BaseAttack> BA_lowHP_noFA;
}
