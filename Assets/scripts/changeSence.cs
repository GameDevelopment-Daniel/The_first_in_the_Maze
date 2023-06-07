using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class changeSence : MonoBehaviour
{
    public void Game1player()
    {
        Debug.Log("whattt");
        SceneManager.LoadScene("Maze1");
    }
    
    public void Abilities()
    {
        SceneManager.LoadScene("Abilities");
    }
    public void Game2player()
    { 
       SceneManager.LoadScene("Menu");
    }
    public void openWindowSence()
    {
        SceneManager.LoadScene("openWindowSence");
    }
    public void openWindowSence_disconnect()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("openWindowSence");
    }

}
