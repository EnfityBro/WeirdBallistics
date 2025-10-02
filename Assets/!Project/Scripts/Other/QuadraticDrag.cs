using UnityEngine;

public class QuadraticDrag : MonoBehaviour
{
    private float radius;
    private float dragCoefficient;
    private float airDensity;
    private float area;
    private Vector3 wind = Vector3.zero;
    private Rigidbody rb;

    private void Awake() => rb = GetComponent<Rigidbody>();

    private void Start() => Destroy(gameObject, 15.0f);

    private void FixedUpdate()
    {
        Vector3 vReaL = rb.linearVelocity - wind;
        float speed = vReaL.magnitude;

        Vector3 drag = -0.5f * airDensity * dragCoefficient * area * speed * vReaL;
        rb.AddForce(drag, ForceMode.Force);
    }

    public void SetPhysicalParams(float mass, float radius, float dragCoefficient, float airDensity, Vector3 wind, Vector3 initialVelocity)
    {
        rb.mass = mass;
        this.radius = radius;
        this.dragCoefficient = dragCoefficient;
        this.airDensity = airDensity;
        this.wind = wind;
        rb.linearVelocity = initialVelocity;

        rb.useGravity = true;
        area = radius * radius * Mathf.PI;
    }
}