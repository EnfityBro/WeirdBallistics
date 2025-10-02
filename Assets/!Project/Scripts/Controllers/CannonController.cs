using UnityEngine;

public class CannonController : MonoBehaviour
{
    private Transform muzzle;
    private float currentMuzzleRotationX = -9.0f;
    private float currentMuzzleRotationZ = 0.0f;
    private float currentCannonRotationY = 0.0f;

    private void Awake() => muzzle = transform.GetChild(0).GetComponent<Transform>();

    void Update()
    {
        if (Input.GetKey(KeyCode.W)) MoveMuzzleX(-1);
        else if (Input.GetKey(KeyCode.S)) MoveMuzzleX(1);
        else if (Input.GetKey(KeyCode.A)) MoveMuzzleZ(1);
        else if (Input.GetKey(KeyCode.D)) MoveMuzzleZ(-1);
        
        if (Input.GetKey(KeyCode.Q)) MoveCannonY(-1);
        else if (Input.GetKey(KeyCode.E)) MoveCannonY(1);
    }

    private void MoveMuzzleX(int sign)
    {
        currentMuzzleRotationX += sign * 50f * Time.deltaTime;
        currentMuzzleRotationX = Mathf.Clamp(currentMuzzleRotationX, -27.0f, 27.0f);
        muzzle.localEulerAngles = new Vector3(currentMuzzleRotationX, muzzle.localEulerAngles.y, muzzle.localEulerAngles.z);
    }

    private void MoveMuzzleZ(int sign)
    {
        currentMuzzleRotationZ += sign * 50f * Time.deltaTime;
        currentMuzzleRotationZ = Mathf.Clamp(currentMuzzleRotationZ, -47.0f, 47.0f);
        muzzle.localEulerAngles = new Vector3(muzzle.localEulerAngles.x, muzzle.localEulerAngles.y, currentMuzzleRotationZ);
    }

    private void MoveCannonY(int sign)
    {
        currentCannonRotationY += sign * 50f * Time.deltaTime;
        currentCannonRotationY = Mathf.Clamp(currentCannonRotationY, -60.0f, 25.0f);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, currentCannonRotationY, transform.localEulerAngles.z);
    }
}