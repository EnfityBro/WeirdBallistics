using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TraectoryRenderer : MonoBehaviour
{
    private const int pointCount = 46;
    private const float timeStep = 0.1f;
    private LineRenderer lineRenderer;

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
}