using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2.5f;
    [SerializeField] private float stopDistance = 0.1f;
    [SerializeField] private Transform target;

    void Awake()
    {
        TryFindTarget();
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            TryFindTarget();
            return;
        }
        MoveTowardsTarget(Time.deltaTime);
    }

    private void MoveTowardsTarget(float deltaTime)
    {
        Vector2 current = transform.position;
        Vector2 toTarget = (Vector2)target.position - current;

        if (toTarget.sqrMagnitude <= stopDistance * stopDistance)
        {
            return;
        }

        Vector2 next = current + toTarget.normalized * moveSpeed * deltaTime;
        transform.position = next;
    }

    private void TryFindTarget()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
    }
}
