using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerLight : MonoBehaviour
{
    [SerializeField] private Light2D playerLight;
    [SerializeField] private float baseRadius = 0f;
    [SerializeField] private float radiusPerTorch = 1.5f;
    [SerializeField] private float baseIntensity = 0.3f;
    [SerializeField] private float intensityPerTorch = 0.25f;

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
        if (!playerLight) return;
        playerLight.pointLightOuterRadius = baseRadius + (torchCount * radiusPerTorch);
        playerLight.intensity = baseIntensity + (torchCount * intensityPerTorch);
    }
}
