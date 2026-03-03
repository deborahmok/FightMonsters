using UnityEngine;

public class EnemyTouchDamage : MonoBehaviour
{
    [SerializeField] private int touchDamage = 1;
    [SerializeField] private float damageCooldown = 0.5f;

    private float nextDamageTime = 0f;

    private void OnCollisionStay2D(Collision2D collision)
    {
        // collider we are touching
        Collider2D other = collision.collider;

        if (!other.CompareTag("Player")) return;

        Debug.Log("COLLISION STAY with: " + other.name);

        if (Time.time < nextDamageTime) return;

        PlayerState player = other.GetComponent<PlayerState>();
        if (player == null) return;

        Debug.Log("Enemy damaged player: " + other.name);
        player.TakeDamage(touchDamage);

        nextDamageTime = Time.time + damageCooldown;
    }
}