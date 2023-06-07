using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class move_player_2 : MonoBehaviourPun
{
    [SerializeField]
    InputAction left = new InputAction(type: InputActionType.Button);
    [SerializeField]
    InputAction right = new InputAction(type: InputActionType.Button);
    [SerializeField]
    InputAction up = new InputAction(type: InputActionType.Button);
    [SerializeField]
    InputAction down = new InputAction(type: InputActionType.Button);
    [SerializeField]
    InputAction rotateLeft = new InputAction(type: InputActionType.Button);
    [SerializeField]
    InputAction rotateRight = new InputAction(type: InputActionType.Button);

    [SerializeField]  int baseSpeed;
    private int speed;

    Rigidbody rb;
    PhotonView view;
    Transform minimapPos;
    int botLimit_minimap = 80;
    int topLimit_minimap = 310;

    GameObject fastRed;
    GameObject slowRed;
    GameObject breakRed;
    GameObject freezeRed;
    GameObject reMazeRed;

    bool breakWallNow= false;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
    }
    public void OnEnable()
    {
        if (view.IsMine)
        {
            left.Enable();
            right.Enable();
            up.Enable();
            down.Enable();
            rotateLeft.Enable();
            rotateRight.Enable();
        }
    }
    public void OnDisable()
    {
        if (view.IsMine)
        {
            left.Disable();
            right.Disable();
            up.Disable();
            down.Disable();
            rotateRight.Disable();
            rotateLeft.Disable();
        }
    }

    void Start()
    {
        view = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();


        speed = baseSpeed;

        fastRed=GameObject.Find("fastRed");
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
            //Debug.Log("speed:" + speed);

            Vector3 direction = new Vector3(0, 0, 0);
            if (left.IsPressed())
            {
                direction.x -= 1;
                // Set the velocity of the rigidbody to move the object in the new forward direction
            }
            if (right.IsPressed())
            {
                direction.x += 1;
            }
            if (down.IsPressed())
            {
                direction.z -= 1;
            }
            if (up.IsPressed())
            {
                direction.z += 1;
            }
            if (rotateLeft.IsPressed())
            {
                transform.Rotate(0, -60 * Time.deltaTime, 0, Space.Self);
            }
            if (rotateRight.IsPressed())
            {
                transform.Rotate(0, 60 * Time.deltaTime, 0, Space.Self);
            }
            rb.velocity = transform.TransformDirection(direction) * speed;

        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (view.IsMine)
        {
            if (collision.name == "freeze_ability") //got to the ice cube
            {
                view.RPC("ActivateDestroy", RpcTarget.All, collision.gameObject.GetComponent<PhotonView>().ViewID);
                freezeRed.GetComponent<Image>().enabled = false;

            }
            if (collision.name == "slow_ability") //got to the turtle
            {
                view.RPC("ActivateDestroy", RpcTarget.All, collision.gameObject.GetComponent<PhotonView>().ViewID);
                slowRed.GetComponent<Image>().enabled = false;

            }
            if (collision.name == "lightning_ability") //got to the lightning
            {
                view.RPC("ActivateDestroy", RpcTarget.All, collision.gameObject.GetComponent<PhotonView>().ViewID);
                fastRed.GetComponent<Image>().enabled = false;

            }
            if (collision.name == "break_ability") //got to the hamer
            {
                view.RPC("ActivateDestroy", RpcTarget.All, collision.gameObject.GetComponent<PhotonView>().ViewID);
                breakRed.GetComponent<Image>().enabled = false;

            }
            if (collision.name == "reMaze_ability") //got to the maze
            {
                Debug.Log("why");
                view.RPC("ActivateDestroy", RpcTarget.All, collision.gameObject.GetComponent<PhotonView>().ViewID);
                reMazeRed.GetComponent<Image>().enabled = false;

            }
            if (collision.name == "finishLine") //the player got to the finish line and won
            {
                Debug.Log(collision.name);
                GameObject.Find("win").GetComponent<Image>().enabled = true;
                GameObject menu = GameObject.Find("menu");
                menu.GetComponent<Image>().enabled = true;
                menu.GetComponent<Button>().enabled = true;
                GameObject.Find("text").GetComponent<TextMeshProUGUI>().enabled = true;

                this.gameObject.GetComponent<move_player_2>().enabled = false;
                this.gameObject.GetComponent<active_abilities_2>().enabled = false;
                view.RPC("loseGame", RpcTarget.Others);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (view.IsMine)
        {
            if ((collision.gameObject.name == "wallHorizontal_2(Clone)" || collision.gameObject.name == "wallVertical_2(Clone)") && breakWallNow)
            {
                //collision.transform.position = new Vector3(1000, 1000, 95);
                //Debug.Log(collision.transform.position);
                collision.gameObject.SetActive(false);
                view.RPC("ActivateDestroy", RpcTarget.All, collision.gameObject.GetComponent<PhotonView>().ViewID);
                breakWallNow = false;
            }
        }
    }
    [PunRPC]
    private void ActivateDestroy(int objectViewId)
    {
        Debug.Log("ActivateDestroy:before=?");
        if (!PhotonNetwork.IsMasterClient) { return; }
        Debug.Log("ActivateDestroy:after=master");
        PhotonView photonView = PhotonView.Find(objectViewId);
        if (photonView != null)
        {
            PhotonNetwork.Destroy(photonView);
        }
        else
        {
            Debug.LogError("Failed to find PhotonView with ViewID: " + objectViewId);
        }
    }


    [PunRPC]
    public void addSpeed(int speedAdd)
    {
        if(!view.IsMine) { return; }
        Debug.Log("in add speed");
        speed += speedAdd;
        Debug.Log("addspeed :" + speedAdd + " , speed now: " + speed);
    }
    [PunRPC]

    public void base_speed()
    {
        speed = baseSpeed;
    }
    [PunRPC]

    public void ResetSpeed()
    {
        speed = 0;
    }
    public void breakWall()
    {
        breakWallNow = true;
        Debug.Log("was in break wall: break wall value: " + breakWallNow);
    }

    [PunRPC]
    public void LowerSpeed()
    {
        // Lower the player's speed by 5
        speed -= 7;
    }
    [PunRPC]
    private void loseGame()
    {
        GameObject.Find("lose").GetComponent<Image>().enabled = true;
        GameObject menu2 = GameObject.Find("menu");
        menu2.GetComponent<Image>().enabled = true;
        menu2.GetComponent<Button>().enabled = true;
        GameObject.Find("text").GetComponent<TextMeshProUGUI>().enabled = true;

        this.gameObject.GetComponent<move_player_2>().enabled = false;
        this.gameObject.GetComponent<active_abilities_2>().enabled = false;
    }


}
