using UnityEngine;

// RoomLogic.cs - Attach to room parent object
public class RoomLogic : MonoBehaviour
{
    [SerializeField] private Door door;
    [SerializeField] private Collider2D[] wallColliders; // 4 walls
    
    private bool isLocked = false;
    private bool isCleared = false;
    
    private void Start()
    {
        UnlockRoom();
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