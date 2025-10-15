using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;              // Reference to player transform
    [SerializeField] private Animator _animator; // Optional: link to enemy Animator

    [Header("Settings")]
    public float detectionRange = 10f;    // Distance within which enemy detects player
    public float moveSpeed = 3f;          // Movement speed
    public float rotationSpeed = 5f;      // How fast to turn toward player

    private bool isChasing = false;

    void Update()
    {
        if (player == null)
        {
            Debug.LogWarning("EnemyAI: Player reference not set!");
            return;
        }

        // Distance check
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            // Player in range — chase!
            isChasing = true;
            ChasePlayer();
        }
        else
        {
            // Player out of range — stop chasing
            isChasing = false;
            if (_animator) _animator.SetBool("isRunning", false);
        }
    }

    private void ChasePlayer()
    {
        // Face toward player smoothly
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f; // Ignore vertical tilt
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

        // Move toward player
        transform.position += direction * moveSpeed * Time.deltaTime;

        // Trigger animation
        if (_animator) _animator.SetBool("isRunning", true);
    }

    // Optional: Draw detection radius in Scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
