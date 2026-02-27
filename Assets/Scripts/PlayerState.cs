using UnityEngine;

public class PlayerState : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHP = 20;
    [SerializeField] private int currentHP = 20;

    [Header("Torches")]
    [SerializeField] private int torches = 3;

    public int MaxHP => maxHP;
    public int CurrentHP => currentHP;
    public int Torches => torches;

    public bool IsDead => currentHP <= 0;

    private void Start()
    {
        // Ensure valid start
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
    }

    public void Heal(int amount)
    {
        if (amount <= 0 || IsDead) return;
        currentHP = Mathf.Clamp(currentHP + amount, 0, maxHP);
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0 || IsDead) return;
        currentHP = Mathf.Clamp(currentHP - amount, 0, maxHP);
        Debug.Log("HP: " + currentHP);
        if (IsDead)
        {
            Debug.Log("Player died.");
            // Later: trigger lose state
        }
    }

    public bool TrySpendTorch(int amount = 1)
    {
        if (amount <= 0) return true;
        if (torches < amount) return false;
        torches -= amount;
        return true;
    }

    public void AddTorches(int amount)
    {
        if (amount <= 0) return;
        torches += amount;
    }
}