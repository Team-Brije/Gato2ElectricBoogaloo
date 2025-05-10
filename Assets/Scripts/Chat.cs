using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

using System;
using NUnit.Framework;
public class Chat : MonoBehaviour
{
  public TMP_InputField MessageInput;
  ConnectionManager wsManager;
    public GameObject contenido;
    public GameObject prefabChat;
    public List<string> messages = new List<string>();
    

 void Start()
    {
        wsManager = ConnectionManager.Instance;   
    }

    private void OnEnable()
    {
        Coroutine UCHat = StartCoroutine(UpdateChat());
    }
    public void SendMessage()
    {
        string Message = MessageInput.text;
        string Time = System.DateTime.UtcNow.ToLocalTime().ToString("HH:mm:ss");
        string Date = System.DateTime.UtcNow.ToLocalTime().ToString("dd/MM/yyyy");
        wsManager.SendWebSocketMessage("7|" + NameLogic.username + "|" + wsManager.otherPlayer + "|" + Message + "₡" + Time + "₡" + Date + "|" + wsManager.filedir);
        wsManager.GetChat();
        MessageInput.text = "";
  }
    public IEnumerator UpdateChat()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            wsManager.GetChat();

            //Debug.Log(wsManager.filedir);
            if (wsManager.chatInfo.Length != 0)
            {
                for (int indexChatUpdate = 0; indexChatUpdate < wsManager.chatInfo.Length; indexChatUpdate += 4)
                {
                    //wsManager.GetChat();
                    GameObject newPrefabChat = prefabChat;
                    newPrefabChat.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = wsManager.chatInfo[indexChatUpdate];
                    newPrefabChat.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = wsManager.chatInfo[indexChatUpdate+3];
                    newPrefabChat.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = wsManager.chatInfo[indexChatUpdate+2];
                    newPrefabChat.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>().text = wsManager.chatInfo[indexChatUpdate+1];
                    string iddate = wsManager.chatInfo[indexChatUpdate+2]+wsManager.chatInfo[indexChatUpdate+3];
                    if (!messages.Contains(iddate))
                    {
                        messages.Add(iddate);
                        Instantiate(newPrefabChat, contenido.transform);
                        contenido.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 325);
                    }
                    //string cosa = wsManager.chatInfo[indexChatUpdate]+wsManager.chatInfo[indexChatUpdate+1]+wsManager.chatInfo[indexChatUpdate+2]+wsManager.chatInfo[indexChatUpdate+3];

                }
            }
        }
    }
    [ContextMenu("cosa6")]
    public void temp()
    {
        for (int indexChatUpdate = 0; indexChatUpdate < wsManager.chatInfo.Length; indexChatUpdate += 4)
        {
            GameObject newPrefabChat = prefabChat;
            newPrefabChat.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = wsManager.chatInfo[indexChatUpdate];
            newPrefabChat.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = wsManager.chatInfo[indexChatUpdate+3];
            newPrefabChat.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = wsManager.chatInfo[indexChatUpdate+2];
            newPrefabChat.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>().text = wsManager.chatInfo[indexChatUpdate+1];
            //string cosa = wsManager.chatInfo[indexChatUpdate]+wsManager.chatInfo[indexChatUpdate+1]+wsManager.chatInfo[indexChatUpdate+2]+wsManager.chatInfo[indexChatUpdate+3];
            Instantiate(newPrefabChat, contenido.transform);
        }
    }
}
