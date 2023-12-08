using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

/// <summary>
/// One time use: for generate spike meshes
/// </summary>
public class SpikeMesh : MonoBehaviour
{

    public float 
      radius = 1,// radius
      height = 2;// height
    public int numOfVertices = 12;// number of edges

    void Start()
    {
        // vertices
        Vector3[] vertices = new Vector3[numOfVertices + 2];
        // triangle meshes
        int[] triangle = new int[2 * 3*(numOfVertices)];
        vertices[0] = new Vector3(0, 0, 0);
        for (int i = 1; i < numOfVertices + 1; i++)
        {
            // vertices on the base of the cone
            vertices[i] = new Vector3((float)(radius * Math.Cos(2 * Math.PI * i / numOfVertices)), (float)(radius * Math.Sin(2 * Math.PI * i / numOfVertices)), 0);
        }
        // vertice of the peak
        vertices[numOfVertices + 1] = new Vector3(0, 0, height);
        

        // for triangle meshes, counterclock order for visibility
        for (int i = 0; i < numOfVertices; i++)
        {
            triangle[3 * i] = 0;
            triangle[3 * i + 1] = i + 2 > numOfVertices ? 1 : i + 2;
            triangle[3 * i + 2] = i + 1;
            triangle[3 * numOfVertices + 3 * i] = numOfVertices + 1;
            triangle[3 * numOfVertices + 3 * i + 1] = i + 1;
            triangle[3 * numOfVertices + 3 * i + 2] = i + 2 > numOfVertices ? 1 : i + 2;
        }

        // replace mesh
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangle;
        // recalculate normal
        mesh.RecalculateNormals();

        // save mesh to assets
        var savePath = "Assets/" + "spikeMesh" + ".asset";
        Debug.Log("Saved Mesh to:" + savePath);
        //AssetDatabase.CreateAsset(mesh, savePath);
    }

}
