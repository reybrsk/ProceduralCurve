using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// [ExecuteAlways]
public class FlipNormals : MonoBehaviour
{
    private Mesh mesh;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Awake()
    {
        mesh = GetComponent<MeshFilter>().sharedMesh;
        Vector3[] normals = mesh.normals;
        for (int i = 0; i < normals.Length; i++)
        {
            normals[i] = -1 * normals[i];
            
        }
        
        mesh.normals = normals;
        
        for (int i = 0; i < mesh.subMeshCount; i++)
        {
            int[] tris = mesh.GetTriangles(i);
            for (int j = 0; j < tris.Length; j+=3)
            {
                (tris[j], tris[j + 1]) = (tris[j + 1], tris[j]);
            }
            mesh.SetTriangles(tris,i);
        }
        mesh.RecalculateNormals();
    }
}
