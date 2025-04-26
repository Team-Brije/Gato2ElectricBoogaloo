using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class NameLogic : MonoBehaviour
{
    public TMP_InputField inputField;
    public static string username;
    ConnectionManager wsManager;
    string[] users;
    string tempUname;
    private void Start()
    {
        username = null;
        wsManager = ConnectionManager.Instance;
    }

    public void CheckValue()
    {
        string input = inputField.text;
        ValidateUsername(input);
    }

    #region PRIVATE_DO_NOT_OPEN
    //LUIS MIGUEL SALVAME DE USAR WEBSOCKETS 
    #endregion  

    public void ValidateUsername(string uname)
    {
        tempUname = uname;
        //Null String
        if (string.IsNullOrEmpty(uname))
        {
            Debug.Log("EMPTY USERNAME");
            return;
        }
        //Username Already Taken
        string[] users = wsManager.GetPlayerList();

        foreach (string usernames in users)
        {
            if (usernames == tempUname)
            {
                Debug.Log("USERNAME ALREADY TAKEN");
                return;
            }
        }
        username = tempUname;
        wsManager.SendMessage("1|" + username);
        Debug.Log("USERNAME SAVED SUCCESFULLY");
        SceneManager.LoadScene("Lobby");
    }
}
