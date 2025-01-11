using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHandler
{
    public abstract HealthSystem GetHealthSystem();
    public abstract void UpdateHealth();
    public abstract void HandleDeath();
    public abstract StatSystem GetStats();
    public abstract bool GetIsTesting();
}
