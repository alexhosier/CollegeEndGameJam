using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GJ_MainMenuController : MonoBehaviour
{
    // Variables
    [Header("References")]
    [SerializeField] private TMP_Text versionText;
    [SerializeField] private GameObject usernamePanel;
    [SerializeField] private TMP_InputField usernameInput;

    // Start is called before the first frame update
    void Start()
    {
        // Check if the user has not set a name
        if(!PlayerPrefs.HasKey("Player_Username"))
        {
            // Set the panel to display
            usernamePanel.SetActive(true);
        }
    }

    // Setup the users username
    public void SetUsername()
    {
        // Set the player preferences
        PlayerPrefs.SetString("Player_Username", usernameInput.text);

        // Hide the username panel
        usernamePanel.SetActive(false);
    }
}
