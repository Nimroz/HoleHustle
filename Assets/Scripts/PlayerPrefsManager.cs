using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    private const string LevelKey = "UnlockedLevel"; // Key to store the highest unlocked level

    /// <summary>
    /// Saves the highest unlocked level.
    /// </summary>
    /// <param name="levelIndex">Index of the unlocked level.</param>
    public static void SaveUnlockedLevel(int levelIndex)
    {
        int currentUnlockedLevel = GetUnlockedLevel();
        if (levelIndex > currentUnlockedLevel)
        {
            PlayerPrefs.SetInt(LevelKey, levelIndex);
            PlayerPrefs.Save();
            Debug.Log("Level " + levelIndex + " unlocked and saved.");
        }
    }

    /// <summary>
    /// Retrieves the highest unlocked level.
    /// </summary>
    /// <returns>The index of the highest unlocked level.</returns>
    public static int GetUnlockedLevel()
    {
        return PlayerPrefs.GetInt(LevelKey, 0); // Default to 0 (first level) if nothing is saved
    }

    /// <summary>
    /// Resets the saved level progress.
    /// </summary>
    public static void ResetLevelProgress()
    {
        PlayerPrefs.DeleteKey(LevelKey);
        PlayerPrefs.Save();
        Debug.Log("Level progress reset.");
    }

}
