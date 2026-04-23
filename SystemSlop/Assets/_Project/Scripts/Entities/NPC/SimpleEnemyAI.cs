using UnityEngine;

public class SimpleEnemyAI : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float baseMoveSpeed = 3f;
    [SerializeField] private float stopDistance = 1.5f;

    public float currentMoveSpeed;

    private void Awake()
    {
        currentMoveSpeed = baseMoveSpeed;
    }

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
    }

    private void Update()
    {
        if (target == null) return;

        Vector3 direction = target.position - transform.position;
        float distance = direction.magnitude;

        if(distance <= stopDistance)
        {
            // Stop moving if within stop distance
            return;
        }

        direction.y = 0;
        direction.Normalize();

        transform.position += direction * currentMoveSpeed * Time.deltaTime;


    }

    public void ApplySlow(float multiplier)
    {
        currentMoveSpeed *= multiplier;
    }

    public void ResetSpeed()
    {
        currentMoveSpeed = baseMoveSpeed;
    }
}
