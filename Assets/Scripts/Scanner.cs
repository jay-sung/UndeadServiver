using UnityEngine;

public class Scanner : MonoBehaviour
{
    public float scanRange;
    public LayerMask targetLayer;
    public RaycastHit2D[] targets;
    public Transform nearestTarget;

    void FixedUpdate()
    {
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0f, targetLayer);
        nearestTarget = GetNearestTarget();
    }

    Transform GetNearestTarget()
    {
        float minDistance = float.MaxValue;
        Transform closestTarget = null;

        foreach (var hit in targets)
        {
            float distance = Vector2.Distance(transform.position, hit.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestTarget = hit.transform;
            }
        }

        return closestTarget;
    }
}

