using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class generate_maze_script : MonoBehaviour
{
    [SerializeField] GameObject wallHorizontal;
    [SerializeField] GameObject wallVertical;
    [SerializeField] GameObject floor;

    [SerializeField] int length;
    [SerializeField] int width;
    Vector3 posHorizontal;
    Vector3 posVertical;
    Point[,] mat;
    List<Edge> edges = new List<Edge>();

    private class Point
    {
        public int i; public int j;
        public Point(int i, int j)
        {
            this.i = i;
            this.j = j;
        }
        public Point(Point p)
        {
            this.i = p.i;
            this.j = p.j;
        }
    }
    private class Edge
    {
        public GameObject wall;
        public Point p1;
        public Point p2;
        public Edge(GameObject wall, Point p1,Point p2)
        {
            this.wall = wall;
            this.p1 = p1;
            this.p2 = p2;
        }
    }


    void Start()
    {
        posHorizontal = wallHorizontal.transform.position;
        posVertical = wallVertical.transform.position;

        //give group number
        mat = new Point[length, width];
        for(int i=0;i<length;i++)
            for(int j=0;j<width;j++)
                mat[i,j] = new Point(i,j);

        generate_walls();
        KruskalAlgorithm();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void generate_walls()
    {
        // generate the walls and save them
        for (int row = 0; row < length; row++)
        {
            for (int col = 0; col < width; col++)
            {
                //for vertical walls
                Vector3 newPosition = new Vector3(posVertical.x + 10 * col, posVertical.y, posVertical.z + 10 * row);
                GameObject newWall1 = Instantiate(wallVertical, newPosition, wallVertical.transform.rotation);
                if (col != 0 && col != width - 1)
                {
                    edges.Add(new Edge(newWall1,new Point(length - 1 - row, col - 1),new Point(length - 1 - row, col))); //save the edges
                }

                //for horizontal walls
                if (col == width - 1 || (row ==(length-1) && col== (width-1)/2))
                    continue;
                Vector3 newPosition2 = new Vector3(posHorizontal.x + 10 * col, posHorizontal.y, posHorizontal.z + 10 * row);
                GameObject newWall2=Instantiate(wallHorizontal, newPosition2, wallHorizontal.transform.rotation);
                if (row != length - 1)
                {
                    edges.Add(new Edge(newWall2,new Point(length - 1 - row, col),new Point(length - 2 - row, col)));  //save the edges
                }

            }
        }
        //create the ground
        GameObject newFloor=Instantiate(floor, new Vector3((width-1)*5+0.5f,-5,(length-1)*5+0.5f), transform.rotation);
        newFloor.transform.localScale=new Vector3(newFloor.transform.localScale.x*width, newFloor.transform.localScale.y, newFloor.transform.localScale.z*length);
        //add the first linw of walls
        int firstEntry = width / 3;
        int secondEntry = width - firstEntry;
        for(int i = 0; i < width-1; i++)
        {
            if (i == firstEntry || i == secondEntry) continue;
            Vector3 newPosition2 = new Vector3(posHorizontal.x + 10 * i, posHorizontal.y,-5);
            GameObject newWall2 = Instantiate(wallHorizontal, newPosition2, wallHorizontal.transform.rotation);
        }

    }
    void KruskalAlgorithm()
    {
        Debug.Log("start kruskal");
        while (edges.Count > 0)
        {
            int randomIndex = Random.Range(0, edges.Count);
            Edge randomEdge = edges[randomIndex];
            Debug.Log("random edge: " + randomEdge.p1.i+randomEdge.p1.j+" , "+ randomEdge.p2.i+ randomEdge.p2.j);
            //if in the same group, remove from list and continue to next edge
            Point temp1 = find(randomEdge.p1);
            Point temp2 = find(randomEdge.p2);
            if (temp1.i != temp2.i || temp1.j != temp2.j)
            {
                Union(randomEdge.p1, randomEdge.p2);
                Destroy(randomEdge.wall);
            }
            edges.RemoveAt(randomIndex);
        }
    }
    Point find(Point p)
    {
        Debug.Log("find start point: "+p.i+p.j);


        while (mat[p.i,p.j].i!=p.i || mat[p.i, p.j].j != p.j)
        {
            p = mat[p.i,p.j];
            Debug.Log("find point: " + p.i + p.j);

        }
        return p;
    }
    void Union(Point p1, Point p2)
    {
        Point p1Group = find(p1);
        Debug.Log("start union ,p1 group:"+ p1Group.i + p1Group.j);
        Point tempP;
        while(mat[p2.i, p2.j].i != p2.i || mat[p2.i, p2.j].j != p2.j)
        {
            tempP = new Point(mat[p2.i, p2.j]);
            Debug.Log("union: set point:" + tempP.i + tempP.j + " to " + p1Group.i + p1Group.j);
            mat[p2.i, p2.j] = p1Group;
            p2= tempP;
        }
        Debug.Log("union end: set point:" + p2.i + p2.j + " to " + p1Group.i + p1Group.j);
        mat[p2.i, p2.j] = p1Group;
    }

}
