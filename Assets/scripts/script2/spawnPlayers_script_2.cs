using UnityEngine;
using Photon.Pun;

public class spawnPlayers_script_2 : MonoBehaviour
{
    public GameObject playerPrefab1;
    public GameObject playerPrefab2;

    private bool player1Spawned = false;
    private bool player2Spawned = false;

    public static InputManager Instance;
    private PlayerControls playerControls;

    
    void Awake()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                // Spawn Player 1 if it hasn't been spawned yet
                if (!player1Spawned)
                {
                    PhotonNetwork.Instantiate(playerPrefab1.name, playerPrefab1.transform.position, playerPrefab1.transform.rotation, 0);
                    player1Spawned = true;
                }
            }
            else
            {
                // Spawn Player 2 if it hasn't been spawned yet
                if (!player2Spawned)
                {
                    PhotonNetwork.Instantiate(playerPrefab2.name, playerPrefab2.transform.position, playerPrefab2.transform.rotation, 0);
                    player2Spawned = true;
                }
            }
        }
    }
}
