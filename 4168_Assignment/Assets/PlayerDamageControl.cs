using UnityEngine;

public class PlayerDamageControl : MonoBehaviour
{
    [SerializeField] private float damageInterval = 1f; // Time between consecutive damages
    [SerializeField] private float triggerCooldown = 1f; // Cooldown to prevent rapid trigger re-entry
    [SerializeField] private float detectionRadius = 1f; // Radius for detecting enemies
    [SerializeField] private LayerMask enemyLayer; // Layer for enemy objects

    private PlayerCharacter playerCharacter;
    private bool isTouchingEnemy = false;
    private float lastDamageTime = -Mathf.Infinity;
    private float lastTriggerTime = -Mathf.Infinity;

    void Start()
    {
        playerCharacter = FindFirstObjectByType<PlayerCharacter>();
    }

    void Update()
    {
        // Check for enemies in range using overlap sphere
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, enemyLayer);
        isTouchingEnemy = hitColliders.Length > 0;

        // Apply continuous damage
        if (isTouchingEnemy && Time.time - lastDamageTime >= damageInterval)
        {
            if (playerCharacter != null)
            {
                playerCharacter.takeDamage();
            }
            lastDamageTime = Time.time;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyBot") && Time.time - lastTriggerTime >= triggerCooldown)
        {
            isTouchingEnemy = true;
            if (playerCharacter != null)
            {
                playerCharacter.takeDamage();
                lastDamageTime = Time.time;
            }
            lastTriggerTime = Time.time;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("EnemyBot"))
        {
            isTouchingEnemy = false;
        }
    }
}
