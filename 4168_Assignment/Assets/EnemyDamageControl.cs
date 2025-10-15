using UnityEngine;
using System.Collections;

public class EnemyDamageControl : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to the player's hand
        if (other.CompareTag("PlayerHand"))
        {
            Debug.Log("Attacked By Hand");
            // Get the PlayerControl script from the player
            PlayerControl playerControl = FindFirstObjectByType<PlayerControl>();

            // Check if the player is attacking
            if (playerControl != null && playerControl.isAttacking)
            {
                _animator.SetBool("gotHit", true);
                // Destroy the enemy
                StartCoroutine(DisappearAfterDelay(1f)); // Call coroutine
            }
        }
    }

    private IEnumerator DisappearAfterDelay(float delay){
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
