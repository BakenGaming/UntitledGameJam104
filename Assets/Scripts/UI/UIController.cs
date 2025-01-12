using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.Rendering;

public class UIController : MonoBehaviour
{
    public static event Action OnBonusHealthUpdated;
    private static UIController _i;
    public static UIController i { get { return _i; } }
    [Header("Menus")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject creditsScreen;
    [SerializeField] private GameObject empowerMenu;
    [Header("Text Fields")]
    [SerializeField] private TextMeshProUGUI curencyCounter;
    [SerializeField] private TextMeshProUGUI healthStat, damageStat, fireRateStat;
    [Header("Images")]
    [SerializeField] private Image dayNightCycleImage;
    [SerializeField] private Sprite daySprite, nightSprite;
    [SerializeField] private Image fillImage;
    [SerializeField] private bool isMainMenu;

    private void Awake() 
    {
        _i = this;    
    }
    private void OnEnable() 
    {
        if (isMainMenu) Initialize();
    }
    private void OnDisable() 
    {
        BaseHandler.OnCurrencyAmountChanged -= UpdateCurrency;
        DayNightSystem.OnDayNightIncremented -= UpdateDayNightCycle;
        DayNightSystem.OnCycleChange -= UpdateCycleImage;
        DayNightSystem.OnNightStarted -= CloseEmpowerMenu;
        DayNightSystem.OnDayStarted -= OpenEmpowerMenu;
    }
    public void Initialize()
    {
        //GetComponent<VolumeSettings>().Initialize();        
        if(!isMainMenu) 
        {
            pauseMenu.SetActive(false);
            UpdateCurrency();
            UpdateStats();
        }
        else creditsScreen.SetActive(false);
        
        settingsMenu.SetActive(false);
        BaseHandler.OnCurrencyAmountChanged += UpdateCurrency;
        DayNightSystem.OnDayNightIncremented += UpdateDayNightCycle;
        DayNightSystem.OnCycleChange += UpdateCycleImage;
        DayNightSystem.OnDayStarted += OpenEmpowerMenu;
        DayNightSystem.OnNightStarted += CloseEmpowerMenu;
    }
    #region Menus
    private void OpenPauseMenu()
    {
        pauseMenu.SetActive(true);
        GameManager.i.PauseGame();
    }

    public void ClosePauseMenu()
    {
        pauseMenu.SetActive(false);
        GameManager.i.UnPauseGame();
    }

    public void OpenSettingsMenu()
    {
        settingsMenu.SetActive(true);
        GetComponent<VolumeSettings>().SettingsMenuOpened();
    }

    public void CloseSettingsMenu()
    {
        settingsMenu.SetActive(false);
    }

    public void OpenCreditsScreen()
    {
        creditsScreen.SetActive(true);
    }

    public void CloseCreditsScreen()
    {
        creditsScreen.SetActive(false);
    }

    private void OpenEmpowerMenu()
    {
        empowerMenu.SetActive(true);
        UpdateStats();
    }

    public void CloseEmpowerMenu()
    {
        empowerMenu.SetActive(false);
        if(DayNightSystem.i.GetIsDayTime()) DayNightSystem.i.InitializeNight();
    }

    #endregion
    #region Scene Management
    public void StartGame()
    {
        SceneController.StartGame();
    }

    public void RestartGame()
    {
        SceneController.StartGame();
    }
    public void BackToMainMenu()
    {
        SceneController.LoadMainMenu();
    }

    public void ExitGame()
    {
        SceneController.ExitGame();
    }
    #endregion
    #region GameUI Functions
    private void UpdateCurrency()
    {
        curencyCounter.text = GameManager.i.GetBaseGO().GetComponent<BaseHandler>().GetCurrencySystem().GetCurrentCurrency().ToString();
    }

    private void UpdateStats()
    {
        healthStat.text = GameManager.i.GetBaseGO().GetComponent<BaseHandler>().GetStats().GetPlayerHealth().ToString();
        damageStat.text = GameManager.i.GetBaseGO().GetComponent<BaseHandler>().GetStats().GetAttackDamage().ToString();
        fireRateStat.text = GameManager.i.GetBaseGO().GetComponent<BaseHandler>().GetStats().GetFireRate().ToString();
    }

    private void UpdateDayNightCycle()
    {
        fillImage.fillAmount = DayNightSystem.i.GetCurrentFillAmount();
    }

    private void UpdateCycleImage()
    {
        if(DayNightSystem.i.GetIsDayTime()) 
        {
            fillImage.sprite = daySprite;
            dayNightCycleImage.sprite = daySprite;
        }
        else 
        {
            fillImage.sprite = nightSprite;
            dayNightCycleImage.sprite = nightSprite;
        }
    }

    public void UpdateHealth()
    {
        GameManager.i.GetBaseGO().GetComponent<BaseHandler>().GetStats().UpdateBonusHealth(10);
        OnBonusHealthUpdated?.Invoke();
        UpdateStats();
    }

    public void UpdateDamage()
    {
        GameManager.i.GetBaseGO().GetComponent<BaseHandler>().GetStats().UpdateBonusAttackDamage(10);
        UpdateStats();
    }

    public void UpdateAttackSpeed()
    {
        GameManager.i.GetBaseGO().GetComponent<BaseHandler>().GetStats().UpdateBonusFireRate(.05f);
        UpdateStats();
    }
    #endregion
}
