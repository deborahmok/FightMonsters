using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerLight : MonoBehaviour
{
    
    [SerializeField] private Light2D playerLight;
    private PlayerState playerState;

    // [SerializeField] private float baseRadius = 0f;
    [SerializeField] private float radiusPerTorch = 1.5f;
    // [SerializeField] private float baseIntensity = 0.3f;
    [SerializeField] private float intensityPerTorch = 0.25f;

    private float baseRadius;
    private float baseIntensity;
    public float CurrentRadius => playerLight != null ? playerLight.pointLightOuterRadius : 0f;    // public int TorchCount => torchCount;
    
    private void Awake()
    {
        if (playerLight == null)
            playerLight = GetComponent<Light2D>();

        playerState = GetComponent<PlayerState>();

        if (playerLight == null || playerState == null)
        {
            Debug.LogError("PlayerLight: missing Light2D or PlayerState reference.");
            enabled = false;
            return;
        }
        int startingTorches = playerState.Torches;

        // baseRadius = playerLight.pointLightOuterRadius + (startingTorches * radiusPerTorch);
        // baseIntensity = playerLight.intensity + (startingTorches * intensityPerTorch);
        baseRadius = playerLight.pointLightOuterRadius;
        baseIntensity = playerLight.intensity;
        
        playerLight.pointLightOuterRadius = baseRadius;
        playerLight.intensity = baseIntensity;

        // UpdateGlow();
    }

    private void Start()
    {
        // Apply correct brightness for starting torch count
        UpdateGlow();
    }

    public void AddTorch(int amount = 1)
    {
        playerState.AddTorches(amount);
        UpdateGlow();
    }

    public bool TryStealTorch(int amount = 1)
    {
        bool success = playerState.TrySpendTorch(amount);
        if (success) UpdateGlow();
        return success;
    }

    private void UpdateGlow()
    {
        // if (!playerLight) return;
        int torchCount = playerState.Torches;
        playerLight.pointLightOuterRadius = baseRadius + (torchCount * radiusPerTorch);
        playerLight.intensity = baseIntensity + (torchCount * intensityPerTorch);
    }

    void Update()
    {
        // Keep radius/intensity synced to torch count
        int torchCount = playerState.Torches;

        float targetRadius = baseRadius + torchCount * radiusPerTorch;
        float targetIntensity = baseIntensity + torchCount * intensityPerTorch;

        // Flicker on top of target intensity
        float flicker = (Mathf.PerlinNoise(Time.time * 3f, 0f) - 0.5f) * 2f * 0.2f; // -0.2..+0.2
        playerLight.pointLightOuterRadius = targetRadius;
        playerLight.intensity = Mathf.Max(0f, targetIntensity + flicker);
    }
}
