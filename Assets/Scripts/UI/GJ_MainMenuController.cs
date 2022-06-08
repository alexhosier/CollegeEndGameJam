using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.RemoteConfig;
using TMPro;

public class GJ_MainMenuController : MonoBehaviour
{
    // Variables
    [Header("References")]
    [SerializeField] private TMP_Text versionText;
    [SerializeField] private GameObject usernamePanel;
    [SerializeField] private TMP_InputField usernameInput;
    
    public struct userAttributes {}
    public struct appAttributes {}

    private void Awake()
    {
        ConfigManager.FetchCompleted += SetVersionText;
        ConfigManager.FetchConfigs<userAttributes, appAttributes>(new userAttributes(), new appAttributes());
    }

    // Start is called before the first frame update
    private void Start()
    {
        // Check if the user has not set a name
        if(!PlayerPrefs.HasKey("Player_Username"))
        {
            // Set the panel to display
            usernamePanel.SetActive(true);
        }
    }

    private void SetVersionText(ConfigResponse response)
    {
        versionText.text = ConfigManager.appConfig.GetString("game_version");
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
