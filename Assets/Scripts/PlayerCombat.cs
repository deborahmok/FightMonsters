using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Slash")]
    [SerializeField] private GameObject slashHitboxPrefab;
    [SerializeField] private float slashCooldown = 0.35f;

    private float nextSlashTime = 0f;

    private void Update()
    {
        if (Time.time < nextSlashTime) return;

        // Space or left click (pick one or keep both)
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            DoSlash();
            nextSlashTime = Time.time + slashCooldown;
        }
    }

    private void DoSlash()
    {
        if (slashHitboxPrefab == null)
        {
            Debug.LogWarning("No slashHitboxPrefab assigned on PlayerCombat.");
            return;
        }

        Instantiate(slashHitboxPrefab, transform.position, Quaternion.identity);
    }
}