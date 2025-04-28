using System.Collections;
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

    public IEnumerator UpdateLobby()
    {
        yield return new WaitForSeconds(1);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
