using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedStationArrow : MonoBehaviour
{
    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();

        DrawArrow(transform.position, transform.position);
    }

    public void DrawArrow(Vector3 start, Vector3 destination)
    {
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, destination);
    }
}
