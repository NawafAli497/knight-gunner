using UnityEngine;

public class CamFollowPlayer : MonoBehaviour
{
    public Transform target;
    float minY = 0f;
    float smooth = 5f;

    void LateUpdate()
    {
        if(!target) return;

        Vector3 pov = transform.position;

        float targetY = Mathf.Lerp(pov.y , target.position.y, smooth * Time.deltaTime);

        targetY = Mathf.Max(targetY, minY);

        pov.y = targetY;

        transform.position = pov;

    }
}
