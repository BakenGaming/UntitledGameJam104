using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThinker : MonoBehaviour
{
    private Brain[] brain;

    public void ActivateBrain(IHandler _handler)
    {
        brain = _handler.GetStats().GetBrains();
        foreach (Brain _brain in brain)
            _brain.InitializeAI(GetComponent<EnemyHandler>());
    }
    private void LateUpdate() 
    {
        foreach (Brain _brain in brain)
            _brain.Think(this);    
    }
}
