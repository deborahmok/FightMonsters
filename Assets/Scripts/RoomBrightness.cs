using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal; // For Light2D

public class RoomBrightness : MonoBehaviour
{
    [SerializeField] private SpriteRenderer darkOverlay;  // Black semi-transparent sprite covering room

    public bool IsLit { get; private set; }
    private int activeTorchCount;

    private void Awake()
    {
        SetDark();
    }

    public void RegisterTorch()
    {
        activeTorchCount++;
        if (activeTorchCount == 1) SetLit();
    }

    public void UnregisterTorch()
    {
        activeTorchCount--;
        if (activeTorchCount <= 0)
        {
            activeTorchCount = 0;
            SetDark();
        }
    }

    private void SetLit()
    {
        IsLit = true;
        if (darkOverlay) darkOverlay.enabled = false;
    }

    private void SetDark()
    {
        IsLit = false;
        if (darkOverlay) darkOverlay.enabled = true;
    }
}
