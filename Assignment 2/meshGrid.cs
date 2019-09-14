using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class meshGrid : MonoBehaviour {

    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;
    Vector2[] uv;

    public GameObject tree;
    public GameObject House;
    public GameObject grass;

    public int xsize = 250;
    public int zsize = 250;

    public int count = 0;
    public int countt = 0;

    public Perlin p = new Perlin();

	// Use this for initialization
	void Start () {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateShape();
        UpdateMesh();
	}

    void CreateShape()
    {
        vertices = new Vector3[(xsize + 1) * (zsize + 1)];

        for (int i = 0, z = 0; z <= zsize; z++) {
            for (int x = 0; x <= xsize; x++) {

                //double y = p.OctavePerlin(xcor, zcor, 0, 6, 20.20) * 80f;

                float xcor = x * .34f /250;
                float zcor = z * .34f /250;

                double y = p.OctavePerlin(xcor, zcor, 0, 5, 20.20) * 90f;
                //height

                vertices[i] = new Vector3(x, (float)y, z);
                i++;

                //Tree
                if ((float)y > 69.507) {
                    Vector3 Treepos = new Vector3(x, (float)y, z);
                    Instantiate(tree, Treepos, Quaternion.identity);
                }

                //House
                if ((float)y > 40 && count < 1 && z == 183 && x == 183)
                {
                    Vector3 Housepos = new Vector3(x, (float)y, z);
                    Instantiate(House, Housepos, Quaternion.identity);
                    count++;
                }

                //Tree2
                if ((float)y > 40 && (float)y < 50 && countt < 1 && z > 30 && x > 30)
                {
                    Vector3 Tree2pos = new Vector3(x, (float)y, z);
                    Instantiate(grass, Tree2pos, Quaternion.identity);
                    countt++;
                }

                Debug.Log("Perlin Noise Y is: " + (float)y);
            }
        }

        //Triangles
        triangles = new int[xsize * zsize * 6];
        int vert = 0;
        int tris = 0;

        for (int z = 0; z < zsize; z++)
        {
            for (int x = 0; x < xsize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xsize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xsize + 1;
                triangles[tris + 5] = vert + xsize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }

        //UV Mapping
        uv = new Vector2[vertices.Length];
        for (int i = 0, z = 0; z <= zsize; z++)
        {
            for (int x = 0; x <= xsize; x++)
            {
                uv[i] = new Vector2((float)x / xsize, (float)z / zsize);
                i++;
            }
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.RecalculateNormals();
    }

    void Update () {
		
	}
}
