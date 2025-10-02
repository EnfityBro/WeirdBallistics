using UnityEngine;
using System;

public class TargetSpawner : MonoBehaviour
{
    public static TargetSpawner instance;
    [SerializeField] private GameObject target;
    [NonSerialized] public int hitsCount = 0;
    private float timer = 0.0f;

    private void Awake() => instance = this;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > 10.0f)
        {
            timer = 0.0f;
            SpawnTarget();
        }
    }

    private void SpawnTarget()
    {
        GameObject newTarget = Instantiate(target, new Vector3(UnityEngine.Random.Range(-20.0f, 5.0f), UnityEngine.Random.Range(1.0f, 6.0f), UnityEngine.Random.Range(15.0f, 40.0f)), Quaternion.identity);
        float randomRadius = UnityEngine.Random.Range(1.0f, 2.0f);

        newTarget.GetComponent<Rigidbody>().mass = UnityEngine.Random.Range(1.0f, 10.0f);
        newTarget.transform.localScale = new Vector3(randomRadius, randomRadius, newTarget.transform.localScale.z);
    }
}