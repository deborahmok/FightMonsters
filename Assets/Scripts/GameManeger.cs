using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Room> rooms = new();

    private void Start()
    {
        Debug.Log("Rooms found: " + rooms.Count);
        foreach (Room room in rooms)
        {
            Debug.Log($"{room.name}: EnemySpawns={room.EnemySpawns.Count}, LootSpawns={room.LootSpawns.Count}");
        }
    }
}