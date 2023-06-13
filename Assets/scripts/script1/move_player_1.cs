using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;


public class move_player_1 : MonoBehaviour
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
    [SerializeField] int width;
    int speed;

    Rigidbody rb;

    GameObject fastRed;
    GameObject slowRed;
    GameObject breakRed;
    GameObject freezeRed;
    GameObject reMazeRed;

    bool breakWallNow= false;

    public GameObject miniCamera;
    

    [SerializeField] AudioSource win_sound;
    [SerializeField] AudioSource bip_sound;

    public void OnEnable()
    {
        left.Enable();
        right.Enable();
        up.Enable();
        down.Enable();
        rotateLeft.Enable();
        rotateRight.Enable();
    }
    public void OnDisable()
    {
        left.Disable();
        right.Disable();
        up.Disable();
        down.Disable();
        rotateRight.Disable();
        rotateLeft.Disable();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        transform.position = new Vector3((width / 3) * 10 + 5,transform.position.y,transform.position.z);

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
        Vector3 direction = new Vector3(0,0,0);
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
            transform.Rotate(0, -60*Time.deltaTime, 0, Space.Self);
        }
        if (rotateRight.IsPressed())
        {
            transform.Rotate(0, 60 * Time.deltaTime, 0, Space.Self);
        }
        rb.velocity = transform.TransformDirection(direction) * speed;

        miniCamera.transform.position = new Vector3(miniCamera.transform.position.x, miniCamera.transform.position.y, transform.position.z);
        
    }

    private void OnTriggerEnter(Collider collision)
    {

        if (collision.name == "freeze_ability") //got to the ice cube
        {
            bip_sound.Play();
            Destroy(collision.gameObject);
            freezeRed.GetComponent<Image>().enabled = false; 

        }
        if (collision.name == "slow_ability") //got to the turtle
        {
            bip_sound.Play();
            Destroy(collision.gameObject);
            slowRed.GetComponent<Image>().enabled = false;

        }
        if (collision.name == "lightning_ability") //got to the lightning
        {
            bip_sound.Play();
            Destroy(collision.gameObject);
            fastRed.GetComponent<Image>().enabled = false;

        }
        if (collision.name == "break_ability") //got to the hamer
        {
            bip_sound.Play();
            Destroy(collision.gameObject);
            breakRed.GetComponent<Image>().enabled = false;

        }
        if (collision.name == "reMaze_ability") //got to the maze
        {
            bip_sound.Play();
            Destroy(collision.gameObject);
            reMazeRed.GetComponent<Image>().enabled = false;

        }

        if (collision.name == "finishLine") //the player got to the finish line and won
        {
            Debug.Log(collision.name);
            GameObject.Find("win").GetComponent<Image>().enabled = true;
            //GameObject menu = GameObject.Find("menu");
            //menu.GetComponent<Image>().enabled = true;
            //menu.GetComponent<Button>().enabled = true;
            //GameObject.Find("text").GetComponent<TextMeshProUGUI>().enabled = true;
            win_sound.Play();

            this.gameObject.GetComponent<move_player_1>().enabled = false;
            GameObject.Find("Abilities").GetComponent<active_abilities_1>().enabled = false;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "wall"&& breakWallNow ) // for me: need to made diffrent tag for framw
        {
            Destroy(collision.gameObject);
            breakWallNow= false;
        }
    }
    public void addSpeed(int speedAdd)
    {
        speed += speedAdd;
    }
    public void base_speed()
    {
        speed = baseSpeed;
    }
    public void ResetSpeed()
    {
        speed = 0;
    }
    public void breakWall()
    {
        breakWallNow = true;
    }
}
