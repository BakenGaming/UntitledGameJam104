using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHandler
{
    public void Initialize();
    public abstract HealthSystem GetHealthSystem();
    public abstract void UpdateHealth();
    public abstract void HandleDeath();
    public abstract StatSystem GetStatSystem();
}
