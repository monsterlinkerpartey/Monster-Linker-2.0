using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class Enemy : ScriptableObject
{
    public string EnemyName;
    public Sprite EnemyAvatar;
    public string EnemyDescription;
    public List<AttackIcon> attacks = new List<AttackIcon>();
    public int maxHealth;
    public int maxRage;
    public int baseDamage;    
}
