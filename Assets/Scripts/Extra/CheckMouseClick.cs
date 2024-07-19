using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckMouseClick : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Create a ray from the camera through the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Perform the raycast
            if (Physics.Raycast(ray, out hit))
            {
                // Check if the object has a specific component or tag (optional)
                // For example, if you want to check if it's an object with a specific tag:
                if (hit.collider.CompareTag("Clickable"))
                {
                    Debug.Log("Clicked on: " + hit.collider.name);
                }
                else
                {
                    Debug.Log("Clicked on: " + hit.collider.name);
                }
            }
        }
    }
}
