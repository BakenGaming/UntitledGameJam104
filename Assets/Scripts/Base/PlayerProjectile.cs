using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class PlayerProjectile : MonoBehaviour
{
    private ProjectileSO projectileSO;
    private Rigidbody2D projectileRB;
    private SpriteRenderer projectileSR;
    private GameObject target;
    private IHandler handler;
    private bool isDecaying;
    private float decayTimer=1f;
    private Vector3 moveDir;
    public void InitializeProjectile(GameObject _target, IHandler _handler)
    {
        target = _target;
        handler = _handler;
        moveDir = (target.transform.position - transform.position).normalized;
        projectileSO = _handler.GetStats().GetProjectileSO();
        projectileSR = GetComponent<SpriteRenderer>();
        projectileSR.sprite = projectileSO.projectileSprite;
        projectileSR.material.SetColor("_BaseColor",  projectileSO.projectileColor);
        float adjustedIntensity = 3f - 0.4169F;
        Color color = Mathf.Pow(2.0F, adjustedIntensity) * projectileSO.projectileColor;
        projectileSR.material.SetColor("_EmissionColor",  color);
        
        projectileRB = GetComponent<Rigidbody2D>();
    }

    private void Update() 
    {
        if(target == null || target.gameObject.activeInHierarchy == false) { isDecaying = true;}


        float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;

        transform.eulerAngles = new Vector3(0, 0, angle - 90f);
        
        transform.position += moveDir * handler.GetStats().GetProjectileSpeed() * Time.deltaTime;
        if(isDecaying) decayTimer -= Time.deltaTime;

        if(decayTimer <= 0) {ObjectPooler.EnqueueObject(this, "Player Projectile"); return;}
    }

    private void OnTriggerEnter2D(Collider2D trigger) 
    {
        IDamageable damageable = trigger.gameObject.GetComponent<IDamageable>();

        bool isCritical = Random.Range(0f,100f) < 20;

        if(damageable != null) 
            damageable.TakeDamage(handler.GetStats().GetAttackDamage(), isCritical);

        ObjectPooler.EnqueueObject(this, "Player Projectile");
    }
}
