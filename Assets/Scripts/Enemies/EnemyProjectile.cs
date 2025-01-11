using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class EnemyProjectile : MonoBehaviour
{
    private ProjectileSO projectileSO;
    private IHandler handler;
    private Rigidbody2D projectileRB;
    private SpriteRenderer projectileSR;
    private GameObject target;
    private Vector3 moveDir;
    private bool isMissile;
    private float lifeTimer;
    
    public void InitializeProjectile(GameObject _target, IHandler _handler)
    {
        target = _target;
        moveDir = (target.transform.position - transform.position).normalized;
        projectileSO = _handler.GetStats().GetProjectileSO();
        handler = _handler;
        lifeTimer = handler.GetStats().GetProjectileLifetime();
        projectileSR = GetComponent<SpriteRenderer>();
        projectileSR.sprite = projectileSO.projectileSprite;
        projectileSR.material.SetColor("_BaseColor",  projectileSO.projectileColor);
        float adjustedIntensity = 20f - 0.4169F;
        Color color = Mathf.Pow(2.0F, adjustedIntensity) * projectileSO.projectileColor;
        projectileSR.material.SetColor("_EmissionColor",  color);
        projectileRB = GetComponent<Rigidbody2D>();
        
        float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angle - 90f);
    }

    public void InitializeProjectile(Vector3 _shootDir, bool _isMissile, IHandler _handler)
    {
        handler = _handler;
        isMissile = _isMissile;
        projectileSO = handler.GetStats().GetProjectileSO();
        lifeTimer = handler.GetStats().GetProjectileLifetime();

        moveDir = _shootDir;
        transform.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(moveDir) -90f);
        projectileSR = GetComponent<SpriteRenderer>();
        projectileSR.sprite = projectileSO.projectileSprite;
        projectileSR.material.SetColor("_BaseColor",  projectileSO.projectileColor);
        float adjustedIntensity = 20f - 0.4169F;
        Color color = Mathf.Pow(2.0F, adjustedIntensity) * projectileSO.projectileColor;
        projectileSR.material.SetColor("_EmissionColor",  color);
        projectileRB = GetComponent<Rigidbody2D>();
    }

    private void Update() 
    {
        if(lifeTimer <= 0) { ObjectPooler.EnqueueObject(this, "Enemy Projectile"); return;}
        
        transform.position += moveDir * handler.GetStats().GetProjectileSpeed() * Time.deltaTime;
        UpdateTimers();
    }

    private void UpdateTimers()
    {
        lifeTimer -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D trigger) 
    {
        IDamageable damageable = trigger.gameObject.GetComponent<IDamageable>();

        bool isCritical = Random.Range(0f,100f) < 20;

        if(damageable != null) 
            damageable.TakeDamage(handler.GetStats().GetAttackDamage(), isCritical);

        ObjectPooler.EnqueueObject(this, "Enemy Projectile");
    }
}
