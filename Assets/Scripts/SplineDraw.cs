using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[ExecuteAlways]
public class SplineDraw : MonoBehaviour
{
    private Vector3[] _points;
    [Range(1, 256), SerializeField] private int segments = 100;
    [Range(1, 2), SerializeField] private int splineP = 2;
    public Transform[] curvePoints;
    private Vector3[] curvePointsOld;
    private Vector3[] _splinePoints;
    public Color col;
    

    
    void Awake()
    {
        _points = new Vector3[curvePoints.Length];
        for (int i = 0; i < curvePoints.Length; i++)
        {
            _points[i] = curvePoints[i].position;
        }
        
        _splinePoints =  DpSpline.Calculate(_points, splineP,segments);
        curvePointsOld = new Vector3[curvePoints.Length];
        UpdateCurve();
        Generate();
    }

    private void UpdateCurve()
    {
        for (int i = 0; i < curvePointsOld.Length; i++)
        {
            curvePointsOld[i] = curvePoints[i].position;
        }
        
    }

    void Update()
    {
        if (CheckCurve())
        {
            Generate();
            UpdateCurve();
            Debug.Log("Curve Is Rebuild!!!!!");
            TubeCreator tubeCreator = GetComponent<TubeCreator>();
            tubeCreator.Redraw();
        }
        

    }

    private bool CheckCurve()
    {
        for (int i = 0; i < curvePoints.Length; i++)
        {
            if (curvePoints[i].position != curvePointsOld[i])
            {
                return true;
            }
        }

        return false;
    }

    void Generate()
    {
        for (int i = 0; i < curvePoints.Length; i++)
        {
            _points[i] = curvePoints[i].position;
        }
        _splinePoints =  DpSpline.Calculate(_points, splineP,segments);
    }
    
    
    public  Vector3[] GetSplinePoints()
    {
        return _splinePoints;
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < _splinePoints.Length; i++)
        {
            if (i + 1 < _splinePoints.Length)
            {
                Debug.DrawLine(_splinePoints[i], _splinePoints[i + 1], col);
            }
            else
            {
                Debug.DrawLine(_splinePoints[i],_splinePoints[0],col); 
            }
        }
    }
}
