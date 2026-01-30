using UnityEngine;

public class branchSpawner : MonoBehaviour
{
public GameObject Branch; // is where we will store the Water Drop prefab
public Transform pointA; // is the transform component of the first point
public Transform pointB;// is the transform component of the second point
public float spawnTime = 2f; //the time between spawning each water drop
private float timer; // a timer.
private int score = 0; // keep track of the score
    void Update()
    {

        timer += Time.deltaTime; // count up the timer

        if (timer >= spawnTime)
        {
            score++; // increase the score by one
            Debug.Log(score); // Prints the score to the console
            SpawnObject();
            timer = 0f; // reset timer
        }
    }
    void SpawnObject()
    {
    // Pick a random position between the two points
    float randomX = Random.Range(pointA.position.x, pointB.position.x);
    float randomY = Random.Range(pointA.position.y, pointB.position.y);
    Vector2 randomPos = new Vector2(randomX, randomY);

    // Spawn the object
    Instantiate(Branch, randomPos, Quaternion.identity);
    }
}
