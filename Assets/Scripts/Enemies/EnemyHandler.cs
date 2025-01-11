using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using UnityEngine.UI;
using System;
using System.Runtime.InteropServices;

public class EnemyHandler : MonoBehaviour, IHandler
{
    public static event Action<EnemyHandler> OnEnemyDeath;
    public static event Action<int> OnGatherDust;
    [SerializeField] private bool isTesting;
    private Vector3 offset = new Vector3(0f,.75f,0f);
    private HealthSystem healthSystem;
    private StatSystem statSystem;
    private GameObject healthBarGraphic;
    [SerializeField] private EnemySO enemySO;

    public void Initialize()
    {
        healthSystem = new HealthSystem(enemySO.health);
        statSystem = new StatSystem(enemySO);
        healthBarGraphic = transform.Find("HealthBar").gameObject;
        transform.Find("Sprite").GetComponent<SpriteRenderer>().color = enemySO.enemyColor;


        UpdateHealth();
        GetComponent<EnemyThinker>().ActivateBrain(this);
        GetComponent<EnemyMovement>().InitializeMovement(this);
        GetComponentInChildren<DamageHandler>().InitializeDamage(true);
        GetComponentInChildren<EnemyAttack>().InitializeAttack(this);
    }

    public void UpdateHealth()
    {
        healthBarGraphic.transform.Find("Slider").GetComponent<Slider>().value = healthSystem.GetHealthPercentage();
    }

    public void HandleDeath()
    {
        //Instantiate(GameAssets.i.pfPlayerDeathParticles, transform.position, Quaternion.identity);
        OnGatherDust?.Invoke(statSystem.GetRuneDustValue()); 
        OnEnemyDeath?.Invoke(this);
        ObjectPooler.EnqueueObject(this, "Enemy");
    }
    public HealthSystem GetHealthSystem(){return healthSystem;}
    public StatSystem GetStats(){return statSystem;}
    public bool GetIsTesting(){return isTesting;}

}
