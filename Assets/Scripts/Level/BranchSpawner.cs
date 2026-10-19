using UnityEngine;

// Fall level: drops tree branches at random positions between two points.
public class BranchSpawner : MonoBehaviour
{
    public GameObject Branch;       // branch prefab to spawn
    public Transform pointA;        // one corner of the spawn area
    public Transform pointB;        // opposite corner of the spawn area
    public float spawnTime = 2f;    // seconds between spawns
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnTime)
        {
            SpawnObject();
            timer = 0f;
        }
    }

    void SpawnObject()
    {
        float randomX = Random.Range(pointA.position.x, pointB.position.x);
        float randomY = Random.Range(pointA.position.y, pointB.position.y);
        Vector2 randomPos = new Vector2(randomX, randomY);

        Instantiate(Branch, randomPos, Quaternion.identity);
    }
}
