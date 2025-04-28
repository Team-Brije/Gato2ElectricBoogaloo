using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class LobbyLogic : MonoBehaviour
{
    string username;
    Coroutine updateLobbyCor;
    string[] usersInGame;
    List<string> usersInLobby = new List<string>();
    ConnectionManager wsManager;
    public GameObject ListObject;
    public GameObject ParentObject;

    [Header("Question Variables")]
    public GameObject waiter;
    public GameObject curtain;
    public GameObject question;
    public TextMeshProUGUI usertext;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        wsManager = ConnectionManager.Instance;
        username = NameLogic.username;
        Debug.Log(username);
        usersInGame = wsManager.GetPlayerList();
        updateLobbyCor = StartCoroutine(UpdateLobby());
        usersInGame = null;
        usersInLobby.Clear();
        usersInLobby.Add("No name");
    }

    private void OnEnable()
    {
        LobbyButtonConnection.OnHandshake += Handshake;
        ConnectionManager.OnQuestion += Question;
        ConnectionManager.OnAccept += Accept;
        ConnectionManager.OnReject += Reject;
        LobbyButtonConnection.OnReject += Reject;
    }

    private void OnDisable()
    {
        LobbyButtonConnection.OnHandshake -= Handshake;
        ConnectionManager.OnQuestion -= Question;
        ConnectionManager.OnAccept -= Accept;
        ConnectionManager.OnReject -= Reject;
        LobbyButtonConnection.OnReject -= Reject;
        StopCoroutine(updateLobbyCor);
    }

    public IEnumerator UpdateLobby()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            usersInGame = wsManager.GetPlayerList();
            foreach (string user in usersInGame)
            {
                if (user == "" || user == "No name") { }
                else if (!usersInLobby.Contains(user) && user != username)
                {
                    usersInLobby.Add(user);
                    GameObject objectToSpawn = Instantiate(ListObject, ParentObject.transform);
                    objectToSpawn.GetComponentInChildren<TextMeshProUGUI>().text = user;
                    objectToSpawn.name = user;
                }
            }
            for (int i = 0; i < usersInLobby.Count(); i++) 
            {
                if (!usersInGame.Contains(usersInLobby[i]) && usersInLobby[i] != "No name")
                {
                    GameObject objectToDestroy = GameObject.Find(usersInLobby[i]);
                    Destroy(objectToDestroy);
                    usersInLobby.Remove(usersInLobby[i]);
                }
                //La forma mas optima de resolver este problema sería tener una lista de gameobjects y borrarlo desde ahi, sin embargo ambos funcionan con relativa similitud.
                //Game dies if we remove a player from the list

            }
        }
    }

    public void Handshake()
    {
        curtain.SetActive(true);
        waiter.SetActive(true);
    }

    public void Question(string name)
    {
        curtain.SetActive(true);
        usertext.text = name;
        question.SetActive(true);
    }

    public void Accept()
    {
        Debug.Log("GAME ACCEPTED");
        //SceneManager.LoadScene("uwu");
    }

    public void Reject()
    {
        curtain.SetActive(false);
        waiter.SetActive(false);
        question.SetActive(false);
    }

    //3|NameOfLobbyPlayer|NameOfPlayer
    //4|NameOfPlayer|NameOfLobbyPlayer|1/0
}
