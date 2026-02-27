using UnityEngine;

public class EnemyTouchDamage : MonoBehaviour
{
    [SerializeField] private int touchDamage = 1;
    [SerializeField] private float damageCooldown = 0.5f;

    private float nextDamageTime = 0f;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (Time.time < nextDamageTime) return;
    
        PlayerState player = other.GetComponent<PlayerState>();
        if (player == null) return;
    
        player.TakeDamage(touchDamage);
        nextDamageTime = Time.time + damageCooldown;
    }
}