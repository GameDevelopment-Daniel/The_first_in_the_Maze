using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraController : MonoBehaviourPun
{
    public GameObject camera;
    PhotonView view;

    private float offset;

    // Start is called before the first frame update
    private void Awake()
    {
        view = GetComponent<PhotonView>();

    }
    void Start()
    {
        if (!view.IsMine) { return; }
        if (PhotonNetwork.IsMasterClient)
        {
            camera = GameObject.Find("minimapCamera");
            Debug.Log("mini111111111111");
        }
        else
        {
            camera = GameObject.Find("minimapCamera2");
            Debug.Log("mini2222222222222222");

        }
        offset = camera.transform.position.z - transform.position.z ;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!view.IsMine) { return; }
        if (!PhotonNetwork.IsMasterClient) { return; }
            camera.transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y, transform.position.z + offset);

            Debug.Log("mini111111111111");
            Debug.Log(camera.transform.position);


            //camera.transform.position = new Vector3 (camera.transform.position.x, camera.transform.position.y , transform.position.z + offset);
        
    }
}
