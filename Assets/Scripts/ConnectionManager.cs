using UnityEngine;
using NativeWebSocket;
using System;

public class ConnectionManager : MonoBehaviour
{
    public static event Action<string> OnQuestion;
    public static event Action OnReject;
    public static event Action OnAccept;
    WebSocket webSocket;
    string[] users;
    public string currentLobbyInquirer;
    public string otherPlayer;
    public string filedir;
    public string[] SendMessage;
    public string[] chatInfo;
    


    private static ConnectionManager instance;

    public static ConnectionManager Instance { get { return instance; } }

    async void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);


        webSocket = new WebSocket("ws://localhost:8080");

        webSocket.OnOpen += () =>
        {
            Debug.Log("Connection Open");
        };

        webSocket.OnError += (e) =>
        {
            Debug.Log("Connection error" + e);
        };

        webSocket.OnClose += (e) =>
        {
            Debug.Log("Connection Closed");
        };

        webSocket.OnMessage += (bytes) =>
        {
            var message = System.Text.Encoding.UTF8.GetString(bytes);
            var newmessage = message.Split("|");
            string command = newmessage[0];
            switch (command)
            {
                case "1":
                    break;
                case "2":
                    users = null;
                    users = newmessage[1].Split("-");
                    break;
                case "3":
                    currentLobbyInquirer = newmessage[1];
                    OnQuestion?.Invoke(currentLobbyInquirer);
                    break;
                case "4":
                    if (newmessage[2] == "1")
                    {
                        otherPlayer = newmessage[1];
                        OnAccept?.Invoke();
                    }
                    if (newmessage[2] == "0")
                    {
                        OnReject?.Invoke();
                    }
                    break;
                case "5": //Check&Create ChatRoom
                    filedir = newmessage[1];
                    break;
                case "6":
                    chatInfo = newmessage[1].Split('₡');
                    break;
                case "7":
                    SendMessage = newmessage[1].Split('₡');

                    break;
                
                //6 - Receive Message
                //7 - Send Message
            }
            //Debug.Log(message);
        };

        await webSocket.Connect();

        
    }

    // Update is called once per frame
    void Update()
    {
        webSocket.DispatchMessageQueue();
    }

    public async void SendWebSocketMessage(string thing)
    {
        if (webSocket.State == WebSocketState.Open)
        {
            await webSocket.SendText(thing);
        }
    }

    [ContextMenu("5")]
    public void CheckForRoom()
    {
        SendWebSocketMessage("5|" + currentLobbyInquirer + "|" + NameLogic.username);
    }
    [ContextMenu("6")]
    public void GetChat()
    {
        SendWebSocketMessage("6|"+filedir);
        //SendWebSocketMessage("6|./Chats/1.json");
    }


    public string[] GetPlayerList()
    {
        string[] players = null;
        SendWebSocketMessage("2");
        players = users;
        return players;
    } 

    private async void OnApplicationQuit()
    {
        await webSocket.Close();
    }
}
