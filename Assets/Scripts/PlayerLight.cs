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
    // private int torchCount;
    // public int TorchCount => torchCount;
    
    private void Awake()
    {
        if (playerLight == null)
            playerLight = GetComponentInChildren<Light2D>();

        playerState = GetComponent<PlayerState>();

        if (playerLight == null || playerState == null)
        {
            Debug.LogError("PlayerLight: missing Light2D or PlayerState reference.");
            enabled = false;
            return;
        }
        int startingTorches = playerState.Torches;

        baseRadius = playerLight.pointLightOuterRadius + (startingTorches * radiusPerTorch);
        baseIntensity = playerLight.intensity + (startingTorches * intensityPerTorch);

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
}
