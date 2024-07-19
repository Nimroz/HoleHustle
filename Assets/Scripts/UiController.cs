using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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

    #endregion

    #region UnityMethods
    private void Start()
    {
        gameManagerRef = FindObjectOfType<GameManager>();
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
        }
        if (sender == SettingsBtn.gameObject)
        {
            SettingsPanel.SetActive(true);
            HomePanel.SetActive(false);
        }
        if (sender == ShopBtn.gameObject)
        {
            ShopPanel.SetActive(true);
            HomePanel.SetActive(false);
        }
        if (sender == HomeBtn.gameObject)
        {
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
            if (GameOverPanel.activeSelf == true)
            {
                GameOverPanel.SetActive(false);
                HomePanel.SetActive(true);
            }
        }
        if (sender == HomeBtnOnWin.gameObject)
        {
            if (WinPanel.activeSelf == true)
            {
                WinPanel.SetActive(false);
                HomePanel.SetActive(true);
            }
        }
    }
    #endregion

}
