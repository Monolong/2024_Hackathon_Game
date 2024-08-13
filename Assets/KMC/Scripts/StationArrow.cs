using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationArrow : MonoBehaviour
{
    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f);
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        DrawArrow(transform.position, worldPosition);
    }

    private void DrawArrow(Vector3 start, Vector3 destination)
    {
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, destination);
    }
}
