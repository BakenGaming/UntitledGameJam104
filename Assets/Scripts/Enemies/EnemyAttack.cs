using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class EnemyAttack : MonoBehaviour
{
    private const float radius = 1f;
    private Transform firePoint;
    private GameObject target;
    private Vector3 aimDirection;
    private IHandler handler;
    private float timeBetweenShots = 0, timeBetweenRadials = 0;
    private bool initialized=false, canAttack=true, isFiring=false;
    public void InitializeAttack(IHandler _handler)
    {
        handler = _handler;
        firePoint = transform.Find("FirePoint");
        initialized = true;
    }

    private void UpdateAimDirection(Vector3 _target)
    {
        aimDirection = (_target - transform.position).normalized;
        transform.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(aimDirection) - 90f);
    }
    public void Attack(Vector3 _playerLastPosition)
    {
        if(!initialized) return;
        
        if(timeBetweenShots <= 0)
        {
            target = GameManager.i.GetBaseGO();
            EnemyProjectile newProjectile = ObjectPooler.DequeueObject<EnemyProjectile>("Enemy Projectile");
            newProjectile.transform.position = firePoint.position;
            newProjectile.transform.rotation = firePoint.rotation;
            newProjectile.gameObject.SetActive(true);
            newProjectile.InitializeProjectile(target, handler);
            timeBetweenShots = handler.GetStats().GetFireRate();
        }
    }

    private void Update() 
    {
        if(!initialized) return;
        UpdateTimers();
        if(timeBetweenRadials <= 0) canAttack = true;    
    }

    private void UpdateTimers()
    {
        timeBetweenShots -= Time.deltaTime;
        if(!isFiring) timeBetweenRadials -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D _trigger) 
    {
        IDamageable damageable = _trigger.gameObject.GetComponent<IDamageable>();
        
        bool isCritical = Random.Range(0f,100f) < 20;

        if(damageable != null) 
        {
            damageable.TakeDamage(handler.GetStats().GetAttackDamage(), isCritical);
            handler.HandleDeath();
        }
    }

    public bool GetCanAttack(){return canAttack;}
    public bool GetIsFiring(){return isFiring;}
}
