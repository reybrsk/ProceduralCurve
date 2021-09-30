using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor.Build;
using UnityEngine;

public class SplineMover : MonoBehaviour
{
    public SplineDraw splineDraw;
    [Range(0f, 10000f)] public float speed = 10f;
    private int _i = 0;

    private Vector3[] points;
    
    private void Start()
    {
        points = splineDraw.GetSplinePoints();
    }

    void Update()
    {
        
        transform.DOMove(points[_i],speed/Time.deltaTime);
        if (_i+1 < points.Length)
        {
            transform.LookAt(points[_i+1],transform.up);
            _i++;
        }
        else
        {
            transform.LookAt(points[0],transform.up);
            _i = 0;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.up);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.right);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward);
    }
}
