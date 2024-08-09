using UnityEngine;

public class OpenLinkedIn : MonoBehaviour
{
    public string linkedInURL = "https://www.linkedin.com/in/yourprofile/";

    public void OpenLinkedInLink()
    {
        Application.OpenURL(linkedInURL);
    }
}