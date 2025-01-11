using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencySystem
{
    public static event Action OnCurrencyGained;
    public static event Action OnCurrencyDecreased;
    public static event Action OnInsufficientCurrency;
    private int currentCurrency;
    private int maxCurrency = 9999;

    public CurrencySystem()
    {
        currentCurrency = 0;
    }

    public void IncreaseCurrency(int _amount)
    {
        currentCurrency += _amount;
        if(currentCurrency > maxCurrency) currentCurrency = maxCurrency;
        OnCurrencyGained?.Invoke();
    }
    public void DecreaseCurrency(int _amount)
    {
        int tempCurrency = currentCurrency;
        tempCurrency -= _amount;
        if(currentCurrency >= 0) 
        {
            currentCurrency -= _amount; 
            OnCurrencyDecreased?.Invoke();
        }
        else OnInsufficientCurrency?.Invoke();
    }

    public int GetCurrentCurrency(){return currentCurrency;}
}
