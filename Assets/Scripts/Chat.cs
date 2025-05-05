using UnityEngine;
using TMPro;

public class Chat : MonoBehaviour
{
  public TMP_InputField MessageInput;
  ConnectionManager wsManager;
 

  public void SendMessage()
  {
    string Message = MessageInput.text;
    wsManager.SendWebSocketMessage("7|" + Message);
    Debug.Log(Message);
  }
}
