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

    [SerializeField] private RectTransform floatingTextPrefab;

    // IMPORTANT: don't rely on dragging a scene Canvas into a prefab
    [SerializeField] private Transform uiCanvas;

    private int healAmount = 1;
    private Vector3 startPos;

    private void Awake()
    {
        startPos = transform.position;

        if (valueText == null)
            valueText = GetComponentInChildren<TMP_Text>(true);

        // Auto-find a Canvas in the scene if not assigned (prefab-safe)
        if (uiCanvas == null)
        {
            Canvas c = FindFirstObjectByType<Canvas>(); // Unity 6+
            if (c != null) uiCanvas = c.transform;
        }
    }

    private void Update()
    {
        transform.position = startPos + Vector3.up * Mathf.Sin(Time.time * 3f) * 0.05f;
    }

    private void Start()
    {
        if (randomizeOnSpawn)
            RollHealAmount();

        UpdateText();
    }

    private void SpawnFloatingText(Vector3 worldPos, string msg)
    {
        if (floatingTextPrefab == null)
        {
            Debug.LogWarning("HPLootPickup: floatingTextPrefab not assigned.");
            return;
        }

        if (uiCanvas == null)
        {
            Debug.LogWarning("HPLootPickup: uiCanvas not found/assigned.");
            return;
        }

        Camera cam = Camera.main;
        if (cam == null) return;

        Vector3 screenPos = cam.WorldToScreenPoint(worldPos);

        RectTransform rt = Instantiate(floatingTextPrefab, uiCanvas);
        rt.position = screenPos;

        FloatingText ft = rt.GetComponent<FloatingText>();
        if (ft != null) ft.SetText(msg);
    }

    private void RollHealAmount()
    {
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

        AudioManager.Instance.PlayHPPickup();
        ps.Heal(healAmount);
        SpawnFloatingText(transform.position, "+" + healAmount);

        Destroy(gameObject);
    }
}