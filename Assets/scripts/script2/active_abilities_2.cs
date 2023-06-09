using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class active_abilities_2 : MonoBehaviourPun
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

    GameObject Maze;
    PhotonView view;

    GameObject fastRed;
    GameObject slowRed;
    GameObject breakRed;
    GameObject freezeRed;
    GameObject reMazeRed;

    move_player_2 move_script1;
    move_player_2 move_script2;

    int speedBoost = 8;
    int slowMinus = 6;
    float timeBoost = 10.0f;

    int slow_counter = 0;
    int freeze_counter = 0;

    private void Awake()
    {
        view = GetComponent<PhotonView>();

    }

    public void OnEnable()
    {

        if (view.IsMine)
        {
            Debug.Log("in on enable");
            one.Enable();
            two.Enable();
            three.Enable();
            four.Enable();
            five.Enable();
        }
    }
    public void OnDisable()
    {

       if(view.IsMine)
        {
            one.Disable();
            two.Disable();
            three.Disable();
            four.Disable();
            five.Disable();
        }
    }

    // Start is called before the first frame update
    void Start()
    {

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
        if (view.IsMine)
        {
            
            if (one.WasPressedThisFrame())
            {
                Debug.Log("one was pressed");
            }

            if (one.WasPressedThisFrame() && fastRed.GetComponent<Image>().enabled==false)
            {
                Debug.Log("update1");
                fastRed.GetComponent<Image>().enabled = true; // set the ability to red
                move_script1 = GameObject.Find("player1(Clone)").GetComponent<move_player_2>();
                move_script2 = GameObject.Find("player2(Clone)").GetComponent<move_player_2>();
                move_script1.addSpeed(speedBoost);
                move_script2.addSpeed(speedBoost);

                StartCoroutine(ResetSpeedAfterDelay(timeBoost, false));
            }
            //for slow
            if (two.WasPressedThisFrame() && slowRed.GetComponent<Image>().enabled == false) // for me: dont make more then 2 slow in the game
            {
                Debug.Log("update2");

                slowRed.GetComponent<Image>().enabled = true;
                StartCoroutine(slow(timeBoost));
            }
            //for freeze
            if (three.WasPressedThisFrame() && freezeRed.GetComponent<Image>().enabled == false) 
            {
                Debug.Log("update3");

                freezeRed.GetComponent<Image>().enabled = true;
                StartCoroutine(freeze(timeBoost/2));
            }
            //for break wall
            if (four.WasPressedThisFrame() && breakRed.GetComponent<Image>().enabled == false)
            {
                Debug.Log("update4");

                breakRed.GetComponent<Image>().enabled = true;
                move_script1 = GameObject.Find("player1(Clone)").GetComponent<move_player_2>();
                move_script2 = GameObject.Find("player2(Clone)").GetComponent<move_player_2>();

                move_script1.breakWall();
                move_script2.breakWall();
            }
            //for reMaze
            if (five.WasPressedThisFrame() && reMazeRed.GetComponent<Image>().enabled == false) // for me: dont make more then 2 slow in the game
            {
                Debug.Log("update5");

                reMazeRed.GetComponent<Image>().enabled = true;

                if(PhotonNetwork.IsMasterClient)
                {
                    GameObject.Find("minimapRaw").GetComponent<RawImage>().enabled = true;
                }
                else
                {
                    GameObject.Find("minimapRaw2").GetComponent<RawImage>().enabled = true;
                }
                //PhotonView photonview = GameObject.Find("Maze").GetPhotonView();
                //photonview.RPC("destroy_maze",RpcTarget.All);
                //photonview.RPC("startTemp", RpcTarget.All);

                //Maze.GetComponent<generate_maze_script_2>().Start();
            }
        }
    }
    IEnumerator ResetSpeedAfterDelay(float delay,bool add)
    {
        yield return new WaitForSeconds(delay);

        move_script1 = GameObject.Find("player1(Clone)").GetComponent<move_player_2>();
        move_script2 = GameObject.Find("player2(Clone)").GetComponent<move_player_2>();

        if (!add)
        {
            move_script1.addSpeed(speedBoost * -1); //return the speed to normal
            move_script2.addSpeed(speedBoost * -1); //return the speed to normal

        }
        else
        {
            move_script1.addSpeed(speedBoost); //return the speed to normal
            move_script2.addSpeed(speedBoost); //return the speed to normal

        }

    }
    IEnumerator ResetSpeedFromFreeze(float delay)
    {
        yield return new WaitForSeconds(delay);

        move_script1 = GameObject.Find("player1(Clone)").GetComponent<move_player_2>();
        move_script2 = GameObject.Find("player2(Clone)").GetComponent<move_player_2>();

        move_script1.base_speed(); 
        move_script2.base_speed();

    }

    IEnumerator slow(float delay)
    {
        slow_counter++; //if more then 1 slow active so we need to know for when to disable the text

        PhotonView photonView1 = GameObject.Find("player2(Clone)").GetComponent<PhotonView>();
        PhotonView photonView2 = GameObject.Find("player1(Clone)").GetComponent<PhotonView>();

        photonView1.RPC("addSpeed", RpcTarget.Others, slowMinus * -1);
        photonView2.RPC("addSpeed", RpcTarget.Others, slowMinus * -1);

        photonView1.RPC("slow_text", RpcTarget.Others, true); // show the enemy he is slowed
        photonView2.RPC("slow_text", RpcTarget.Others, true);


        yield return new WaitForSeconds(delay); // after this the slow time end

        slow_counter--;

        photonView1.RPC("addSpeed", RpcTarget.Others, slowMinus);
        photonView2.RPC("addSpeed", RpcTarget.Others, slowMinus);

        if (slow_counter == 0) //all the slows was over
        {
            photonView1.RPC("slow_text", RpcTarget.Others, false);
            photonView2.RPC("slow_text", RpcTarget.Others, false);
        }


    }
    IEnumerator freeze(float delay)
    {
        freeze_counter++;

        PhotonView photonView1 = GameObject.Find("player2(Clone)").GetComponent<PhotonView>();
        PhotonView photonView2 = GameObject.Find("player1(Clone)").GetComponent<PhotonView>();

        photonView1.RPC("ResetSpeed", RpcTarget.Others);
        photonView2.RPC("ResetSpeed", RpcTarget.Others);

        photonView1.RPC("freeze_text", RpcTarget.Others, true); // show the enemy he is freeze
        photonView2.RPC("freeze_text", RpcTarget.Others, true);
        //GetComponent<PhotonView>().RPC("slowRPC", RpcTarget.Others,false);

        yield return new WaitForSeconds(delay); // after this the freeze end

        freeze_counter--;

        

        if (freeze_counter == 0)
        {
            photonView1.RPC("base_speed", RpcTarget.Others);
            photonView2.RPC("base_speed", RpcTarget.Others);
            photonView1.RPC("freeze_text", RpcTarget.Others, false);
            photonView2.RPC("freeze_text", RpcTarget.Others, false);
        }

    }
    [PunRPC]
    public void slow_text(bool active) {
        GameObject slow_text = GameObject.Find("slow_text");
        slow_text.GetComponent<TextMeshProUGUI>().enabled = active;

    }
    [PunRPC]
    public void freeze_text(bool active)
    {
        GameObject freeze_text = GameObject.Find("freeze_text");
        freeze_text.GetComponent<TextMeshProUGUI>().enabled = active;

    }


}
