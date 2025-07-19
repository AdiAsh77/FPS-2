using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandamSpawner : MonoBehaviour
{
    // The GameObject to spawn
    public GameObject objectToSpawn;

    // Number of objects to spawn
    public int spawnCount = 10;

    // Area bounds
    public Vector3 areaSize = new Vector3(10, 0, 10);

    // Time after which each object despawns
    public float despawnTime = 20f;

    void Start()
    {
        StartCoroutine(SpawnObjects());
    }

    IEnumerator SpawnObjects()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 randomPosition = GetRandomPosition();
            GameObject spawnedObject = Instantiate(objectToSpawn, randomPosition, Quaternion.identity);
            StartCoroutine(DespawnAfterTime(spawnedObject, despawnTime));

            yield return new WaitForSeconds(despawnTime / spawnCount);
        }
    }

    Vector3 GetRandomPosition()
    {
        float randomX = Random.Range(-areaSize.x / 2, areaSize.x / 2);
        float randomY = Random.Range(-areaSize.y / 2, areaSize.y / 2);
        float randomZ = Random.Range(-areaSize.z / 2, areaSize.z / 2);

        return new Vector3(randomX, randomY, randomZ) + transform.position;
    }

    IEnumerator DespawnAfterTime(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(obj);
        // After despawning, spawn a new object in a random position
        Vector3 randomPosition = GetRandomPosition();
        GameObject newObject = Instantiate(objectToSpawn, randomPosition, Quaternion.identity);
        StartCoroutine(DespawnAfterTime(newObject, time));
    }
}
