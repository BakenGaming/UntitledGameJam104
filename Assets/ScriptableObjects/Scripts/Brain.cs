using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Brain : ScriptableObject
{
    public abstract void InitializeAI(IHandler _handler);
    public abstract void Think(EnemyThinker _thinker);
}
