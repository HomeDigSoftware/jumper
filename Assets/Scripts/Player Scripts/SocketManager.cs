using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using Newtonsoft.Json.Linq;
using UnityEngine.Networking;

public class SocketManager : MonoBehaviour
{
    WebSocket socket;
    public GameObject player;
    public PlayerData playerData;

    //Package URL for Newtonsoft JSON utilities
    string PackageURL = "https://github.com/jilleJr/Newtonsoft.Json-for-Unity.git#upm";

    // Start is called before the first frame update
    void Start()
    {

       // socket = new WebSocket("ws://localhost:8080");
        socket = new WebSocket("ws://https://66d6-2a0d-6fc2-56c0-3300-c4d0-7df8-59db-1005.eu.ngrok.io/");
        socket.Connect();
        StartCoroutine(APP_Get_Locations());
                    //WebSocket onMessage function
        socket.OnMessage += (sender, e) =>
        {

            //If received data is type text...
            if (e.IsText)
            {
                //Debug.Log("IsText");
                //Debug.Log(e.Data);
                JObject jsonObj = JObject.Parse(e.Data);

                //Get Initial Data server ID data (From intial serverhandshake
                if (jsonObj["id"] != null)
                {
                    //Convert Intial player data Json (from server) to Player data object
                    PlayerData tempPlayerData = JsonUtility.FromJson<PlayerData>(e.Data);
                    playerData = tempPlayerData;
                    Debug.Log("player ID is " + playerData.id);
                    return;
                }

            }

        };

        //If server connection closes (not client originated)
        socket.OnClose += (sender, e) =>
        {
            Debug.Log(e.Code);
            Debug.Log(e.Reason);
            Debug.Log("Connection Closed!");
        };
    }
    IEnumerator APP_Get_Locations()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("https://66d6-2a0d-6fc2-56c0-3300-c4d0-7df8-59db-1005.eu.ngrok.io" + "/"))
        {
            Debug.Log("Form upload complete!");
            yield return www.SendWebRequest();
            int num = 0;
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (socket == null)
        {
            return;
        }

        //If player is correctly configured, begin sending player data to server
        if (player != null && playerData.id != "")
        {
            Debug.Log("player ID is Connected" + playerData.id);
            //Grab player current position and rotation data
            playerData.xPos = player.transform.position.x;
            playerData.yPos = player.transform.position.y;
            playerData.zPos = player.transform.position.z;

            System.DateTime epochStart = new System.DateTime(1970, 1, 1, 8, 0, 0, System.DateTimeKind.Utc);
            double timestamp = (System.DateTime.UtcNow - epochStart).TotalSeconds;
            //Debug.Log(timestamp);
            playerData.timestamp = timestamp;

            string playerDataJSON = JsonUtility.ToJson(playerData);
            socket.Send(playerDataJSON);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            string messageJSON = "{\"message\": \"Some Message From Client\"}";
            socket.Send(messageJSON);
        }
    }

    private void OnDestroy()
    {
        //Close socket when exiting application
        socket.Close();
    }

}