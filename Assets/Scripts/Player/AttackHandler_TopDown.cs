using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHandler_TopDown : MonoBehaviour, IAttackHandler
{
    #region Variables

    #endregion

    #region Initialize
    public void Initialize()
    {
        PlayerInputController_TopDown.OnPlayerAttack += SetupAttack;
    }

    private void OnDisable() 
    {
        PlayerInputController_TopDown.OnPlayerAttack -= SetupAttack;    
    }

    private void SetupAttack(Vector3 mousePos)
    {
        
    }

    #endregion
}
