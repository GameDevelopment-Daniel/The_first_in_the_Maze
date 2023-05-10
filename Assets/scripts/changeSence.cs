using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class changeSence : MonoBehaviour
{
    public void Game1player()
    {
        Debug.Log("whattt");
        SceneManager.LoadScene("MazeScene");
    }
    
    public void Abilities()
    {
        SceneManager.LoadScene("Abilities");
    }
    public void Game2player()
    {
       // SceneManager.LoadScene("MazeScene");
    }
}
