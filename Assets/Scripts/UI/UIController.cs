using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject creditsScreen;
    [SerializeField] private TextMeshProUGUI curencyCounter;
    [SerializeField] private Image dayNightCycleImage;
    [SerializeField] private Sprite daySprite, nightSprite;
    [SerializeField] private Image fillImage;
    [SerializeField] private bool isMainMenu;

    private void OnEnable() 
    {
        if (isMainMenu) Initialize();
        GameManager.OnBaseSpawned += Initialize;
    }
    private void OnDisable() 
    {
        BaseHandler.OnCurrencyAmountChanged -= UpdateCurrency;
        DayNightSystem.OnDayNightIncremented -= UpdateDayNightCycle;
        DayNightSystem.OnCycleChange -= UpdateCycleImage;
    }
    private void Initialize()
    {
        //GetComponent<VolumeSettings>().Initialize();        
        if(!isMainMenu) 
        {
            pauseMenu.SetActive(false);
            UpdateCurrency();
        }
        else creditsScreen.SetActive(false);
        
        settingsMenu.SetActive(false);
        BaseHandler.OnCurrencyAmountChanged += UpdateCurrency;
        DayNightSystem.OnDayNightIncremented += UpdateDayNightCycle;
        DayNightSystem.OnCycleChange += UpdateCycleImage;
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
    #endregion
}
