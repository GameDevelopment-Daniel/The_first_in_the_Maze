using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class move_script : MonoBehaviour
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

    [SerializeField] int speed;
    Rigidbody rb;

    PhotonView view;


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
    private void Start()
    {
        view = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (view.IsMine) 
        {
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

}
