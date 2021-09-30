// using System;
// using System.CodeDom.Compiler;
// using System.Collections;
// using System.Collections.Generic;
// using System.Security.Cryptography;
// using System.Xml.Xsl;
// using UnityEngine;
// [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
//
// public class MyGrid : MonoBehaviour
// {
//     private Mesh mesh;
//     private Vector3[] vertices;
//     
//    
//
//    
//
//
//     // private void Update()
//     //
//     // {
//     //     Generate();
//     //    
//     //   GetComponent<MeshCollider>().sharedMesh = mesh;
//     // }
//
//     public void Generate(Vector3[] vert)
//     {
//         GetComponent<MeshFilter>().mesh = mesh = new Mesh();
//         mesh.name = "Procedural Grid";
//         vertices = new Vector3[(zSize + 1) * (xSize + 1)];
//         Vector2[] uv = new Vector2[vertices.Length];
//         Vector4[] tangents = new Vector4[vertices.Length];
//         Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);
//         float t = 1 / (float)zSize;
//         
//         
//         for (int i = 0, x = 0; x <= xSize; x++) {
//             
//             for (int z = 0; z <= zSize; z++, i++)
//             {
//                 Vector3 bezierPoint = Bezier.GetPoint(P0.position, P1.position, P2.position, P3.position, t*z);
//                 // Vector3 lookAt = Bezier.GetFirstDerivative(P0.position, P1.position, P2.position, P3.position, t*z);
//                 // Debug.Log("bezierPoint" + bezierPoint + "lookAT" + lookAt);
//         
//                 if (x == 0)
//                 {
//                     vertices[i] = bezierPoint;
//                 }else  vertices[i] = bezierPoint + Vector3.left*2f;
//                 
//                 
//                 
//                 
//                                     
//                 uv[i] = new Vector2((float)x / xSize, (float)z / zSize);
//                 tangents[i] = tangent;
//             }
//         }
//         mesh.vertices = vertices;
//         mesh.uv = uv;
//         mesh.tangents = tangents;
//
//         int[] triangles = new int[xSize * zSize * 6];
//         for (int ti = 0, vi = 0, y = 0; y < xSize; y++, vi++) {
//             for (int x = 0; x < zSize; x++, ti += 6, vi++) {
//                 triangles[ti] = vi;
//                 triangles[ti + 3] = triangles[ti + 2] = vi + 1;
//                 triangles[ti + 4] = triangles[ti + 1] = vi + zSize + 1;
//                 triangles[ti + 5] = vi + zSize + 2;
//             }
//         }
//         mesh.triangles = triangles;
//         
//         Vector3[] normals = mesh.normals;
//         for (int i = 0; i < normals.Length; i++)
//         {
//             normals[i] = -1 * normals[i];
//         }
//        
//         
//         // Flip Normals
//         // mesh.normals = normals;
//         //
//         // for (int i = 0; i < mesh.subMeshCount; i++)
//         // {
//         //     int[] tris = mesh.GetTriangles(i);
//         //     for (int j = 0; j < tris.Length; j+=3)
//         //     {
//         //         (tris[j], tris[j + 1]) = (tris[j + 1], tris[j]);
//         //     }
//         //     mesh.SetTriangles(tris,i);
//         // }
//          mesh.RecalculateNormals();
//         
//         
//
//
//     }
//
//     public Vector3 StayPoint (int lineNumber)
//     {
//         Vector3 point;
//         point = Vector3.Lerp(mesh.vertices[zSize + lineNumber],mesh.vertices[lineNumber], 0.5f);
//         return point;
//     }
//
//     public Vector3 StayForvard()
//     {
//         Vector3 forvard;
//         forvard  = Bezier.GetFirstDerivative(P0.position, P1.position, P2.position, P3.position, 0.1f);
//         return forvard;
//     }
// }
