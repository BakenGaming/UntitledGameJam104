using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Unity.VisualScripting;

public class BaseHandler : MonoBehaviour, IHandler
{
    private static BaseHandler _i;
    public static BaseHandler i { get { return _i; } }

    //Events
    public static event Action<GameObject> OnFireProjectile;
    public static event Action OnCurrencyAmountChanged;
    public static event Action OnBaseDestruction;

    //Serialzeable variables
    [SerializeField] private bool isTesting;
    [SerializeField] private Collider2D playerCollider;
    [SerializeField] private SpriteRenderer playerSR;
    [SerializeField] private PlayerStatsSO _statsSO;

    //Private variables (Classes)
    private StatSystem statSystem;
    private HealthSystem healthSystem;
    private CurrencySystem currencySystem;

    //Private variables (Non-Class)
    private float attackCD;
    private GameObject healthBarGraphic;
    private Transform target;

    private void OnEnable() 
    {

    }
    private void OnDisable() 
    {
        EnemyHandler.OnGatherDust -= IncreaseCurrency;
        EnemyHandler.OnEnemyDeath -= ResetAttackCooldown;
    }
    private void Awake()
    {
        _i = this;

    }
    public void InitializePlayer()
    {
        statSystem = new StatSystem(_statsSO);
        currencySystem = new CurrencySystem();
        healthBarGraphic = transform.Find("HealthBar").gameObject;
        healthSystem = new HealthSystem(statSystem.GetPlayerHealth());
        target = transform;

        UpdateHealth();
        GetComponent<DamageHandler>().InitializeDamage(false);
        GetComponentInChildren<PlayerAttack>().InitializeAttack(this);
        EnemyHandler.OnGatherDust += IncreaseCurrency;
        EnemyHandler.OnEnemyDeath += ResetAttackCooldown;
        UIController.OnBonusHealthUpdated += IncreaseHealth;
    }

    #region Loop
    private void Update() 
    {
        if(DayNightSystem.i.GetIsDayTime()) return;

        UpdateTimers();
    }

    private void LateUpdate() 
    {
        if(DayNightSystem.i.GetIsDayTime()) return;
        FindTarget();

    }
    private void UpdateTimers()
    {
        attackCD -= Time.deltaTime;
    }
    #endregion
    #region Attack Related
    private void FindTarget()
    {
        float distancetoClosestEnemy = Mathf.Infinity;
        EnemyHandler closestEnemy = null;
        EnemyHandler[] allEnemies = GameObject.FindObjectsOfType<EnemyHandler>();
        
        foreach(EnemyHandler currentEnemy in allEnemies)
        {
            float distanceToEnemy = (currentEnemy.transform.position - this.transform.position).sqrMagnitude;
            if(distanceToEnemy < distancetoClosestEnemy)
            {
                distancetoClosestEnemy = distanceToEnemy;
                closestEnemy = currentEnemy;
            }
        }
        
        if(closestEnemy != null) FireProjectile(closestEnemy.gameObject);
    }

    private void FireProjectile(GameObject _target)
    {
        if(attackCD <= 0f && _target != null)
        {
            OnFireProjectile?.Invoke(_target);
            attackCD = statSystem.GetFireRate();
        }
        else return;
    }
    #endregion

    private void IncreaseCurrency(int _amount)
    {
        currencySystem.IncreaseCurrency(_amount);
        OnCurrencyAmountChanged?.Invoke();
    }

    private void DecreaseCurrency(int _amount)
    {
        currencySystem.DecreaseCurrency(_amount);
        OnCurrencyAmountChanged?.Invoke();
    }

    private void ResetAttackCooldown(EnemyHandler _notUsed){attackCD = 0;}

    #region Ihandler Functions
    public void HandleDeath()
    {
        throw new System.NotImplementedException();
    }

    private void IncreaseHealth()
    {
        healthSystem.IncreaseMaxHealth(statSystem.GetPlayerHealth());
        UpdateHealth();
    }

    public void UpdateHealth()
    {
        healthBarGraphic.transform.Find("Slider").GetComponent<Slider>().value = healthSystem.GetHealthPercentage();
    }
    public HealthSystem GetHealthSystem(){return healthSystem;} 
    public CurrencySystem GetCurrencySystem(){return currencySystem;}
    public StatSystem GetStats(){return statSystem;}
    public bool GetIsTesting(){return isTesting;}
    #endregion
}
