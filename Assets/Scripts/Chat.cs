using UnityEngine;
using TMPro;
public class Chat : MonoBehaviour
{
  public TMP_InputField MessageInput;
  ConnectionManager wsManager;

 void Start()
    {
        wsManager = ConnectionManager.Instance;   
    }
  public void SendMessage()
  {
    string Message = MessageInput.text;
    string Time = System.DateTime.UtcNow.ToLocalTime().ToString("HH:mm");
    string Date = System.DateTime.UtcNow.ToLocalTime().ToString("dd/MM/yyyy");
    wsManager.SendWebSocketMessage("7|" + NameLogic.username+"₡"+ Message +"₡"+ Time+"₡"+Date);
    Debug.Log(Message);
  }
}
