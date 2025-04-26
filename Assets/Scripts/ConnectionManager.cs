using UnityEngine;
using NativeWebSocket;

public class ConnectionManager : MonoBehaviour
{
    WebSocket webSocket;
    string[] users;

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
                    
            }
            Debug.Log(message);
        };

        await webSocket.Connect();

        
    }

    // Update is called once per frame
    void Update()
    {
        webSocket.DispatchMessageQueue();
    }

    async void SendWebSocketMessage(string thing)
    {
        if (webSocket.State == WebSocketState.Open)
        {
            await webSocket.SendText(thing);
        }
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
