using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }
    
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Vector3 camForward = Camera.main.transform.forward;

        Vector3 offset = new Vector3(
            Random.Range(-0.3f, 0.3f),
            Random.Range(1.5f, 2f),
            Random.Range(-0.3f, 0.3f)
        );

        Vector3 spawnPos = transform.position + offset + camForward * 0.5f;

        DamageNumberSpawner.Instance.Spawn(damage, spawnPos);
        //Debug.Log($"{gameObject.name} took {damage} damage. Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        Destroy(gameObject);
    }
}
