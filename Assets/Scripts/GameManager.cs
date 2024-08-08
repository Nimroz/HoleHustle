using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Variables
    public Action OnWin;
    public Action OnGameOver;
    public Action<int> OnMiss;
    public UiController uiControllerRef;
    public LevelSelector levelSelectorRef;
    public LevelsManager levelsParentRef;
    public LevelDefaults LevelDefaultsRef;
    #endregion

    #region UnityMethods
    private void OnEnable()
    {
        OnMiss += LivesRemoves;
    }

    private void OnDisable()
    {
        OnMiss -= LivesRemoves;
    }

    private void Start()
    {
        uiControllerRef = FindObjectOfType<UiController>(true);
        levelsParentRef = FindObjectOfType<LevelsManager>(true);
        levelSelectorRef = FindObjectOfType<LevelSelector>(true);
        LevelDefaultsRef = levelsParentRef.transform.GetChild(0).GetComponent<LevelDefaults>();
    }
    #endregion

    #region CustomMethods
    public void OnWinning()
    {
        LevelDefaultsRef.isWin = true;
        if (uiControllerRef.GameOverPanel.activeSelf)
        {
            uiControllerRef.GameOverPanel.SetActive(false);
        }
        StartCoroutine(OnWinCourotine());
        Debug.Log("You Win");
    }

    IEnumerator OnWinCourotine()
    {
        yield return new WaitForSeconds(1f);
        uiControllerRef.WinPanel.SetActive(true);
        Destroy(LevelDefaultsRef.Player);
    }

    public void OnGameIsOver()
    {
        if (!LevelDefaultsRef.isWin)
        {
            StartCoroutine(GameOverCoroutine());
            Debug.Log("You Lose");
        }
    }

    IEnumerator GameOverCoroutine()
    {
        yield return new WaitForSeconds(1.1f);
        uiControllerRef.GameOverPanel.SetActive(true);
        Destroy(LevelDefaultsRef.Player);
    }

    public void OnLevelReload()
    {
        for (int i = 0; i < levelsParentRef.transform.childCount; i++)
        {
            levelsParentRef.LevelObjects[i].gameObject.SetActive(false);
        }
        levelsParentRef.LevelObjects[levelSelectorRef.currentLevel].gameObject.SetActive(true);
        uiControllerRef.GameOverPanel.SetActive(false);
        LevelDefaults tempLevelObj = levelsParentRef.LevelObjects[levelSelectorRef.currentLevel].GetComponent<LevelDefaults>();
        tempLevelObj.ResetBypass();
        LivesReload();
    }

    public void NextLevelBtn()
    {
        uiControllerRef.WinPanel.SetActive(false);
        levelsParentRef.LevelObjects[levelSelectorRef.currentLevel].gameObject.SetActive(false);
        levelSelectorRef.currentLevel = levelSelectorRef.currentLevel + 1;
        if (levelSelectorRef.currentLevel >= 18)
        {
            levelSelectorRef.currentLevel = 0;
        }
        LevelDefaultsRef = levelsParentRef.LevelObjects[levelSelectorRef.currentLevel].GetComponent<LevelDefaults>();
        LevelDefaultsRef.ResetBypass();
        uiControllerRef.levelText.text = "Level: " + (levelSelectorRef.currentLevel + 1).ToString();
        levelsParentRef.LevelObjects[levelSelectorRef.currentLevel].gameObject.SetActive(true);
        LivesReload();
    }

    public void LivesRemoves(int life)
    {
        // Check if 'life' is within valid range
        if (life < 0 || life >= uiControllerRef.LivesParent.transform.childCount)
        {
            Debug.LogError("Life parameter out of bounds: " + life);
            return;
        }

        // Disable the UI element corresponding to the current life
        Transform lifeTransform = uiControllerRef.LivesParent.transform.GetChild(life);
        if (lifeTransform != null)
        {
            lifeTransform.gameObject.SetActive(false);
        }

        Debug.Log("Life removed at index: " + life);
    }

    public void LivesReload()
    {
        for (int i = 0; i < uiControllerRef.LivesParent.transform.childCount; i++)
        {
            if (!uiControllerRef.LivesParent.transform.GetChild(i).gameObject.activeSelf)
                uiControllerRef.LivesParent.transform.GetChild(i).gameObject.SetActive(true);
        }
        Debug.Log("LivesReload");
    }

    public void DisableAllLevel()
    {
        for (int i = 0; i < levelsParentRef.transform.childCount; i++)
        {
            if (levelsParentRef.LevelObjects[i].gameObject.activeSelf)
            {
                levelsParentRef.LevelObjects[i].gameObject.SetActive(false);
            }
        }
    }

    public void CloseGame()
    {
        Application.Quit();
        Debug.Log("Close Game");
    }
    #endregion
}
