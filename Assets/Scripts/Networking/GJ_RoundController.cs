using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace GJ.Networking
{
    public class GJ_RoundController : NetworkBehaviour
    {
        // Variables
        [Header("Round Settings")]
        [SerializeField] private float enemyAmountModifer = 1.1f;
    
        [Header("References")]
        [SerializeField] private GameObject enemyPrefab;

        public List<GameObject> enemies = new List<GameObject>();
        private int roundCounter = 0;
        public static GJ_RoundController instance { get; private set; }
        
        // Called when instance loading
        private void Awake()
        {
            // Check if there is an instance already
            if (instance != null)
                // Destroy this
                Destroy(this);
            else
                // Set the instance to this
                instance = this;
        }

        // Called before the first frame
        private void Start()
        {
            // Check if this is the server
            if (isServer)
                // Start the game
                StartGame();
        }

        // Server method to start the rounds
        [Server]
        public void StartGame()
        {
            StartCoroutine(StartNewRound());
        }

        // Method for starting a new round
        IEnumerator StartNewRound()
        {
            // Increment the counter
            roundCounter += 1;
            
            // Calculate how many enemies to spawn each round
            var enemiesToSpawn = Mathf.Round(roundCounter * enemyAmountModifer);

            // Spawn the enemies
            SpawnEnemy(enemiesToSpawn);

            // Wait until the enemies are all dead
            yield return new WaitUntil(() => enemies.Count == 0);

            // Wait round delay
            yield return new WaitForSeconds(5);

            // Restart to the next round
            StartCoroutine(StartNewRound());
        }

        [Server]
        private void SpawnEnemy(float enemyAmount)
        {
            // Loop through all of the amounts
            for (int i = 0; i < enemyAmount; i++)
            {
                // Spawn in an object and tell the clients about it
                GameObject enemy = Instantiate(enemyPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                NetworkServer.Spawn(enemy);
                
                // Add enemies
                enemies.Add(enemy);
            }
        }
    }
}