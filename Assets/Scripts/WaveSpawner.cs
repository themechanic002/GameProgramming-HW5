using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public GameObject prefab;
    public float startTime;
    public float endTime;
    public float spawnRate;

    void Start()
    {
        WavesManager.instance.AddWave(this);
        InvokeRepeating("Spawn", startTime, spawnRate);
        Invoke("EndSpawner", endTime);
    }

    void Spawn()
    {
        // Spawn enemies in a random position
        Vector3 randomPosition = new Vector3(transform.position.x + Random.Range(-13, 13), transform.position.y, transform.position.z + Random.Range(-13, 13));
        Instantiate(prefab, randomPosition, transform.rotation);

        // Instantiate(prefab, transform.position, transform.rotation);
    }

    void EndSpawner()
    {
        WavesManager.instance.RemoveWave(this);
        CancelInvoke();
    }
}
