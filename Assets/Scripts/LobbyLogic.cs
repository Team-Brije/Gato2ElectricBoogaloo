using UnityEngine;

public class LobbyLogic : MonoBehaviour
{
    string username;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        username = NameLogic.username;
        Debug.Log(username);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
