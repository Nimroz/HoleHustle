using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollider : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
    }

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
    }
}
