using UnityEngine;

public class TargetController : MonoBehaviour
{
    private float pointA;
    private float pointB;
    private float baseSpeed = 2.0f;

    private Rigidbody rb;
    private float currentTarget;
    private bool movingToB = true;

    private void Start()
    {
        pointA = UnityEngine.Random.Range(transform.position.x - 5.0f, transform.position.x - 1.0f);
        pointB = UnityEngine.Random.Range(transform.position.x + 0.0f, transform.position.x + 5.0f);

        rb = GetComponent<Rigidbody>();
        currentTarget = pointB;
        movingToB = true;
    }

    private void FixedUpdate()
    {
        // Calculating direction and distance to the target
        float distanceToTarget = currentTarget - transform.position.x;
        float direction = Mathf.Sign(distanceToTarget);

        // Calculating speed based on mass
        float massFactor = rb != null ? 1f / Mathf.Sqrt(rb.mass) : 1f;
        float speed = baseSpeed * massFactor;

        // Apply force or speed
        Vector3 velocity = rb.linearVelocity;
        velocity.x = direction * speed;
        rb.linearVelocity = velocity;

        if (Mathf.Abs(distanceToTarget) < 0.1f)
        {
            if (movingToB) { movingToB = false; currentTarget = pointA; }
            else { movingToB = true; currentTarget = pointB; }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            TargetSpawner.instance.hitsCount++;
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector3(pointA, transform.position.y, transform.position.z), 0.5f);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(new Vector3(pointB, transform.position.y, transform.position.z), 0.5f);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(pointA, transform.position.y, transform.position.z),
                        new Vector3(pointB, transform.position.y, transform.position.z));
    }
}