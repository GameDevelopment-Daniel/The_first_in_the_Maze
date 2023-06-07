using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class my_minicamera2 : MonoBehaviourPun
{
    // Start is called before t he first frame update
    void Start()
    {
        if (PhotonNetwork.IsMasterClient) { return; }

        GameObject.Find("minimapRaw2").GetComponent<RawImage>().enabled = true;
        PhotonView view= GameObject.Find("player2(Clone)").GetComponent<PhotonView>();
        view.RPC("minimapActive",RpcTarget.Others);

    }
}
