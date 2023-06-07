using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;


public class connectToServer_2 : MonoBehaviourPunCallbacks
{
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); //we first connect to the server
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("succesfuly connect to the master server");
        //TypedLobby lobby = new TypedLobby("daniel lobby", LobbyType.Default);
        //PhotonNetwork.JoinLobby(lobby);
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("in OnJoinedLobby: number of Rooms:"+PhotonNetwork.CountOfRooms);
        SceneManager.LoadScene("lobby");
    }

    

}
