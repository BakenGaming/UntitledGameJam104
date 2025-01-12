using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatSystem
{
    private int health;
    private int attackDamage;
    private float projectileSpeed;
    private float projectileLifetime;
    private float fireRate;
    private float attackRange;
    private float moveSpeed;
    private int numberOfBullets;
    private int runeDustValue;
    private ProjectileSO projectileSO;
    private Brain[] brains;
    private int bonusHealth, bonusDamage;
    private float bonusAttackSpeed;

    public StatSystem(PlayerStatsSO _stats)
    {
        health = _stats.health;
        attackDamage = _stats.attackDamage;
        projectileSpeed = _stats.projectileSpeed;
        projectileLifetime = _stats.projectileLifetime;
        fireRate = _stats.fireRate;
        projectileSO = _stats.projectileSO;
    }

    public StatSystem (EnemySO _stats)
    {
        health = _stats.health;
        attackDamage = _stats.attackDamage;
        projectileSpeed = _stats.projectileSpeed;
        projectileLifetime = _stats.projectileLifetime;
        fireRate = _stats.fireRate;
        attackRange = _stats.attackRange;
        moveSpeed = _stats.moveSpeed;
        numberOfBullets = _stats.numberOfBullets;
        runeDustValue = _stats.runeDustValue;
        projectileSO = _stats.projectileSO;
        brains = _stats.enemyBrains;
    }

    public void UpdateBonusHealth(int _amount)
    {
        bonusHealth += _amount;
    }
    public void UpdateBonusAttackDamage(int _amount)
    {
        bonusDamage += _amount;
    }
    public void UpdateBonusFireRate(float _amount)
    {
        bonusAttackSpeed = Mathf.Clamp(bonusAttackSpeed - _amount, -.2f, 1);
    }

    public int GetPlayerHealth (){return health + bonusHealth;}
    public int GetAttackDamage(){return attackDamage + bonusDamage;}
    public float GetMoveSpeed(){return moveSpeed;}
    public int GetNumberOfBullets(){return numberOfBullets;}
    public float GetProjectileSpeed(){return projectileSpeed;}
    public float GetFireRate(){return fireRate + bonusAttackSpeed;}
    public float GetProjectileLifetime(){return projectileLifetime;}
    public ProjectileSO GetProjectileSO(){return projectileSO;}
    public int GetRuneDustValue(){return runeDustValue;}
    public Brain[] GetBrains(){return brains;}

}
