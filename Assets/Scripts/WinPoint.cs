using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Date: 16-Jul-2024
/// Author: Nimroz Baloch
/// Purpose: This Script Controls Win Point Functionality.
/// </summary>
public class WinPoint : MonoBehaviour
{
    #region Variables
    public ParticleSystem ParticleSystem;
    public UiController uiController;
    public GameManager  GameManager;
    #endregion

    #region UnityMethods
    private void Start()
    {
        GameManager = FindAnyObjectByType<GameManager>();
        uiController = FindAnyObjectByType<UiController>();
        uiController = GameManager.uiControllerRef;
        ParticleSystem = FindAnyObjectByType<ParticleSystem>();

        if (GameManager != null)
        {
            GameManager.OnWin += GameManager.OnWinning; // Subscribe to the action
            Debug.Log("Onwin Subscribed");
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if( collision.gameObject.tag == "Player") 
        {
            Destroy( collision.gameObject );
            WinMethod();
        }
    }
    #endregion

    #region CustomMethods
    public void WinMethod() 
    {
        ParticleSystem.Play();
        GameManager.OnWin?.Invoke();
    }
    #endregion

}
