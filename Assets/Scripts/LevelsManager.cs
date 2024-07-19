using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Date: 16-Jul-2024
/// Author: Nimroz Baloch
/// Purpose: This Script Controls All the UI Functionality.
/// </summary>
public class LevelsManager : MonoBehaviour
{
    #region Variables
    public List<LevelDefaults> LevelObjects;
    #endregion

    #region UnityMethods
    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            LevelObjects.Add(transform.GetChild(i).GetComponent<LevelDefaults>());
            LevelObjects[i].gameObject.SetActive(false);
        }
    }
    #endregion
}