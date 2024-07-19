using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDefaults : MonoBehaviour
{

    public GameObject Player;
    public GameObject Spawner;
    public int Lives;
    public bool isGameOver;
    public bool isWin;

    private void Start()
    {
        ResetBypass();
    }

    public void ResetBypass()
    {
        Player = GetComponentInChildren<PlayerMovements>(true).gameObject;
        Spawner = GetComponentInChildren<Spawner>(true).gameObject;
        Lives = 2;
        isGameOver = false;
        isWin = false;
        Player.transform.position = Spawner.transform.position;
    }

 


}
