using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script manages the obstacle behavior and interaction with the player.
/// </summary>
public class Obstacle : MonoBehaviour
{
    #region Variables
    public bool isHit = false;
    public GameManager gameManager;
    public LevelDefaults levelDefaults;
    #endregion

    #region UnityMethods
    private void OnEnable()
    {
        gameManager = FindObjectOfType<GameManager>(true);
        levelDefaults = GetComponentInParent<LevelDefaults>(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isHit)
        {
            isHit = true; // Prevent multiple hits on this obstacle
            levelDefaults.Lives -= 1; // Deduct life here
            gameManager.OnMiss?.Invoke(levelDefaults.Lives);
            Debug.Log("Obstacle Hit");

            if (levelDefaults.Lives < 0)
            {
                gameManager.OnGameIsOver();
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isHit = false; // Reset isHit when the player leaves the obstacle
        }
    }
    #endregion
}
