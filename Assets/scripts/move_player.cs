using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class move_player : MonoBehaviour
{
    [SerializeField]
    InputAction left = new InputAction(type: InputActionType.Button);
    [SerializeField]
    InputAction right = new InputAction(type: InputActionType.Button);
    [SerializeField]
    InputAction up = new InputAction(type: InputActionType.Button);
    [SerializeField]
    InputAction down = new InputAction(type: InputActionType.Button);

    [SerializeField] int speed;
    [SerializeField] int width;


    public void OnEnable()
    {
        left.Enable();
        right.Enable();
        up.Enable();
        down.Enable();
    }
    public void OnDisable()
    {
        left.Disable();
        right.Disable();
        up.Disable();
        down.Disable();
    }

    void Start()
    {
        transform.position = new Vector3((width/3)*10 +5 , 0,0);
    }

    // Update is called once per frame
    void Update()
    {
        if (left.IsPressed())
        {
            transform.position += new Vector3(Time.deltaTime * speed * -1, 0, 0);
        }
        if (right.IsPressed())
        {
            transform.position += new Vector3(Time.deltaTime * speed, 0, 0);
        }
        if (down.IsPressed())
        {
            transform.position += new Vector3(0,0,Time.deltaTime * speed * -1);
        }
        if (up.IsPressed())
        {
            transform.position += new Vector3(0,0,Time.deltaTime * speed);
        }
    }
}
