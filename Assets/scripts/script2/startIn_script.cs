using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class startIn_script : MonoBehaviour
{
    PhotonView view;
    int sec = 15;
    // Start is called before the first frame update
    private void Start()
    {
        view = GetComponent<PhotonView>();
       
        this.gameObject.GetComponent<move_player_2>().enabled = false;
        this.gameObject.GetComponent<active_abilities_2>().enabled = false;
        StartCoroutine(sec_to_start(sec));
    }
    IEnumerator sec_to_start(int sec)
    {
        GameObject sec_object = GameObject.Find("sec");
        for (int i = sec; i >= 1; i--)
        {
            if (view.IsMine)
            {
                sec_object.GetComponent<TextMeshProUGUI>().text = i.ToString();
            }
            yield return new WaitForSeconds(1);
        }
        this.gameObject.GetComponent<move_player_2>().enabled = true;
        this.gameObject.GetComponent<active_abilities_2>().enabled = true;
       
        GameObject.Find("start in").SetActive(false);
        
    }
}
