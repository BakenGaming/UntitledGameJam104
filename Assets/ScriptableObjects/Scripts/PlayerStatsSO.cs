using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Player Statistics")]
public class PlayerStatsSO : ScriptableObject
{
    public int health;
    public int attackDamage;
    public float projectileSpeed;
    public float projectileLifetime;
    public float fireRate;
    public ProjectileSO projectileSO;
}
