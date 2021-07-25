using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quad : MonoBehaviour
{
    public float size = 1.0f;


    void Start()
    {
        // create a new mesh
        var mesh = new Mesh();

        // Define vertices
        var hsize = size * 0.5f;
        var vertices = new Vector3[] {
            new Vector3(-hsize,  hsize, 0.0f),
            new Vector3( hsize,  hsize, 0.0f),
            new Vector3( hsize, -hsize, 0.0f),
            new Vector3(-hsize, -hsize, 0.0f)
        };

        // Define the UVs
        var uv = new Vector2[] {
            new Vector2(0f, 0f), 
            new Vector2(1f, 0f), 
            new Vector2(1f, 1f), 
            new Vector2(0f, 1f)            
        };

        // Define the Normals
        var normals = new Vector3[] {
            new Vector3(0f, 0f, -1f),
            new Vector3(0f, 0f, -1f),
            new Vector3(0f, 0f, -1f),
            new Vector3(0f, 0f, -1f)            
        };

        // Define the Tris, (stride of 3)
        var triangles = new int[] {
            0, 1, 2, // 1
            2, 3, 0, // 2
        };
        mesh.vertices=vertices;
        mesh.uv = uv;
        mesh.normals = normals;
        mesh.triangles = triangles;

        // Bounds used for frustum culling calculation
        mesh.RecalculateBounds();

        // send the mesh to a mesh filter so Unity knows
        // Need a mesh renderer for it to show up.
        GetComponent<MeshFilter>().mesh = mesh;
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
