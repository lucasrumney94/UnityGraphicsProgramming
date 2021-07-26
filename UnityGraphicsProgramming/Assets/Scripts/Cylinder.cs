using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cylinder : MonoBehaviour
{
    [SerializeField, Range(0.1f, 10f)] protected float height = 3f, radius = 1f;
    [SerializeField, Range(3, 32)] protected int segments = 16;
    [SerializeField] bool openEnded = true;

    const float PI2 = Mathf.PI * 2.0f;

    void Start()
    {
        // Create Mesh containers
        var mesh = new Mesh();

        var vertices = new List<Vector3>();
        var normals = new List<Vector3>();
        var uvs = new List<Vector2>();
        var triangles = new List<int>();

        // Construct Cylinder
        float top = height *0.5f;
        float bottom = -height *0.5f;

        // Vertex data for sides
        GenerateCap(segments + 1, top, bottom, radius, vertices, uvs, normals, true);

        // Refer to the vertices on the circle while constructing the side tri's
        var len = (segments + 1) * 2;

        // Build the sides by connecting the top and bottom
        for (int i = 0; i < segments + 1; i++) {
            int idx = i * 2;
            int a = idx, b = idx + 1, c = (idx + 2) % len, d = (idx + 3) % len;
            triangles.Add(a);
            triangles.Add(c);
            triangles.Add(b);

            triangles.Add(d);
            triangles.Add(b);
            triangles.Add(c);
        }

        // Generate Caps
        if(!openEnded)
        {
            // Lid points created separate 
            GenerateCap(segments + 1, top, bottom, radius, vertices, uvs, normals, false);

            // Top middle point
            vertices.Add(new Vector3(0f, top, 0f));
            uvs.Add(new Vector2(0.5f, 1f));
            normals.Add(new Vector3(0f, 1f, 0f));

            // Bottom middle point
            vertices.Add(new Vector3(0f, top, 0f));
            uvs.Add(new Vector2(0.5f, 1f));
            normals.Add(new Vector3(0f, 1f, 0f));

            var it = vertices.Count - 2;
            var ib = vertices.Count - 1;

            // offset to avoid indexing the already-created side points
            var offset = len;

            // Top lid Surface
            for (int i = 0; i < len; i += 2) {
                triangles.Add(it);
                triangles.Add((i + 2) % len + offset);
                triangles.Add(i + offset);
            }

            // Bottom lid Surface
            for (int i = 1; i < len; i += 2) {
                triangles.Add(ib);
                triangles.Add(i + offset);
                triangles.Add((i + 2) % len + offset);
            }
        }
        


        // Assign information back to mesh
        mesh.vertices = vertices.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.normals = normals.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateBounds();

        // Assign to Mesh Filter
        GetComponent<MeshFilter>().mesh = mesh;
    }

    void GenerateCap(int segments, float top, float bottom, float radius, 
    List<Vector3> vertices, List<Vector2> uvs, List<Vector3> normals, bool side)
    {
        for (int i = 0; i < segments; i++)
        {
            // 0.0 ~ 1.0 (normalized)
            float ratio = (float)i/(segments-1);
            float rad = ratio*PI2;

            // Sweep the circle and generate points
            float cos = Mathf.Cos(rad), sin = Mathf.Sin(rad);
            float x = cos*radius, z = sin*radius;
            Vector3 tp = new Vector3(x, top, z), bp = new Vector3(x, bottom, z);

            // Add the verts and UVs
            // vertices and uvs we passed by reference
            vertices.Add(tp);
            uvs.Add(new Vector2(ratio, 1f));

            vertices.Add(bp);
            uvs.Add(new Vector2(ratio, 0f));

            if (side)
            {
                // normals pointing out from each side.
                var normal = new Vector3(cos, 0f, sin);
                // We are adding 2 here (1 normal per tri, 2 tris in each sides quad)
                normals.Add(normal);
                normals.Add(normal);
            }
            else
            {
                normals.Add(new Vector3(0f, 1f, 0f)); // top cap
                normals.Add(new Vector3(0f, -1f, 0f)); // bottom cap
            }

            


        }
    }   

    
    void Update()
    {
        
    }
}
