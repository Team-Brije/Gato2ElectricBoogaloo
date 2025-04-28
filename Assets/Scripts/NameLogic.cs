using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NameLogic : MonoBehaviour
{
    public TMP_InputField inputField;
    public static string username;
    ConnectionManager wsManager;
    string[] users;
    string tempUname;
    public Coroutine QOLcor;
    public TextMeshProUGUI ErrorText;

    private void Start()
    {
        username = null;
        wsManager = ConnectionManager.Instance;
        QOLcor = StartCoroutine(QOL());
    }

    private void OnDisable()
    {
        StopCoroutine(QOLcor);
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
        //Null String
        if (string.IsNullOrEmpty(uname) || uname == "No name")
        {
            Debug.Log("EMPTY USERNAME");
            ErrorText.text = "USERNAME CANNOT BE EMPTY";
            return;
        }
        //Username Already Taken
        
        users = wsManager.GetPlayerList();
        foreach (string usernames in users)
        {
            if (usernames == uname)
            {
                Debug.Log("USERNAME ALREADY TAKEN");
                ErrorText.text = "USERNAME ALREADY TAKEN";
                return;
            }
        }
        
        username = uname;
        wsManager.SendWebSocketMessage("1|" + username);
        Debug.Log("USERNAME SAVED SUCCESFULLY");
        SceneManager.LoadScene("Lobby");
    }
    
    public IEnumerator QOL()
    {
        while (true)
        {
            users = wsManager.GetPlayerList();
            yield return new WaitForSeconds(1);
        }
    }
}
