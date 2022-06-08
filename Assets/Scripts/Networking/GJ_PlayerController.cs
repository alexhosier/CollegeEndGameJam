using System.Collections;
using System.Collections.Generic;
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

        #region Client Callbacks
        private void UpdatePlayerDisplayName(string oldName, string newName)
        {
            playerNameText.text = newName;
        }
        #endregion
    }
}