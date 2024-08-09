using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
/// <summary>
/// Date: 16-Jul-2024
/// Author: Nimroz Baloch
/// Purpose: This Script Controls All the UI Functionality.
/// </summary>
public class UiController : MonoBehaviour
{
    #region Variables
    public GameObject HomePanel;
    public GameObject LevelsPanel;
    public GameObject SettingsPanel;
    public GameObject ShopPanel;
    public GameObject GameOverPanel;
    public GameObject WinPanel;
    public GameObject LivesParent;
    public GameManager gameManagerRef;

    public Button PlayBtn;
    public Button SettingsBtn;
    public Button ShopBtn;
    public Button HomeBtn;
    public Button HomeBtnOnGO;
    public Button HomeBtnOnWin;
    public Button ReloadBtn;
    public Button NextLevelBtn;
    public Button CloseGame;

    public TMP_Text levelText;

    #endregion

    #region UnityMethods
    private void Awake()
    {
      SceneManager.LoadSceneAsync("GamePlay", LoadSceneMode.Additive);
    }


    private void Start()
    {
        gameManagerRef = FindObjectOfType<GameManager>();
        // Find all buttons in the scene

        AddListenerToButtons();
    }
    #endregion

    #region CustomMethods
    /// <summary>
    /// This Method Adds Listener to All UI Buttons.
    /// </summary>
    private void AddListenerToButtons()
    {
        PlayBtn.onClick.AddListener(() => MainButtonsMethod(PlayBtn.gameObject));
        SettingsBtn.onClick.AddListener(() => MainButtonsMethod(SettingsBtn.gameObject));
        ShopBtn.onClick.AddListener(() => MainButtonsMethod(ShopBtn.gameObject));
        HomeBtnOnGO.onClick.AddListener(() => MainButtonsMethod(HomeBtnOnGO.gameObject));
        HomeBtnOnWin.onClick.AddListener(() => MainButtonsMethod(HomeBtnOnWin.gameObject));
        HomeBtn.onClick.AddListener(() => MainButtonsMethod(HomeBtn.gameObject));
        ReloadBtn.onClick.AddListener(() => gameManagerRef.OnLevelReload());
        NextLevelBtn.onClick.AddListener(() => gameManagerRef.NextLevelBtn());
        CloseGame.onClick.AddListener(() => gameManagerRef.CloseGame());
    }

    /// <summary>
    /// This Method Checks Which  Btn is Clicked and Adds Functionality to All Buttons.
    /// </summary>
    public void MainButtonsMethod(GameObject sender)
    {
        if (sender == PlayBtn.gameObject)
        {
            LevelsPanel.SetActive(true);
            HomePanel.SetActive(false);
            CloseGame.gameObject.SetActive(false);
        }
        if (sender == SettingsBtn.gameObject)
        {
            SettingsPanel.SetActive(true);
            HomePanel.SetActive(false);
            CloseGame.gameObject.SetActive(false);
        }
        if (sender == ShopBtn.gameObject)
        {
            ShopPanel.SetActive(true);
            HomePanel.SetActive(false);
            CloseGame.gameObject.SetActive(false);
        }
        if (sender == HomeBtn.gameObject)
        {
             gameManagerRef.DisableAllLevel();
            if (levelText.gameObject.activeSelf)
            {
                levelText.gameObject.SetActive(false);
            }
            CloseGame.gameObject.SetActive(true);
            if (ShopPanel.activeSelf == true)
            {
                ShopPanel.SetActive(false);
                HomePanel.SetActive(true);
            }
            if (LevelsPanel.activeSelf == true)
            {
                LevelsPanel.SetActive(false);
                HomePanel.SetActive(true);
            }
            if (SettingsPanel.activeSelf == true)
            {
                SettingsPanel.SetActive(false);
                HomePanel.SetActive(true);
            }
            if (HomePanel.activeSelf == false)
            {
                HomePanel.SetActive(true);
            }
        }
        if (sender == HomeBtnOnGO.gameObject)
        {
            CloseGame.gameObject.SetActive(true);
            if (GameOverPanel.activeSelf == true)
            {
                gameManagerRef.DisableAllLevel();
                GameOverPanel.SetActive(false);
                HomePanel.SetActive(true);
                if (levelText.gameObject.activeSelf)
                {
                    levelText.gameObject.SetActive(false);
                }
            }
        }
        if (sender == HomeBtnOnWin.gameObject)
        {
            CloseGame.gameObject.SetActive(true);
            if (WinPanel.activeSelf == true)
            {
                gameManagerRef.DisableAllLevel();
                WinPanel.SetActive(false);
                HomePanel.SetActive(true);
                if (levelText.gameObject.activeSelf)
                {
                    levelText.gameObject.SetActive(false);
                }
            }
        }
    }
    #endregion



}
