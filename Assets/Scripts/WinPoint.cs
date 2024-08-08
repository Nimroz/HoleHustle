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
    public List<ParticleSystem> particleSystems;
    public UiController uiController;
    public GameManager  GameManager;
    #endregion

    #region UnityMethods
    private void Start()
    {
        GameManager = FindObjectOfType<GameManager>();
        uiController = GameManager.uiControllerRef;

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
        if (particleSystems.Count > 0)
        {
            int randomIndex = Random.Range(0, particleSystems.Count);
            ParticleSystem selectedParticleSystem = particleSystems[randomIndex];
            selectedParticleSystem.gameObject.SetActive(true);
            selectedParticleSystem.Play();
            StartCoroutine(DisableParticleSystem());
        }
       // ParticleSystem.Play();
        GameManager.OnWin?.Invoke();
    }

    IEnumerator DisableParticleSystem() 
    {
        yield return new WaitForSeconds(.9f);
        foreach (var item in particleSystems)
        {
            if(item.gameObject.activeSelf)
            item.gameObject.SetActive(false);
        }
    }
    #endregion

}
