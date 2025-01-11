using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class EnemyMovement : MonoBehaviour
{
    private IHandler handler;
    private Rigidbody2D thisRigidbody;
    private Vector3 aimDirection;
    private bool initialized = false;
    private bool canMove = true;

    public void InitializeMovement(IHandler _handler)
    {
        handler = _handler;
        thisRigidbody = GetComponent<Rigidbody2D>();
        initialized = true;
    }   
    private void LateUpdate()
    {
        if (!initialized) return;
        UpdateAimDirection(GameManager.i.GetBaseGO().transform.position);
        if (GameManager.i.GetIsPaused())
        { 
            thisRigidbody.velocity = Vector2.zero;
            return;
        }
    }
    private void UpdateAimDirection(Vector3 _target)
    {
        aimDirection = (_target - transform.position).normalized;
        //transform.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(aimDirection) - 90f);
    }
    public void ChaseMovement()
    {
        if(GameManager.i.GetIsPaused())
        {
            thisRigidbody.velocity = Vector2.zero;
            return;
        }
        if (Vector2.Distance(GameManager.i.GetBaseGO().transform.position, transform.position) >= 1f)
            thisRigidbody.velocity = aimDirection * handler.GetStats().GetMoveSpeed() * Time.deltaTime;
        else
            thisRigidbody.velocity = Vector2.zero;
    }

    public void ChaseShootMovement()
    {
        if(GameManager.i.GetIsPaused())
        {
            thisRigidbody.velocity = Vector2.zero;
            return;
        }
        if (Vector2.Distance(GameManager.i.GetBaseGO().transform.position, transform.position) >= 1f)
            thisRigidbody.velocity = aimDirection * handler.GetStats().GetMoveSpeed() * Time.deltaTime;
        else
            thisRigidbody.velocity = Vector2.zero;
    }

    public void RadialTurretMovement()
    {
        
        if(GameManager.i.GetIsPaused() || !canMove)
        {
            thisRigidbody.velocity = Vector2.zero;
            return;
        }
        if (Vector2.Distance(GameManager.i.GetBaseGO().transform.position, transform.position) >= 1f)
            thisRigidbody.velocity = aimDirection * handler.GetStats().GetMoveSpeed() * Time.deltaTime;
        else
            thisRigidbody.velocity = Vector2.zero;
    }

    public void SetCanMove(bool _canMove){canMove = _canMove;}
    public bool GetCanMove(){return canMove;}
}
