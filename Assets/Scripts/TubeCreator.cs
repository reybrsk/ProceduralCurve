using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using UnityEngine;


[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]

public class TubeCreator : MonoBehaviour
{

    private SplineDraw spline;
    private Vector3[] points;
    private Vector3[] meshVertices;
    private Vector3[,] doubleArrayVertices;
    private Mesh mesh;
   
    

    public GameObject obj;
    


    void Start()
    {
    
        spline = GetComponent<SplineDraw>();
        points = spline.GetSplinePoints();
        meshVertices = new Vector3[points.Length];
        DrawDebug(GOtoArray(InstantObj(points)));
        // MeshVerticesMaker(GOtoArray(InstantObj(points)));

        CreateVertices();
        CreateTriangles();

    }
    private void CreateVertices()
    {
        mesh.vertices = meshVertices;
    }
    private void CreateTriangles()
      {
          
      }

    private static int
        Setquad(int[] triangles, int i, int v00, int v10, int v01, int v11)
    {
        triangles[i] = v00;
        triangles[i + 1] = triangles[i + 4] = v01;
        triangles[i + 2] = triangles[i + 3] = v10;
        triangles[i + 5] = v11;
        return i + 6;
    }

    // private void MeshCreate(Vector3[] gOtoArray)
    // {
    //     GetComponent<MeshFilter>().mesh = mesh = new Mesh();
    //     mesh.name = "Procedural Grid";
    //     Vector3[] vertices = gOtoArray;
    //     Vector2[] uv = new Vector2[vertices.Length];
    //     Vector4[] tangents = new Vector4[vertices.Length];
    //     Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);
    //     // float t = 1 / (float)zSize;
    //     
    //     
    //     for (int i = 0, x = 0; x <= xSize; x++) {
    //         
    //         for (int z = 0; z <= zSize; z++, i++)
    //         {
    //             Vector3 bezierPoint = Bezier.GetPoint(P0.position, P1.position, P2.position, P3.position, t*z);
    //             // Vector3 lookAt = Bezier.GetFirstDerivative(P0.position, P1.position, P2.position, P3.position, t*z);
    //             // Debug.Log("bezierPoint" + bezierPoint + "lookAT" + lookAt);
    //     
    //             if (x == 0)
    //             {
    //                 vertices[i] = bezierPoint;
    //             }else  vertices[i] = bezierPoint + Vector3.left*2f;
    //             
    //             
    //             
    //             
    //                                 
    //             uv[i] = new Vector2((float)x / xSize, (float)z / zSize);
    //             tangents[i] = tangent;
    //         }
    //     }
    //     mesh.vertices = vertices;
    //     mesh.uv = uv;
    //     mesh.tangents = tangents;
    //
    //     int[] triangles = new int[xSize * zSize * 6];
    //     for (int ti = 0, vi = 0, y = 0; y < xSize; y++, vi++) {
    //         for (int x = 0; x < zSize; x++, ti += 6, vi++) {
    //             triangles[ti] = vi;
    //             triangles[ti + 3] = triangles[ti + 2] = vi + 1;
    //             triangles[ti + 4] = triangles[ti + 1] = vi + zSize + 1;
    //             triangles[ti + 5] = vi + zSize + 2;
    //         }
    //     }
    //     mesh.triangles = triangles;
    //     
    //     Vector3[] normals = mesh.normals;
    //     for (int i = 0; i < normals.Length; i++)
    //     {
    //         normals[i] = -1 * normals[i];
    //     }
    //    
    //     
    //     // Flip Normals
    //     // mesh.normals = normals;
    //     //
    //     // for (int i = 0; i < mesh.subMeshCount; i++)
    //     // {
    //     //     int[] tris = mesh.GetTriangles(i);
    //     //     for (int j = 0; j < tris.Length; j+=3)
    //     //     {
    //     //         (tris[j], tris[j + 1]) = (tris[j + 1], tris[j]);
    //     //     }
    //     //     mesh.SetTriangles(tris,i);
    //     // }
    //      mesh.RecalculateNormals();
    //     
    //     
    //
    //
    // }

    public void Redraw()
    {
        points = spline.GetSplinePoints();
        DrawDebug(GOtoArray(InstantObj(points)));
    }

    private void DrawDebug(Vector3[] arrayVert)
    {
        for (int i = 0; i < arrayVert.Length; i++)
        {
            if (i + 1 < arrayVert.Length)
            {
                Debug.DrawLine(arrayVert[i], arrayVert[i + 1], Color.magenta,100f);
            }
            else
            {
                Debug.DrawLine(arrayVert[i],arrayVert[0],Color.magenta,100f); 
            }
        }
    }

    private Vector3[] GOtoArray(List<GameObject> gaObList)
    {
        List<Vector3[]> pointsArray = new List<Vector3[]>();
        int i = 0;
        foreach (GameObject gameObj in gaObList)
        {
            if (gameObj.TryGetComponent(out SplineDraw spline))
            {
                pointsArray.Add(spline.GetSplinePoints());
                i++;
            }
            else
            {
                Debug.Log("gameObj don't have SplineDraw");
            }

        }

        List<Vector3> meshVerticesCcc = new List<Vector3>();
        
        foreach (Vector3[] circles in pointsArray)
        {
            for (int j = 0; j < circles.Length; j++)
            {
                meshVerticesCcc.Add(circles[j]);
            }
            
        }

        meshVertices = meshVerticesCcc.ToArray();


        return meshVertices;


    }



    private List<GameObject> InstantObj(Vector3[] vector3S)
    {
        List<GameObject> objects = new List<GameObject>();
        for (int i = 0; i < vector3S.Length; i++)
        {
            Quaternion qua;
            GameObject gO;
            if (i == 0)
            {
                qua = Quaternion.LookRotation((vector3S[vector3S.Length - 1] - vector3S[i+1]).normalized, (vector3S[i] - transform.position).normalized);
         
            }
            else if (i == vector3S.Length - 1) 
            {
                qua = Quaternion.LookRotation((vector3S[i-1] - vector3S[0]).normalized, (vector3S[i] - transform.position).normalized);
               
            }
            else
            {
                qua = Quaternion.LookRotation((vector3S[i - 1] - vector3S[i+1]).normalized, (vector3S[i] - transform.position).normalized);
                
            }
            gO = Instantiate(obj,vector3S[i],qua);
           
            objects.Add(gO);
            Destroy(gO);
        }
        return objects;
    }
    
}
