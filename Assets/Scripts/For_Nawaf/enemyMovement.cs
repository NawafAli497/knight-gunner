using UnityEngine;

public class PathMover : MonoBehaviour
{
    [Header("Path Settings")]
    public Transform[] waypoints;   // Drag your Point objects here
    public float moveSpeed = 5f;    // How fast to move
    public float waitTime = 0.5f;   // Optional delay at each point
    
    [Header("Loop Settings")]
    public bool loop = true;        // Should it restart when it hits the end?

    private int currentPointIndex = 0;
    private float waitCounter = 0f;
    private bool isWaiting = false;

    void Update()
    {
        if (isWaiting)
        {
            waitCounter -= Time.deltaTime;
            if (waitCounter <= 0f)
            {
                isWaiting = false;
            }
            return; // Stop moving while waiting
        }

        // 1. Get the current target position
        Transform targetWaypoint = waypoints[currentPointIndex];

        // 2. Move towards the target
        // MoveTowards(current, target, maxDistanceDelta)
        transform.position = Vector2.MoveTowards(transform.position, targetWaypoint.position, moveSpeed * Time.deltaTime);

        // 3. Check if we have reached the target (distance is very small)
        if (Vector2.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            // We arrived! Pause for a moment?
            if (waitTime > 0)
            {
                isWaiting = true;
                waitCounter = waitTime;
            }

            // Go to next waypoint
            currentPointIndex++;

            // 4. Handle the end of the path
            if (currentPointIndex >= waypoints.Length)
            {
                if (loop)
                {
                    currentPointIndex = 0; // Go back to start
                }
                else
                {
                    currentPointIndex = waypoints.Length - 1; // Stop at the end
                    this.enabled = false; // Disable script
                }
            }
        }
    }
}