using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Mirror;

namespace GJ.Networking
{
    public class GJ_NetworkManager : NetworkManager
    {
        // When the client connects to the server
        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            // Use the default implementation
            base.OnServerAddPlayer(conn);

            Debug.Log("Client connected to the server");
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

                for (int i = 0; i < 5; i++)
                {
                    char c = (char)('A' + Random.Range(0, 26));
                    serverInstance += c;
                }

                // Set the new server identity
                PlayerPrefs.SetString("Server_Instance", serverInstance);
            }

            // Send the webhook
            StartCoroutine(SendStartingWebhook());
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
        IEnumerator SendStartingWebhook()
        {
            // Send an API log to the discord channel to alert of server 
            WWWForm form = new WWWForm();
            form.AddField("content", $"Server **{PlayerPrefs.GetString("Server_Instance")}** starting / OS: {SystemInfo.operatingSystem}");

            // Create a post request for discord
            using (UnityWebRequest www = UnityWebRequest.Post("https://discord.com/api/webhooks/984072238745653328/BqutJJ6tYMVIJiTeb_IfOvcbIc_DK9mgv0Tu8JKJG-P5rd0mSVMl3uwso7Y2JREUxQuD", form))
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
            form.AddField("content", $"Server **{PlayerPrefs.GetString("Server_Instance")}** stopping!");

            // Create a post request for discord
            using (UnityWebRequest www = UnityWebRequest.Post("https://discord.com/api/webhooks/984072238745653328/BqutJJ6tYMVIJiTeb_IfOvcbIc_DK9mgv0Tu8JKJG-P5rd0mSVMl3uwso7Y2JREUxQuD", form))
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