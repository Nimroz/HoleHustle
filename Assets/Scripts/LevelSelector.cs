using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
/// <summary>
/// Date: 16-Jul-2024
/// Author: Nimroz Baloch
/// Purpose: This Script Controls All the UI Functionality.
/// </summary>
public class LevelSelector : MonoBehaviour
{
    #region Variables
    public LevelsManager LevelsManager;
    public GameManager gameManager;
    public List<Button> LevelButtons;
    public int currentLevel = 0;
    #endregion



    #region UnityMethods
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        LevelsManager = FindObjectOfType<LevelsManager>(true);
        for (int i = 0; i < transform.childCount; i++)
        {
            int levelIndex = i; // Local variable to capture current index
            LevelButtons.Add(transform.GetChild(i).GetComponent<Button>());
            LevelButtons[i].onClick.AddListener(() =>
            {
                // Start the level associated with this button
                StartLevel(levelIndex);
            });
        }
    }
    #endregion

    #region CustomMethods
    public void StartLevel(int levelNo) 
    {
        if(LevelsManager.transform.GetChild(levelNo).gameObject != null) 
        {
            for (int i = 0; i < LevelsManager.transform.childCount; i++)
            {
                LevelsManager.LevelObjects[i].gameObject.SetActive(false);
            }
            LevelsManager.LevelObjects[levelNo].gameObject.SetActive(true);
            if (!gameManager.uiControllerRef.levelText.gameObject.activeSelf) 
            {
                gameManager.uiControllerRef.levelText.gameObject.SetActive(true);
            }
            gameManager.uiControllerRef.levelText.text = "Level :  " + (levelNo + 1).ToString();
            LevelsManager.LevelObjects[levelNo].GetComponent<LevelDefaults>().ResetBypass();
            Debug.Log("LevelOn");
        }
        gameManager.LivesReload();
        currentLevel = levelNo;
        transform.parent.gameObject.SetActive(false);
    }
    #endregion

}