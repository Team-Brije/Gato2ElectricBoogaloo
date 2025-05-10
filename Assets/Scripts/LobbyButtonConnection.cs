using System;
using TMPro;
using UnityEngine;

public class LobbyButtonConnection : MonoBehaviour
{
    public static event Action OnReject;
    public static event Action OnHandshake;
    ConnectionManager wsManager;
    string lobbyname;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        wsManager = ConnectionManager.Instance;   
        lobbyname = GetComponentInChildren<TextMeshProUGUI>().text;
    }

    public void SendHandshake()
    {
        wsManager.SendWebSocketMessage("3|"+lobbyname+"|"+NameLogic.username);
        wsManager.otherPlayer = lobbyname;
        OnHandshake?.Invoke();
    }

    public void AcceptGame()
    {
        wsManager.SendWebSocketMessage("4|" + wsManager.currentLobbyInquirer + "|" + NameLogic.username+"|1");
        wsManager.otherPlayer = wsManager.currentLobbyInquirer;
        wsManager.CheckForRoom();
        //Add scenemanager 
    }

    public void RejectGame()
    {
        wsManager.SendWebSocketMessage("4|" + wsManager.currentLobbyInquirer + "|" + NameLogic.username + "|0");
        OnReject?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
