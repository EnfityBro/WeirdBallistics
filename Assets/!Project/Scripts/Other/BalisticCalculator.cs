using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(TraectoryRenderer))]
public class BalisticCalculator : MonoBehaviour
{
    public static BalisticCalculator instance;

    // cannon config
    [SerializeField] private Transform shotPoint;
    [SerializeField] private Transform projectile;
    [NonSerialized] public float muzzleAngle = 20;
    [NonSerialized] public bool isRandomMassRadius = true;
    private float muzzleVelocity = 20;
    private TraectoryRenderer traectoryRenderer;
    
    // physics parameters
    [NonSerialized] public float mass = 1.0f;               // kg
    [NonSerialized] public float radius = 0.1f;             // m
    [NonSerialized] public float dragCoefficient = 0.47f;   // sphere
    [NonSerialized] public float airDensity = 1.225f;       // kg/m^3
    [NonSerialized] public float area;                      // r^2
    [NonSerialized] public Vector3 wind = Vector3.zero;     // m/s

    private void Awake()
    {
        instance = this;
        traectoryRenderer = GetComponent<TraectoryRenderer>();
        area = radius * radius * Mathf.PI;
    }

    public void Update()
    {
        Vector3 v0 = CalculateVelocityVector(muzzleAngle);
        traectoryRenderer.DrawWithAirEuler(shotPoint.position, v0);

        if (Keyboard.current.spaceKey.wasPressedThisFrame) Fire(v0);
    }

    private Vector3 CalculateVelocityVector(float angle)
    {
        float vx = muzzleAngle * Mathf.Cos(angle * Mathf.Deg2Rad);
        float vy = muzzleAngle * Mathf.Sin(angle * Mathf.Deg2Rad);

        return shotPoint.forward * vx + shotPoint.up * vy;
    }

    private void Fire(Vector3 initialVelocity)
    {
        Instantiate(projectile.gameObject, shotPoint.position, Quaternion.identity).GetComponent<QuadraticDrag>().SetPhysicalParams(
            isRandomMassRadius ? UnityEngine.Random.Range(1.0f, 5.0f) : mass,
            isRandomMassRadius ? UnityEngine.Random.Range(0.1f, 0.5f) : radius,
            dragCoefficient, airDensity, wind, initialVelocity);
    }
}