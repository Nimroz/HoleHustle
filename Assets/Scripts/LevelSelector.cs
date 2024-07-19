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
    public LevelsManager LevelsParent;
    public GameManager GameManager;
    public List<Button> LevelButtons;
    public int currentLevel = 0;
    #endregion



    #region UnityMethods
    private void Start()
    {
        GameManager = FindObjectOfType<GameManager>();
        LevelsParent = FindObjectOfType<LevelsManager>(true);
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
        if(LevelsParent.transform.GetChild(levelNo).gameObject != null) 
        {
            for (int i = 0; i < LevelsParent.transform.childCount; i++)
            {
            LevelsParent.LevelObjects[i].gameObject.SetActive(false);
            }
            LevelsParent.LevelObjects[levelNo].gameObject.SetActive(true);
            Debug.Log("LevelOn");
        }
        GameManager.LivesReload();
        currentLevel = levelNo;
        transform.parent.gameObject.SetActive(false);
    }
    #endregion

}