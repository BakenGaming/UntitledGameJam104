using UnityEngine;
using CodeMonkey.Utils;
using UnityEngine.UIElements;

public class PlayerAttack : MonoBehaviour
{
    private IHandler handler;
    private Transform firePoint;
    private GameObject target;
    private Vector3 aimDirection;
    private Vector2 smoothedRotation, smoothRotateVelocity;
    private bool isFiring;

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
        RotateCannon();
        PlayerProjectile newProjectile = ObjectPooler.DequeueObject<PlayerProjectile>("Player Projectile");
        newProjectile.transform.position = firePoint.position;
        newProjectile.transform.rotation = firePoint.rotation;
        newProjectile.gameObject.SetActive(true);
        newProjectile.InitializeProjectile(target, handler);
    }

    private void RotateCannon()
    {
        aimDirection = (target.transform.position - transform.position).normalized;
        transform.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(aimDirection) - 90f);
    }

    
}
