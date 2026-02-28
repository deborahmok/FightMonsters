using UnityEngine;

public class RoomEntryZone : MonoBehaviour
{
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;
        GetComponentInParent<RoomLogic>()?.LockRoom();
        
        // Optional: destroy this trigger since it's one-time use
        // gameObject.SetActive(false);
    }
}