using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class active_abilities_1 : MonoBehaviour
{
    [SerializeField]
    InputAction one = new InputAction(type: InputActionType.Button);
    [SerializeField]
    InputAction two = new InputAction(type: InputActionType.Button);
    [SerializeField]
    InputAction three = new InputAction(type: InputActionType.Button);
    [SerializeField]
    InputAction four = new InputAction(type: InputActionType.Button);
    [SerializeField]
    InputAction five = new InputAction(type: InputActionType.Button);

    GameObject player;
    GameObject Maze;

    GameObject fastRed;
    GameObject slowRed;
    GameObject breakRed;
    GameObject freezeRed;
    GameObject reMazeRed;


    int speedBoost = 10;
    float timeBoost = 10.0f;


    public void OnEnable()
    {
        one.Enable();
        two.Enable();
        three.Enable();
        four.Enable();
        five.Enable();
    }
    public void OnDisable()
    {
        one.Disable();
        two.Disable();
        three.Disable();
        four.Disable();
        five.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player");
        Maze = GameObject.Find("Maze");

        fastRed = GameObject.Find("fastRed");
        breakRed = GameObject.Find("breakRed");
        freezeRed = GameObject.Find("freezeRed");
        slowRed = GameObject.Find("slowRed");
        reMazeRed = GameObject.Find("reMazeRed");

    }

    // Update is called once per frame
    void Update()
    {
        //for speed
        if (one.WasPressedThisFrame() && fastRed.GetComponent<Image>().enabled==false)
        {
            fastRed.GetComponent<Image>().enabled=true; // set the ability to red
            player.GetComponent<move_player_1>().addSpeed(speedBoost);
            StartCoroutine(ResetSpeedAfterDelay(timeBoost,false));
        }
        //for slow
        if (two.WasPressedThisFrame() && slowRed.GetComponent<Image>().enabled == false) // for me: dont make more then 2 slow in the game
        {
            slowRed.GetComponent<Image>().enabled = true;
            //player.GetComponent<move_player>().addSpeed(speedBoost*-1); // need to slow the other player
            //StartCoroutine(ResetSpeedAfterDelay(timeBoost,true));
        }
        //for freeze
        if (three.WasPressedThisFrame() && freezeRed.GetComponent<Image>().enabled == false) // for me: dont make more then 2 slow in the game
        {
            freezeRed.GetComponent<Image>().enabled = true;
            //player.GetComponent<move_player_2>().ResetSpeed();     // need to freeze the other player
            //StartCoroutine(ResetSpeedFromFreeze(timeBoost / 2));
        }
        //for break wall
        if (four.WasPressedThisFrame() && breakRed.GetComponent<Image>().enabled == false) // for me: dont make more then 2 slow in the game
        {
            breakRed.GetComponent<Image>().enabled = true;
            player.GetComponent<move_player_1>().breakWall(); 
        }
        //for reMaze
        if (five.WasPressedThisFrame() && reMazeRed.GetComponent<Image>().enabled == false) // for me: dont make more then 2 slow in the game
        {
            reMazeRed.GetComponent<Image>().enabled = true;
            GameObject.Find("minimapRaw").GetComponent<RawImage>().enabled = true;
           // Maze.GetComponent<generate_maze_script_1>().destroy_maze();
           // Maze.GetComponent<generate_maze_script_1>().Start();
        }
    }
    IEnumerator ResetSpeedAfterDelay(float delay,bool add)
    {
        yield return new WaitForSeconds(delay);

        if(!add)
        {
            player.GetComponent<move_player_1>().addSpeed(speedBoost * -1);//return the speed to normal
        }
        else
        {
            player.GetComponent<move_player_1>().addSpeed(speedBoost);//return the speed to normal
        }

    }
    IEnumerator ResetSpeedFromFreeze(float delay)
    {
        yield return new WaitForSeconds(delay);
        player.GetComponent<move_player_1>().base_speed();
    }

}
