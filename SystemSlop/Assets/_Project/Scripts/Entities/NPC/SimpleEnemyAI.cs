using UnityEngine;

namespace Project.Entities
{
    public class SimpleEnemyAI : MonoBehaviour
    {
        [SerializeField] private bool chasePlayer;
        [SerializeField] private Transform player;
        [SerializeField] private float baseMoveSpeed = 3f;
        [SerializeField] private float stopDistance = .1f;

        [SerializeField] private Transform[] patrolPoints;
        [SerializeField] private bool loop;

        private int _currentIndex;

        private float currentMoveSpeed;

        private void Awake()
        {
            currentMoveSpeed = baseMoveSpeed;
        }

        private void Start()
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
            {
                player = p.transform;
            }
        }

        private void Update()
        {
            Transform target = GetTarget();
            if (target == null) return;

            Vector3 direction = target.position - transform.position;
            direction.y = 0;

            float distance = direction.magnitude;

            if (!chasePlayer && distance <= stopDistance)
            {
                AdvancePoint();
                return;
            }

            direction.Normalize();

            transform.position += direction * currentMoveSpeed * Time.deltaTime;


        }

        private Transform GetTarget()
        {
            if (chasePlayer && player != null)
                return player;

            if (patrolPoints == null || patrolPoints.Length == 0)
                return null;

            return patrolPoints[_currentIndex];
        }

        private void AdvancePoint()
        {
            _currentIndex++;

            if (_currentIndex >= patrolPoints.Length)
            {
                _currentIndex = 0;
            }

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
}