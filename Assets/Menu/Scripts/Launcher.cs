using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using System.Linq;
using UnityEngine.SceneManagement;

public class Launcher : MonoBehaviourPunCallbacks {
  public static Launcher Instance;

  [SerializeField] TMP_InputField playerNameInputField;
  [SerializeField] TMP_Text titleWelcomeText;
  [SerializeField] TMP_InputField roomNameInputField;
  [SerializeField] Transform roomListContent;
  [SerializeField] GameObject roomListItemPrefab;
  [SerializeField] TMP_Text roomNameText;
  [SerializeField] Transform playerListContent;
  [SerializeField] GameObject playerListItemPrefab;
  [SerializeField] GameObject startGameButton;
  [SerializeField] TMP_Text errorText;

  private void Awake() {
    Instance = this;
  }

  private void Start() {
    Debug.Log("Connecting to master...");
    PhotonNetwork.ConnectUsingSettings();
  }
    private void FixedUpdate()
    {
        if (PhotonNetwork.InRoom)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {
                PhotonNetwork.CurrentRoom.IsVisible = false;
            }
            else
            {
                PhotonNetwork.CurrentRoom.IsVisible = true;
            }
        }
    }

    public override void OnConnectedToMaster() {
    Debug.Log("Connected to master!");
    PhotonNetwork.JoinLobby();
    // Automatically load scene for all clients when the host loads a scene
    PhotonNetwork.AutomaticallySyncScene = true;
  }

  public override void OnJoinedLobby() {
    if (PhotonNetwork.NickName == "") {
      PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString(); // Set a default nickname, just as a backup
      MenuManager.Instance.OpenMenu("name");
    } else {
      MenuManager.Instance.OpenMenu("title");
    }
    Debug.Log("Joined lobby");
  }

  public void SetName() {
    string name = playerNameInputField.text;
    if (!string.IsNullOrEmpty(name)) {
      PhotonNetwork.NickName = name;
      titleWelcomeText.text = $"Welcome, {name}!";
      MenuManager.Instance.OpenMenu("title");
      playerNameInputField.text = "";
    } else {
      Debug.Log("No player name entered");
      // TODO: Display an error to the user
    }
  }

  public void CreateRoom() {
    if (!string.IsNullOrEmpty(roomNameInputField.text)) {
    
      PhotonNetwork.CreateRoom(roomNameInputField.text, new RoomOptions() {MaxPlayers = 2, CleanupCacheOnLeave = false, IsVisible = true });
      MenuManager.Instance.OpenMenu("loading");
      roomNameInputField.text = "";
    } else {
      Debug.Log("No room name entered");
      // TODO: Display an error to the user
    }
  }

  public override void OnJoinedRoom() {
    // Called whenever you create or join a room
    MenuManager.Instance.OpenMenu("room");
    roomNameText.text = PhotonNetwork.CurrentRoom.Name;
    Player[] players = PhotonNetwork.PlayerList;
    if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
    {
        Debug.Log("got 2 players in the same room");
        //PhotonNetwork.CurrentRoom.IsVisible = false;
    }
        foreach (Transform trans in playerListContent) {
      Destroy(trans.gameObject);
    }
    for (int i = 0; i < players.Count(); i++) {
      Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
    }
    // Only enable the start button if the player is the host of the room
    startGameButton.SetActive(PhotonNetwork.IsMasterClient);
  }

  public override void OnMasterClientSwitched(Player newMasterClient) {
    startGameButton.SetActive(PhotonNetwork.IsMasterClient);
  }

  public void LeaveRoom() {
        Debug.Log("click on leave room");
        Debug.Log("current room visible is: "+ PhotonNetwork.CurrentRoom.IsVisible);
        
        //PhotonNetwork.CurrentRoom.IsVisible = true;
        
        Debug.Log("current room visible after is: " + PhotonNetwork.CurrentRoom.IsVisible);
        PhotonNetwork.LeaveRoom();

       // MenuManager.Instance.OpenMenu("loading");
        
        PhotonNetwork.Disconnect();
  }

  public void JoinRoom(RoomInfo info) {
    PhotonNetwork.JoinRoom(info.Name);
    MenuManager.Instance.OpenMenu("loading");
  }

  public override void OnLeftRoom() {
        SceneManager.LoadScene("Menu");
        Debug.Log("was on left room");
    }

  public override void OnRoomListUpdate(List<RoomInfo> roomList) {
        
    foreach (Transform trans in roomListContent) {
      Destroy(trans.gameObject);
    }
    for (int i = 0; i < roomList.Count; i++) {
      if (roomList[i].RemovedFromList) {
        // Don't instantiate stale rooms
        continue;
      }
      Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
    }
  }

  public override void OnCreateRoomFailed(short returnCode, string message) {
    errorText.text = "Room Creation Failed: " + message;
    MenuManager.Instance.OpenMenu("error");
  }

  public override void OnPlayerEnteredRoom(Player newPlayer) {
    Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
  }

  public void StartGame() {
        // 1 is used as the build index of the game scene, defined in the build settings
        // Use this instead of scene management so that *everyone* in the lobby goes into this scene

        // Changing the nickname of the master client in PUN
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2) 
        {
            PhotonNetwork.CurrentRoom.IsVisible= false;
            PhotonNetwork.LoadLevel(4);
        }
    }

  public void QuitGame() {
    //Application.Quit();
    
    PhotonNetwork.Disconnect();
    SceneManager.LoadScene("openWindowSence");
    }
}

