using UnityEngine;

// Door.cs - Attach to door object (a gap in the wall)
public class Door : MonoBehaviour
{
    [SerializeField] private Collider2D doorBlocker; // Invisible collider that blocks when closed
    [SerializeField] private SpriteRenderer doorVisual; // Optional visual
    
    private bool isOpen = true;
    
    private void Start()
    {
        // Door starts OPEN - player can enter
        doorBlocker.enabled = false;
    }

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     // Player starts entering
    //     if (!isOpen) return;
    //     if (!other.CompareTag("Player")) return;
    //     Debug.Log("Player entering door");
    // }

    // private void OnTriggerExit2D(Collider2D other)
    // {
    //     // Player fully passed through - NOW lock
    //     if (!isOpen) return;
    //     if (!other.CompareTag("Player")) return;
        
    //     Debug.Log("Player exited door trigger - locking room");
    //     GetComponentInParent<RoomLogic>()?.LockRoom();
    // }
    
    public void Open()
    {
        Debug.Log($"Door Opened");
        isOpen = true;
        doorBlocker.enabled = false;
        // Update visual
    }
    
    public void Close()
    {
        Debug.Log($"Door Closed");
        isOpen = false;
        doorBlocker.enabled = true;
        // Update visual
    }
}
