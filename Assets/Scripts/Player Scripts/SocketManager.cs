using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using Newtonsoft.Json.Linq;
using UnityEngine.Networking;
using System;

public class SocketManager : MonoBehaviour
{
    WebSocket socket;
    public GameObject player;
    public PlayerData playerData;
    public int the_Score;  
    public string scoreee;

    //Package URL for Newtonsoft JSON utilities
    string PackageURL = "https://github.com/jilleJr/Newtonsoft.Json-for-Unity.git#upm";

    private void OnEnable()
    {
        PlayerScript.player_Score += send_The_Score;
    }

    

    private void OnDisable()
    {
        PlayerScript.player_Score -= send_The_Score;
    }

    // Start is called before the first frame update
    void Start()
    {

        // socket = new WebSocket("ws://localhost:8080");
       // StartCoroutine(APP_Get_Locations());
       //  socket = new WebSocket("ws://127.0.0.1:4040");
        socket = new WebSocket("ws://3.237.18.47:443");
        socket.Connect();


        socket.OnOpen += (sender, e) =>
        {
            Debug.Log(" connection establish");
        };


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
        Debug.Log(" run the start function :)");
    }

    public void send_The_Score(string key, int _score)
    {
        if(key == "new_score")
        {
            the_Score = _score;
            scoreee = _score.ToString();
        }
       
    }

    IEnumerator APP_Get_Locations()
    {
        
        using (UnityWebRequest www = UnityWebRequest.Get("https://8c40-2a0d-6fc2-56c0-3300-c4d0-7df8-59db-1005.eu.ngrok.io" + "/" ))
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

        WWWForm _form = new WWWForm();
        _form.AddField("myField", scoreee);
        using (UnityWebRequest www = UnityWebRequest.Post("https://8c40-2a0d-6fc2-56c0-3300-c4d0-7df8-59db-1005.eu.ngrok.io/", _form))
        {
            Debug.Log("Form the POST !!!!  " + scoreee);
            yield return www.SendWebRequest();         
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
            playerData.score = the_Score;
            System.DateTime epochStart = new System.DateTime(1970, 1, 1, 8, 0, 0, System.DateTimeKind.Utc);
            double timestamp = (System.DateTime.UtcNow - epochStart).TotalSeconds;
            //Debug.Log(timestamp);
            playerData.timestamp = timestamp;

            string playerDataJSON = JsonUtility.ToJson(playerData);
            socket.Send(playerDataJSON);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            StartCoroutine(APP_Get_Locations());
          //  string messageJSON = "{\"message\": \"Some Message From Client\"}";
         //   socket.Send(messageJSON);
        }
    }

    private void OnDestroy()
    {
        //Close socket when exiting application
        socket.Close();
    }

}