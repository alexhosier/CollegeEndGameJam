using UnityEngine;
using UnityEditor;

namespace GJ.Editor
{
    public class GJ_PlayerPrefsDelete : MonoBehaviour
    {
        [MenuItem("Game Jam/Player Prefs/Reset")]
        private static void ResetPlayerPrefs()
        {
            // Delete all of the player prefs
            PlayerPrefs.DeleteAll();

            // Log to the console what has happend
            Debug.Log("Player Prefs Reset");
        }
    }
}
