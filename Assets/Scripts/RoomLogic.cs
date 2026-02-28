using UnityEngine;

public class RoomLogic : MonoBehaviour
{
    [SerializeField] private Door door;
    [SerializeField] private Room room;

    private bool isLocked = false;
    private bool isCleared = false;

    private void Start()
    {
        // Start open, but NOT cleared
        isLocked = false;
        isCleared = false;
        door.Open();
    }

    public void OnPlayerEnteredRoom()
    {
        if (isCleared) return;   // if already cleared, don't relock
        if (room == null) return;
        LockRoom();
        room?.OnPlayerEnteredRoom(); // <-- starts waves + enemy behavior
    }

    public void LockRoom()
    {
        isLocked = true;
        door.Close();
    }

    public void UnlockRoom()
    {
        isLocked = false;
        isCleared = true;
        door.Open();
    }

    public bool IsCleared => isCleared;
}