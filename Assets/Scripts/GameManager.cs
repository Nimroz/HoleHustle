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

    private void Awake()
    {
        SceneManager.LoadSceneAsync("UserInterface", LoadSceneMode.Additive);
    }
    private void Start()
    {
        uiControllerRef = FindObjectOfType<UiController>(true);
        levelsParentRef = FindObjectOfType<LevelsManager>(true);
        levelSelectorRef = FindObjectOfType<LevelSelector>(true);
        LevelDefaultsRef = FindObjectOfType<LevelDefaults>(true);
    }
    #endregion

    #region CustomMethods
    public void OnWinning()
    {
        StartCoroutine(OnWinCourotine());
        Debug.Log("You Win");
    }

    IEnumerator OnWinCourotine()
    {
        yield return new WaitForSeconds(.5f);
        uiControllerRef.WinPanel.SetActive(true);
        LevelDefaultsRef.isWin = true;
    }

    public void OnGameIsOver()
    {
        StartCoroutine(GameOverCoroutine());
        Debug.Log("You Lose");
    }
    IEnumerator GameOverCoroutine()
    {
        yield return new WaitForSeconds(.5f);
        uiControllerRef.GameOverPanel.SetActive(true);
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
        tempLevelObj.Lives = 2;
        tempLevelObj.Player.transform.position = tempLevelObj.Spawner.transform.position;
        tempLevelObj.isGameOver = false;
        LivesReload();
        //Debug.Log("Player Pos" + LevelDefaultsRef.Lives);
        //LevelDefaultsRef.Player.transform.position = LevelDefaultsRef.Spawner.transform.position;
        //LevelDefaultsRef.isGameOver = false;
    }

    public void NextLevelBtn() 
    {
        uiControllerRef.WinPanel.SetActive(false);
        //Debug.Log("Curr Lvl# " + levelsParentRef.LevelObjects[levelSelectorRef.currentLevel]);
        levelsParentRef.LevelObjects[levelSelectorRef.currentLevel].gameObject.SetActive(false);
        levelSelectorRef.currentLevel = levelSelectorRef.currentLevel + 1; 
        levelsParentRef.LevelObjects[levelSelectorRef.currentLevel].gameObject.SetActive(true);
        LivesReload();
        //Debug.Log("Curr Lvl# aftr" + levelsParentRef.LevelObjects[levelSelectorRef.currentLevel]);
    }

    public void LivesRemoves(int life)
    {
        for (int i = 0; i < uiControllerRef.LivesParent.transform.childCount; i++)
        {
            uiControllerRef.LivesParent.transform.GetChild(life).gameObject.SetActive(false);
        }
        Debug.Log("You Miss");

    }

     public void LivesReload()
    {
        for (int i = 0; i < uiControllerRef.LivesParent.transform.childCount; i++)
        {
            if(!uiControllerRef.LivesParent.transform.GetChild(i).gameObject.activeSelf)
            uiControllerRef.LivesParent.transform.GetChild(i).gameObject.SetActive(true);
        }
        Debug.Log("LivesReload");
    }

    public void CloseGame() 
    {
        Application.Quit();
        Debug.Log("Close Game");
    }
    #endregion

}
