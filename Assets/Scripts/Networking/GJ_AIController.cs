using System;
using System.Collections;
using System.Collections.Generic;
using GJ.Networking;
using Mirror;
using UnityEngine;
using UnityEngine.AI;

namespace GJ.Networking
{
    public class GJ_AIController : NetworkBehaviour
    {
        // Variables
        [Header("References")]
        [SerializeField] private NavMeshAgent agent;

        // Command to move the enemy on the server
        [Command]
        private void CmdMoveEnemy()
        {
            
        }
        
        // When the GameObject is destroyed
        private void OnDestroy()
        {
            // Remove this enemy from the list
            GJ_RoundController.instance.enemies.Remove(this.gameObject);
        }
    }
}