using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Torch : MonoBehaviour
{
    [SerializeField] private SpriteRenderer glowSprite;
    [SerializeField] private float glowRadius = 3f;

    private RoomBrightness parentRoom;

    private void Start()
    {
        if (glowSprite) 
            glowSprite.transform.localScale = Vector3.one * glowRadius;

        parentRoom = GetComponentInParent<RoomBrightness>();
        parentRoom?.RegisterTorch();
    }

    private void OnDestroy()
    {
        parentRoom?.UnregisterTorch();
    }
}