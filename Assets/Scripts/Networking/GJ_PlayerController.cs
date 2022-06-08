using UnityEngine;
using Mirror;
using TMPro;

namespace GJ.Networking
{
    [RequireComponent(typeof(CharacterController))]
    public class GJ_PlayerController : NetworkBehaviour
    {
        // Variables
        [Header("Player Data")]
        [SerializeField] [SyncVar(hook = nameof(UpdatePlayerDisplayName))] private string playerName;

        [Header("Character References")]
        [SerializeField] private TMP_Text playerNameText;
        [SerializeField] private CharacterController characterController;

        // Called before the first frame
        private void Start()
        {
            // Check if the player has authority
            if (!hasAuthority) return;
            
            // Set the players name on join
            CmdSetPlayerName(PlayerPrefs.GetString("Player_Username"));
        }

        #region Server Commands

        // Set the players name
        [Command]
        private void CmdSetPlayerName(string newPlayerName)
        {
            // TODO Player name sanitization
            
            // Update playName var
            playerName = newPlayerName;
        }

        #endregion
        
        #region Client Callbacks
        
        // Update the players name when the SyncVar is called
        private void UpdatePlayerDisplayName(string oldName, string newName)
        {
            // Update the players name with the new name
            playerNameText.text = newName;
        }
        
        #endregion
    }
}