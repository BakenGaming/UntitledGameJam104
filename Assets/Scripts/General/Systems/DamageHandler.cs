using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHandler : MonoBehaviour, IDamageable
{
    private IHandler handler;
    private bool isEnemy;
    private Vector2 offset = new Vector2(0, 1f);
    
    public void InitializeDamage(bool _isEnemy)
    {
        isEnemy = _isEnemy;
        if (isEnemy) handler = GetComponent<EnemyHandler>(); 
        else handler = GetComponent<BaseHandler>();
    }
    public void TakeDamage(int _damage, bool _isCrit)
    {
        if(!isEnemy) Debug.Log($"Taking Damage {_damage}");
        if (_isCrit) 
        {
             _damage = (int)(_damage * 1.5f);
        }

        handler.GetHealthSystem().LoseHealth((int)_damage);
        DamagePopup.Create(new Vector2(transform.position.x, transform.position.y + offset.y), (int)_damage, _isCrit);
        
        if(handler.GetHealthSystem().GetCurrentHealth() <= 0) 
        {
            if(handler.GetIsTesting()) handler.GetHealthSystem().ResetHealth();
            else HandleDeath();
        }

        handler.UpdateHealth();
        //Instantiate(GameAssets.i.pfEnemyImpactParticles, transform.position, Quaternion.identity);
        
    }

    private void HandleDeath()
    {
        handler.HandleDeath();
    }
}
