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
        int highestUnlockedLevel = PlayerPrefsManager.GetUnlockedLevel();

        for (int i = 0; i < transform.childCount; i++)
        {
            int levelIndex = i; // Local variable to capture current index
            Button levelButton = transform.GetChild(i).GetComponent<Button>();
            LevelButtons.Add(levelButton);
            levelButton.onClick.AddListener(() =>
            {
                // Start the level associated with this button
                StartLevel(levelIndex);
            });

            // Set interactability based on unlocked levels
            levelButton.interactable = (i + 1 <= highestUnlockedLevel);
        }
    }
    #endregion

    #region CustomMethods
    public void StartLevel(int levelNo)
    {
        if (LevelsManager.transform.GetChild(levelNo).gameObject != null)
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

        // Unlock the next level if this level is completed
        if (levelNo + 1 <= LevelsManager.transform.childCount)
        {
            PlayerPrefsManager.SaveUnlockedLevel(levelNo + 1);
            LevelButtons[levelNo + 1].interactable = true;
        }
    }
    #endregion

}