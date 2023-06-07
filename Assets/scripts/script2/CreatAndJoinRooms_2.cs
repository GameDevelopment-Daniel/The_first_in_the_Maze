using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using Photon.Realtime;


public class CreatAndJoinRooms_2 : MonoBehaviourPunCallbacks
{
    public TMP_InputField createInput;
    public TMP_InputField JoinInput;
    byte maxPlayers = 2;

    public void creatRoom()
    {
        PhotonNetwork.CreateRoom(createInput.text);
    }
    public void joinRoom()
    {
        PhotonNetwork.JoinRoom(JoinInput.text);
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("in on joined room");
        Debug.Log("in  OnJoinedRoom: number of plyers in current room:" + PhotonNetwork.CurrentRoom.PlayerCount);
        Debug.Log("in  OnJoinedRoom: number of Rooms:" + PhotonNetwork.CountOfRooms);

        PhotonNetwork.LoadLevel("waitScene");
        if (PhotonNetwork.CurrentRoom.PlayerCount == maxPlayers) 
        {
            // Hide the room
           // PhotonNetwork.CurrentRoom.IsVisible = false;
        }
        //when want to load multiplayer scene we need this function.
    }
    // override void OnJoinRandomFailed(short returnCode, string message)
    //{
      //  Debug.Log("in on random failed");
        //PhotonNetwork.CreateRoom(null,new RoomOptions { MaxPlayers = maxPlayers },null,null);
    //}
}


