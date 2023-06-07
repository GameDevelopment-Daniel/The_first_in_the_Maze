using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class myCamera : MonoBehaviourPun
{
    // Start is called before t he first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
            gameObject.GetComponent<Camera>().enabled = true;
            gameObject.GetComponent<AudioListener>().enabled = true;
        }
    }
}
