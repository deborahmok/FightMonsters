using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Torch : MonoBehaviour
{
    [SerializeField] private Light2D torchLight;
    [SerializeField] private float glowRadius = 4f;
    [SerializeField] private float intensity = 0.8f;

    private void Start()
    {
        if (torchLight)
        {
            torchLight.pointLightOuterRadius = glowRadius;
            torchLight.intensity = intensity;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerLight player))
        {
            player.AddTorch(1);
            Destroy(gameObject);
        }
    }
}
