using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Photon.Pun;

public class generate_maze_script_1 : MonoBehaviourPun
{
    [SerializeField] GameObject wallHorizontal;
    [SerializeField] GameObject wallVertical;
    //[SerializeField] GameObject floor;
    [SerializeField] int wallLength;

    [SerializeField] int length;
    [SerializeField] int width;
    Vector3 posHorizontal;
    Vector3 posVertical;
    int [,] mat;
    List<Edge> edges = new List<Edge>();
    public GameObject maze;


    private class Edge
    {
        public GameObject wall;
        public int index1;
        public int index2;
        public Edge(GameObject wall, int index1,int index2)
        {
            this.wall = wall;
            this.index1 = index1;
            this.index2 = index2;
        }
        
    }
    public int geti(int index)
    {
        return index / width;
    }
    public int getj(int index)
    {
        return index % width;
    }



    public void Start()
    {
        
        posHorizontal = wallHorizontal.transform.position;
        posVertical = wallVertical.transform.position;

        //give group number
        mat = new int[length, width];
        for(int i=0;i<length;i++)
            for(int j = 0; j < width; j++) 
                mat[i,j] = i*width+j;

        generate_maze();
        Debug.Log("IN START");
    }
    private void generate_maze()
    {
        generate_walls();
        KruskalAlgorithm();
        Debug.Log("IN generate maze");
    }
    public void destroy_maze()
    {
        foreach (Transform child in maze.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }




    private void generate_walls()
    {
        // generate the walls and save them
        for (int row = 0; row < length; row++)
        {
            for (int col = 0; col < width; col++)
            {
                //generate vertical walls
                Vector3 newPosition = new Vector3(posVertical.x + wallLength * col, posVertical.y, posVertical.z + wallLength * row);
                GameObject newWall1 = Instantiate(wallVertical, newPosition, wallVertical.transform.rotation);
                newWall1.transform.SetParent(maze.transform);

                if (col != 0 && col != width - 1) // dont add to edges list edges of the maze frame
                {
                    newWall1.tag = "wall";
                    edges.Add( new Edge( newWall1, (length - 1 - row)*width + (col - 1), (length - 1 - row)*width + col) ); //save the edges
                }
                else
                {
                    newWall1.tag = "frame wall";
                }

                //generate horizontal walls
                if (col == width - 1 || (row ==(length-1) && col== (width-1)/2)) // skip the last col horizontal walls and the exit generation
                    continue;
                Vector3 newPosition2 = new Vector3(posHorizontal.x + wallLength * col, posHorizontal.y, posHorizontal.z + wallLength * row);
                GameObject newWall2=Instantiate(wallHorizontal, newPosition2, wallHorizontal.transform.rotation);
                newWall2.transform.SetParent(maze.transform);

                if (row != length - 1) // dont add to edges list edges of the maze frame
                {
                    newWall2.tag = "wall";
                    edges.Add(new Edge( newWall2, (length - 1 - row)*width + col, (length - 2 - row)*width +col));  //save the edges
                }
                else
                {
                    newWall2.tag = "frame wall";
                }

            }
        }
        ////create the ground 
        //GameObject newFloor=Instantiate(floor, new Vector3((width-1)*wallLength/2 + 0.5f, -wallLength/2, (length-1)*wallLength /2 +0.5f), transform.rotation);
        //newFloor.transform.localScale=new Vector3(newFloor.transform.localScale.x*width, newFloor.transform.localScale.y, newFloor.transform.localScale.z*length);

        //add the first linw of walls
        int firstEntry = width / 3;      // position for player 1 entry
        int secondEntry = width - firstEntry; // position for player 2 entry
        for (int i = 0; i < width-1; i++)
        {
            if (i == firstEntry || i == secondEntry) continue;
            Vector3 newPosition2 = new Vector3(posHorizontal.x + wallLength * i, posHorizontal.y,-wallLength/2);
            GameObject newWall = Instantiate(wallHorizontal, newPosition2, wallHorizontal.transform.rotation);
            newWall.tag = "frame wall";
            newWall.transform.SetParent(maze.transform);

        }

    }
     private void KruskalAlgorithm()
    {
        while (edges.Count > 0)
        {
            //take random edge evrey time
            int randomIndex = Random.Range(0, edges.Count);
            Edge randomEdge = edges[randomIndex];

            //if in the same group continue to next edge, else union the groups
            int group1 = find(randomEdge.index1);
            int group2 = find(randomEdge.index2);
            if (group1!= group2)
            {
                Union(randomEdge.index1, randomEdge.index2);
                Destroy(randomEdge.wall); // remove the wall
            }
            edges.RemoveAt(randomIndex); // remove the edge from edges list
        }
    }
    int find(int index)
    {
        List<int> myList = new List<int>();

        int saveIndex = index;

        while (mat[geti(index),getj(index)] != index)
        {
            myList.Add(index); //save all the points, for update the values later
            index = mat[geti(index), getj(index)];
        }

        for(int i = 0; i < myList.Count; i++)
        {
            int temp = myList[i];
            mat[geti(temp), getj(temp)] = index; //update the group number
        }

        return index;
    }

    //set group of point 2 to group of point 1
    void Union(int index1 , int index2)
    {
        int index1Group = find(index1);
        int tempIndex;

        while(mat[geti(index2), getj(index2)] != index2)
        {
            tempIndex = mat[geti(index2), getj(index2)];
            mat[geti(index2), getj(index2)] = index1Group;
            index2= tempIndex;
        }
        mat[geti(index2), getj(index2)] = index1Group;
    }

}
