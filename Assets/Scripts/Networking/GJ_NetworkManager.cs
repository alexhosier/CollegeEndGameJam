using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Mirror;
using Unity.RemoteConfig;
using UnityEngine.Profiling;

namespace GJ.Networking
{
    public class GJ_NetworkManager : NetworkManager
    {
        // Variables
        private struct UserAttributes {}
        private struct AppAttributes {}

        private string discordWebhookURL = "";

        // Called before first frame
        public override void Awake()
        {
            // Use the default implementation
            base.Awake();
            
            // Fetch the Discord webhook url from the remote config
            ConfigManager.FetchCompleted += SetConfigs;
            ConfigManager.FetchConfigs<UserAttributes, AppAttributes>(new UserAttributes(), new AppAttributes());
        }

        // Callback for FetchCompleted event
        private void SetConfigs(ConfigResponse response)
        {
            discordWebhookURL = ConfigManager.appConfig.GetString("discord_webhook_url");
        }

        // When the client connects to the server
        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            // Use the default implementation
            base.OnServerAddPlayer(conn);
        }

        // When the server starts
        public override void OnStartServer()
        {
            // Use the default implementation
            base.OnStartServer();

            // Check if the server has an instance already generated
            if (!PlayerPrefs.HasKey("Server_Instance"))
            {
                // For later use
                var serverInstance = "#";

                // Generate four characters
                for (int i = 0; i < 5; i++)
                {
                    // Random character
                    char c = (char)('A' + Random.Range(0, 26));
                    serverInstance += c;
                }

                // Set the new server identity
                PlayerPrefs.SetString("Server_Instance", serverInstance);
            }

            // Send the starting webhook
            StartCoroutine(SendStartingWebhook());
            
            // Send the update webhook
            StartCoroutine(SendUpdateWebhook());
        }

        // When the server stops
        public override void OnStopServer()
        {
            // Use the default implementation
            base.OnStopServer();

            // Send the webhook
            StartCoroutine(SendStoppingWebhook());
        }

        #region Webhooks
        IEnumerator SendUpdateWebhook()
        {
            // Wait seconds
            yield return new WaitForSeconds(30);
            
            // Create new user information
            WWWForm form = new WWWForm();
            form.AddField("username", "Server Status");
            form.AddField("content", $"Server **{PlayerPrefs.GetString("Server_Instance")}** / **Clients:** {numPlayers} / **RAM Allocated:** {Profiler.GetTotalAllocatedMemoryLong()/1024/1024}MB");
            
            // Create a post request for discord
            using (UnityWebRequest www = UnityWebRequest.Post(discordWebhookURL, form))
            {
                // Send the web request
                yield return www.SendWebRequest();

                // Check if the request was sent
                if (www.result != UnityWebRequest.Result.Success)
                {
                    // Log the error
                    Debug.LogError(www.error);
                }
            }
            
            // Restart the process
            StartCoroutine(SendUpdateWebhook());
        }

        IEnumerator SendStartingWebhook()
        {
            // Send an API log to the discord channel to alert of server 
            WWWForm form = new WWWForm();
            form.AddField("username", "Server Status");
            form.AddField("content", $"Server **{PlayerPrefs.GetString("Server_Instance")}** starting / **OS:** {SystemInfo.operatingSystem} / **RAM:** {SystemInfo.systemMemorySize/1000}GB");

            // Create a post request for discord
            using (UnityWebRequest www = UnityWebRequest.Post(discordWebhookURL, form))
            {
                // Send the web request
                yield return www.SendWebRequest();

                // Check if the request was sent
                if (www.result != UnityWebRequest.Result.Success)
                {
                    // Log the error
                    Debug.LogError(www.error);
                }
            }
        }

        IEnumerator SendStoppingWebhook()
        {
            // Send an API log to the discord channel to alert of server stopping
            WWWForm form = new WWWForm();
            form.AddField("username", "Server Status");
            form.AddField("content", $"Server **{PlayerPrefs.GetString("Server_Instance")}** stopping!");

            // Create a post request for discord
            using (UnityWebRequest www = UnityWebRequest.Post(discordWebhookURL, form))
            {
                // Send the web request
                yield return www.SendWebRequest();

                // Check if the request was sent
                if (www.result != UnityWebRequest.Result.Success)
                {
                    // Log the error
                    Debug.LogError(www.error);
                }
            }
        }
        #endregion
    }
}