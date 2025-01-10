using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHandler_Platformer : MonoBehaviour, IAttackHandler
{
    #region Variables

    #endregion

    #region Initialize
    public void Initialize()
    {
        PlayerInputController_Platformer.OnPlayerAttack += SetupAttack;
    }

    private void OnDisable() 
    {
        PlayerInputController_Platformer.OnPlayerAttack -= SetupAttack;    
    }

    private void SetupAttack()
    {
        
    }

    #endregion
}
