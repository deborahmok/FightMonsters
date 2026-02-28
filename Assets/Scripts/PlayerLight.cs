using UnityEngine;

public class PlayerLight : MonoBehaviour
{
    [SerializeField] private SpriteRenderer glowSprite;
    [SerializeField] private float baseRadius = 1f;
    [SerializeField] private float radiusPerTorch = 0.5f;
    [SerializeField] private float baseAlpha = 0.2f;
    [SerializeField] private float alphaPerTorch = 0.15f;

    private int torchCount;

    public int TorchCount => torchCount;

    private void Start()
    {
        UpdateGlow();
    }

    public void AddTorch()
    {
        torchCount++;
        UpdateGlow();
    }

    public bool TryStealTorch()
    {
        if (torchCount <= 0) return false;
        torchCount--;
        UpdateGlow();
        return true;
    }

    private void UpdateGlow()
    {
        float radius = baseRadius + (torchCount * radiusPerTorch);
        glowSprite.transform.localScale = Vector3.one * radius;

        float alpha = Mathf.Clamp01(baseAlpha + (torchCount * alphaPerTorch));
        Color c = glowSprite.color;
        c.a = alpha;
        glowSprite.color = c;
    }
}