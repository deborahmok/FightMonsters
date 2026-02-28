using UnityEngine;
using UnityEngine.InputSystem;

public class TorchSpawnTest : MonoBehaviour
{
    [SerializeField] private GameObject torchPrefab;
    [SerializeField] private Transform spawnPoint;  // Drag a TorchSpawn point here

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.tKey.wasPressedThisFrame)
        {
            Debug.Log("Spawning torch...");
            Instantiate(torchPrefab, spawnPoint.position, Quaternion.identity, spawnPoint.parent);
        }
    }
}