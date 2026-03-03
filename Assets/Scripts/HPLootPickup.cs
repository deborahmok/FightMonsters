using TMPro;
using UnityEngine;

public class HPLootPickup : MonoBehaviour
{
    [Header("Heal Settings")]
    [SerializeField] private int minHeal = 1;
    [SerializeField] private int maxHeal = 3;
    [SerializeField] private bool randomizeOnSpawn = true;

    [Header("UI")]
    [SerializeField] private TMP_Text valueText;

    private int healAmount = 1;

    private void Awake()
    {
        if (valueText == null)
            valueText = GetComponentInChildren<TMP_Text>();
    }

    private void Start()
    {
        if (randomizeOnSpawn)
            RollHealAmount();

        UpdateText();
    }

    private void RollHealAmount()
    {
        // inclusive min, inclusive max
        healAmount = Random.Range(minHeal, maxHeal + 1);
    }

    private void UpdateText()
    {
        if (valueText != null)
            valueText.text = healAmount.ToString();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerState ps = other.GetComponent<PlayerState>();
        if (ps == null) return;

        ps.Heal(healAmount);
        Destroy(gameObject);
    }
}