using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUpMessages : MonoBehaviour
{

    public Text[] textObj;

    private void Start()
    {
        textObj = GetComponentsInChildren<Text>();
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }


}
