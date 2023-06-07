using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

public class randomAbilities : MonoBehaviour
{
    [SerializeField]
    int limit_left_x;
    [SerializeField]
    int limit_right_x;
    [SerializeField]
    int limit_bottom_z;
    [SerializeField]
    int limit_top_z;
    [SerializeField]
    int block_size;
    // Start is called before the first frame update
    void Start()
    {
        List<GameObject[]> allArrays = new List<GameObject[]>();
        allArrays.Add(GameObject.FindObjectsOfType<GameObject>().Where(obj => obj.name == "lightning_ability").ToArray());
        allArrays.Add(GameObject.FindObjectsOfType<GameObject>().Where(obj => obj.name == "break_ability").ToArray());
        allArrays.Add(GameObject.FindObjectsOfType<GameObject>().Where(obj => obj.name == "freeze_ability").ToArray());
        allArrays.Add(GameObject.FindObjectsOfType<GameObject>().Where(obj => obj.name == "slow_ability").ToArray());
        allArrays.Add(GameObject.FindObjectsOfType<GameObject>().Where(obj => obj.name == "reMaze_ability").ToArray());


        int ix = (limit_right_x - limit_left_x) / block_size;
        int iz = (limit_top_z -limit_bottom_z) / block_size;

        for (int i=0;i<allArrays.Count;i++)
        {
            foreach (GameObject obj in allArrays[i]) 
            {
                float tempy = obj.transform.position.y;
                obj.transform.position = new Vector3(Random.Range(0, ix + 1) * block_size + limit_left_x,
                                                     tempy,
                                                     Random.Range(0, iz + 1) * block_size + limit_bottom_z);
            }
        }
        
    }
}
