using System;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TraectoryRenderer : MonoBehaviour
{
    private const int pointCount = 4650;
    private const float timeStep = 0.001f;
    private LineRenderer lineRenderer;
    [NonSerialized] public bool enableTraectoryCut = false;

    private void Awake() => lineRenderer = GetComponent<LineRenderer>();

    //public void DrawVacuum(Vector3 startPosition, Vector3 startVelocity)
    //{
    //    lineRenderer.positionCount = pointCount;

    //    for (int i = 0; i < pointCount; i++)
    //    {
    //        float t = i * timeStep;
    //        Vector3 newPosition = startPosition + (t * startVelocity) + (Physics.gravity * t * t / 2);
    //        lineRenderer.SetPosition(i, newPosition);
    //    }
    //}

    public void DrawWithAirEuler(Vector3 startPosition, Vector3 startVelocity)
    {
        if (!enableTraectoryCut)
        {
            Vector3 p = startPosition;
            Vector3 v = startVelocity;
            lineRenderer.positionCount = pointCount;

            for (int i = 0; i < pointCount; i++)
            {
                lineRenderer.SetPosition(i, p);

                // acceleration: g + Fd/m, Fd = -0.5*rho*Cd*A*|v_rel|*v_rel
                Vector3 vRel = v - BalisticCalculator.instance.wind;
                float speed = vRel.magnitude;
                Vector3 drag = speed > 1e-6f ? (-0.5f * BalisticCalculator.instance.airDensity * BalisticCalculator.instance.dragCoefficient * BalisticCalculator.instance.area * speed) * vRel : Vector3.zero;
                Vector3 a = Physics.gravity + drag / BalisticCalculator.instance.mass;

                v += a * timeStep;  // step by speed
                p += v * timeStep;  // step by position
            }
        }
        else
        {
            Vector3[] points = new Vector3[pointCount];
            points[0] = startPosition;
            int currentPointCount = pointCount;

            for (int i = 1; i < pointCount; i++)
            {
                // acceleration: g + Fd/m, Fd = -0.5*rho*Cd*A*|v_rel|*v_rel
                Vector3 vRel = startVelocity - BalisticCalculator.instance.wind;
                float speed = vRel.magnitude;
                Vector3 drag = speed > 1e-6f ? (-0.5f * BalisticCalculator.instance.airDensity * BalisticCalculator.instance.dragCoefficient * BalisticCalculator.instance.area * speed) * vRel : Vector3.zero;
                Vector3 a = Physics.gravity + drag / BalisticCalculator.instance.mass;

                startVelocity += a * timeStep;
                Vector3 newPosition = startPosition + startVelocity * timeStep;

                // Collide check
                Vector3 direction = newPosition - startPosition;
                float distance = direction.magnitude;

                if (distance > 0)
                {
                    if (Physics.SphereCast(startPosition, 0.1f, direction.normalized, out RaycastHit hit, distance, ~(1 << 6)))
                    {
                        // If collide were detected - adding a new collide point and stop the cicle
                        points[i] = hit.point;
                        currentPointCount = i + 1;
                        break;
                    }
                }

                points[i] = newPosition;
                startPosition = newPosition;
            }

            // Set the necessary points count only
            lineRenderer.positionCount = currentPointCount;
            for (int i = 0; i < currentPointCount; i++)
            {
                lineRenderer.SetPosition(i, points[i]);
            }
        }
    }
}