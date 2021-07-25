using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    public float width;
    public float height;
    public int widthSegments = 5;
    public int heightSegments = 7;
    
    

    void Start()
    {
        // Establish data containers
        var vertices = new List<Vector3>();
        var uv = new List<Vector2>();
        var normals = new List<Vector3>();

        // factor for normalizing UV
        // constants calculated as preprocess for loop logic
        var winv = 1f / (widthSegments - 1);
        var hinv = 1f / (heightSegments - 1);


        // Vertices, UVs, Normals
        for (int y=0; y<heightSegments; y++)
        {
            var ry = y * hinv;
            for (int x = 0; x<widthSegments; x++)
            {
                var rx = x * winv;

                vertices.Add(
                    new Vector3(
                        (rx - 0.5f)*width, // centers on origin
                        0f, 
                        (0.5f - ry) * height  // centers on origin
                    )
                );

                uv.Add(new Vector2(rx, ry));
                normals.Add(new Vector3(0f, 1f, 0f));

            }
        }

        // Triangles
        var triangles = new List<int>();
        for(int y = 0; y < heightSegments - 1; y++) {
            for(int x = 0; x < widthSegments - 1; x++) {
                int index = y * widthSegments + x;
                var a = index;
                var b = index + 1;
                var c = index + 1 + widthSegments;
                var d = index + widthSegments;

                // Like 1 Quad, tiled (for loops)
                triangles.Add(a);
                triangles.Add(b);
                triangles.Add(c);

                triangles.Add(c);
                triangles.Add(d);
                triangles.Add(a);
            }
        }

        var mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.uv = uv.ToArray();
        mesh.normals = normals.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateBounds(); // for culling

        GetComponent<MeshFilter>().mesh = mesh;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
