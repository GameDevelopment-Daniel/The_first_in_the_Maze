using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class enterToMaze : MonoBehaviourPun
{
    int countPlyersReady = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(countPlyersReady >= 2)
        {
            // Close and hide the room
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            //load the maze
            PhotonNetwork.LoadLevel("Maze2");
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        countPlyersReady++;
        Debug.Log("on trigger enter");
    }

    private void OnCollisionExit(Collision collision)
    {
        countPlyersReady--;
        Debug.Log("on trigger Exit");
    }
}
