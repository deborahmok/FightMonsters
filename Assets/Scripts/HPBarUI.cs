using UnityEngine;
using UnityEngine.UI;

public class HPBarUI : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private PlayerState player;

    private void Awake()
    {
        if (fillImage == null)
            fillImage = GetComponent<Image>();
    }

    private void Update()
    {
        if (player == null || fillImage == null) return;

        float current = player.CurrentHP;
        float max = player.MaxHP;

        fillImage.fillAmount = (max <= 0f) ? 0f : Mathf.Clamp01(current / max);
    }
}