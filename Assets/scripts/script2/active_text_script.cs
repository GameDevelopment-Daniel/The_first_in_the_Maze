using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class active_text_script : MonoBehaviour
{
    public GameObject roomMenu;
    
    void Update()
    {
        if (roomMenu.active)
        {
            this.GetComponent<TextMeshProUGUI>().enabled = true;
        }
        else
        {
            this.GetComponent<TextMeshProUGUI>().enabled = false;
        }
    }
}
