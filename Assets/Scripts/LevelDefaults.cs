using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDefaults : MonoBehaviour
{

    public GameObject Player;
    public GameObject Spawner;
    public Transform WinPointPos;

    public int Lives;
    public bool isGameOver;
    public bool isWin;
    public string playerPath = "Prefab/Player";

    [SerializeField]private WinPoint winPoint;

    private void Reset()
    {
        winPoint = FindObjectOfType<WinPoint>(true);
        Spawner = GetComponentInChildren<Spawner>(true).gameObject;
        WinPointPos = GetComponentInChildren<WinPointPosRef>(true).transform;
        Player = Resources.Load<GameObject>(playerPath);
        Lives = 2;
        isGameOver = false;
        isWin = false;
        Player.transform.position = Spawner.transform.position;
    }

    public void ResetBypass()
    {
        Spawner = GetComponentInChildren<Spawner>(true).gameObject;
        WinPointPos = GetComponentInChildren<WinPointPosRef>(true).transform;
        Player = Resources.Load<GameObject>(playerPath);
        if (Player != null )
        {
            Player = Instantiate(Player, Spawner.transform.position, Player.transform.rotation , this.transform);
            Debug.Log("Prefab loaded and instantiated successfully.");
        }
        else
        {
            Debug.LogError("Failed to load the prefab. Check the path and make sure the prefab is in the Resources folder.");
        }
        Lives = 2;
        isGameOver = false;
        isWin = false;
        Player.transform.position = Spawner.transform.position;
        winPoint.transform.position = WinPointPos.position;
    }

 


}
