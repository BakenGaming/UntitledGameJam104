using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DayNightSystem : MonoBehaviour
{
    public static event Action OnDayNightIncremented;
    public static event Action<bool> OnCycleComplete;
    public static event Action OnCycleChange;
    public static event Action OnNightStarted;
    private static DayNightSystem _i;
    public static DayNightSystem i { get { return _i; } }
    private float currentFillAmount;
    private float dayNightIncrement=.3f;
    private bool cycleActive = false, isDaytime = true;

    private void Awake() 
    {
        _i = this;    
    }

    public void Initialize()
    {
        currentFillAmount = 0;
        cycleActive = true;
    }

    public void InitializeDay()
    {
        currentFillAmount = 0;
        cycleActive = true;
        isDaytime = true;
        OnCycleChange?.Invoke();
    }

    public void InitializeNight()
    {
        currentFillAmount = 0;
        cycleActive = true;
        isDaytime = false;
        OnCycleChange?.Invoke();
        OnNightStarted?.Invoke();
    }

    private void LateUpdate() 
    {
        if(cycleActive) currentFillAmount += dayNightIncrement * Time.deltaTime;
        if(currentFillAmount >= 1) 
        {
            OnCycleComplete?.Invoke(isDaytime); 
            cycleActive = false;
            if(isDaytime) InitializeNight();
        }
        OnDayNightIncremented?.Invoke();
    }
    public float GetCurrentFillAmount(){return currentFillAmount;}
    public bool GetIsDayTime(){return isDaytime;}
}
