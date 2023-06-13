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

    GameObject fastRed;
    GameObject slowRed;
    GameObject breakRed;
    GameObject freezeRed;
    GameObject reMazeRed;

    bool breakWallNow= false;
    private int freeze_value = 1;

    GameObject miniCamera1;
    GameObject miniCamera2;

    [SerializeField] AudioSource loss_sound;
    [SerializeField] AudioSource win_sound;
    [SerializeField] AudioSource bip_sound;

    bool use_left_text=false;


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
        rb = GetComponent<Rigidbody>();
        miniCamera1 = GameObject.Find("minimapCamera");
        miniCamera2 = GameObject.Find("minimapCamera2");

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

            if(speed <= 0)
            {
                rb.velocity = transform.TransformDirection(direction) * freeze_value; // if he got to slow, speed is <=0 so use speed = 1
            }
            else 
            {
                rb.velocity = transform.TransformDirection(direction) * speed * freeze_value; // freeze value set to 0 when the player got freeze, else 1
            }

            // controll the minimap camera
            if (PhotonNetwork.IsMasterClient) 
            {
                miniCamera1.transform.position = new Vector3(miniCamera1.transform.position.x, miniCamera1.transform.position.y, transform.position.z);
            }
            else
            {
                miniCamera2.transform.position = new Vector3(miniCamera2.transform.position.x, miniCamera2.transform.position.y, transform.position.z);
            }

            //check if player leave the game
            if(PhotonNetwork.CurrentRoom.PlayerCount < 2 && !use_left_text) {StartCoroutine(On_player_leave());}
            
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
                bip_sound.Play();
            }

            if (collision.name == "slow_ability") //got to the turtle
            {
                view.RPC("ActivateDestroy", RpcTarget.All, collision.gameObject.GetComponent<PhotonView>().ViewID);
                slowRed.GetComponent<Image>().enabled = false;
                bip_sound.Play();
            }

            if (collision.name == "lightning_ability") //got to the lightning
            {
                view.RPC("ActivateDestroy", RpcTarget.All, collision.gameObject.GetComponent<PhotonView>().ViewID);
                fastRed.GetComponent<Image>().enabled = false;
                bip_sound.Play();
            }

            if (collision.name == "break_ability") //got to the hamer
            {
                view.RPC("ActivateDestroy", RpcTarget.All, collision.gameObject.GetComponent<PhotonView>().ViewID);
                breakRed.GetComponent<Image>().enabled = false;
                bip_sound.Play();
            }

            if (collision.name == "reMaze_ability") //got to the maze
            {
                Debug.Log("why");
                view.RPC("ActivateDestroy", RpcTarget.All, collision.gameObject.GetComponent<PhotonView>().ViewID);
                reMazeRed.GetComponent<Image>().enabled = false;
                bip_sound.Play();
            }

            if (collision.name == "finishLine") //the player got to the finish line and won
            {
                Debug.Log(collision.name);
                GameObject.Find("win").GetComponent<Image>().enabled = true;
                GameObject menu = GameObject.Find("menu");
                menu.GetComponent<Image>().enabled = true;
                menu.GetComponent<Button>().enabled = true;
                GameObject.Find("text").GetComponent<TextMeshProUGUI>().enabled = true;
                win_sound.Play();

                GameObject.Find("left_text").SetActive(false); // dont show left text when player left, the game already end/

                PhotonView photonView1 = GameObject.Find("player2(Clone)").GetComponent<PhotonView>();
                PhotonView photonView2 = GameObject.Find("player1(Clone)").GetComponent<PhotonView>();


                photonView1.RPC("loseGame", RpcTarget.Others);
                photonView2.RPC("loseGame", RpcTarget.Others);

                photonView1.RPC("disablePlayers", RpcTarget.All);
                photonView2.RPC("disablePlayers", RpcTarget.All);
            }

        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (view.IsMine)
        {
            if ((collision.gameObject.name == "wallHorizontal_2(Clone)" || collision.gameObject.name == "wallVertical_2(Clone)") && breakWallNow)
            {
                collision.gameObject.SetActive(false);
                view.RPC("ActivateDestroy", RpcTarget.All, collision.gameObject.GetComponent<PhotonView>().ViewID);
                breakWallNow = false;
            }
        }
    }
    [PunRPC]
    private void ActivateDestroy(int objectViewId)
    {
        //Debug.Log("ActivateDestroy:before=?");
        if (!PhotonNetwork.IsMasterClient) { return; }
        //Debug.Log("ActivateDestroy:after=master");
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
        speed += speedAdd;
    }
    [PunRPC]

    public void base_speed()
    {
        freeze_value = 1;
    }
    [PunRPC]

    public void ResetSpeed()
    {
        freeze_value = 0;
    }
    public void breakWall()
    {
        breakWallNow = true;
        Debug.Log("was in break wall: break wall value: " + breakWallNow);
    }
    [PunRPC]
    private void loseGame()
    {
        GameObject.Find("lose").GetComponent<Image>().enabled = true;
        GameObject menu2 = GameObject.Find("menu");
        menu2.GetComponent<Image>().enabled = true;
        menu2.GetComponent<Button>().enabled = true;
        GameObject.Find("text").GetComponent<TextMeshProUGUI>().enabled = true;
        loss_sound.Play();

        GameObject.Find("left_text").SetActive(false);


    }

    [PunRPC]
    private void disablePlayers()
    {
        if (!view.IsMine) { return;}
        this.gameObject.GetComponent<move_player_2>().enabled = false;
        this.gameObject.GetComponent<active_abilities_2>().enabled = false;
    }
    IEnumerator On_player_leave()
    {
        use_left_text = true;
        GameObject menu = GameObject.Find("menu");
        menu.GetComponent<Image>().enabled = true;
        menu.GetComponent<Button>().enabled = true;
        GameObject.Find("text").GetComponent<TextMeshProUGUI>().enabled = true;
        GameObject.Find("left_text").GetComponent<TextMeshProUGUI>().enabled = true; 

        yield return new WaitForSeconds(7);

        GameObject.Find("left_text").GetComponent<TextMeshProUGUI>().enabled = false;

    }
}
