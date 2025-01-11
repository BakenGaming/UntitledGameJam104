using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName ="ScriptableObjects/Enemy")]
public class EnemySO : ScriptableObject
{
    public string enemyName;
    public GameObject enemyPrefab;
    public Brain[] enemyBrains;
    public Color enemyColor;
    public int health;
    public int attackDamage;
    public float projectileSpeed;
    public float projectileLifetime;
    public float fireRate;
    public float attackRange;
    public float moveSpeed;
    public int numberOfBullets;
    public int runeDustValue;
    public ProjectileSO projectileSO;
}
