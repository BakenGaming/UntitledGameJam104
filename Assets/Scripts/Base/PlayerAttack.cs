using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private IHandler handler;
    private Transform firePoint;
    private GameObject target;

    public void InitializeAttack(IHandler _handler) 
    {
        handler = _handler;
        firePoint = transform.Find("FirePoint");  
    }
    private void OnEnable() 
    {
        BaseHandler.OnFireProjectile += Fire;    
    }
    private void OnDisable() 
    {
        BaseHandler.OnFireProjectile -= Fire;    
    }

    private void Fire(GameObject _target)
    {
        target = _target;
        PlayerProjectile newProjectile = ObjectPooler.DequeueObject<PlayerProjectile>("Player Projectile");
        newProjectile.transform.position = firePoint.position;
        newProjectile.transform.rotation = firePoint.rotation;
        newProjectile.gameObject.SetActive(true);
        newProjectile.InitializeProjectile(target, handler);
    }

    
}
