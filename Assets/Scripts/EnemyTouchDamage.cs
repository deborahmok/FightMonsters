using UnityEngine;

public class EnemyTouchDamage : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private int touchDamage = 1;
    [SerializeField] private float damageCooldown = 0.5f;

    [Header("Torch Steal")]
    [SerializeField] private bool canStealTorch = false;
    [SerializeField] private int torchAmount = 1;
    [SerializeField] private float torchStealCooldown = 1.0f;

    private float nextDamageTime = 0f;
    private float nextTorchStealTime = 0f;

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



        if (!canStealTorch) return;               // normal enemies stop here

        if (Time.time >= nextTorchStealTime)
        {
            PlayerLight playerLight = other.GetComponent<PlayerLight>();
            if (playerLight != null)
            {
                bool success = playerLight.TryStealTorch(torchAmount);
                if (success)
                {
                    Debug.Log("Enemy stole " + torchAmount + " torch(es) from player.");
                    nextTorchStealTime = Time.time + torchStealCooldown;
                }
            }
        }
    }
}