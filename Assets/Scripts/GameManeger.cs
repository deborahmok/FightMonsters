using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Rooms")]
    [SerializeField] private List<Room> rooms = new();

    [Header("Prefabs")]
    [SerializeField] private GameObject enemy1Prefab;
    [SerializeField] private GameObject enemy2Prefab;
    [SerializeField] private GameObject hpLootPrefab;
    [SerializeField] private GameObject torchPrefab;

    [Header("Scene Parents")]
    [SerializeField] private Transform enemiesParent;
    [SerializeField] private Transform pickupsParent;
    [SerializeField] private Transform torchesParent;

    [Header("Enemy Counts (per room)")]
    [SerializeField] private int minEnemiesPerRoom = 2;
    [SerializeField] private int maxEnemiesPerRoom = 5;

    [Header("Loot Counts (per room)")]
    [SerializeField] private int minLootPerRoom = 1;
    [SerializeField] private int maxLootPerRoom = 3;

    [Header("Torch Counts (per room)")]
    [SerializeField] private int minTorchesPerRoom = 1;
    [SerializeField] private int maxTorchesPerRoom = 3;

    [Header("Enemy Type Weights")]
    [Range(0f, 1f)]
    [SerializeField] private float enemy1Chance = 0.7f; // Enemy2 chance is (1 - enemy1Chance)

    [SerializeField] private int totalTreasuresToWin = 3;
    private int treasuresCollected = 0;
    
    private void Start()
    {
        Debug.Log("Rooms found: " + rooms.Count);

        foreach (Room room in rooms)
        {
            room.LockTreasure();
            room.ClearSpawns();
            GenerateRoomContents(room);
            Debug.Log($"{room.name}: EnemySpawns={room.EnemySpawns.Count}, LootSpawns={room.LootSpawns.Count}, SpawnedEnemies={room.AliveEnemies}");
        }
    }

    private void GenerateRoomContents(Room room)
    {
        if (room == null) return;

        // --- Spawn Enemies ---
        int enemyCount = Random.Range(minEnemiesPerRoom, maxEnemiesPerRoom + 1);
        enemyCount = Mathf.Min(enemyCount, room.EnemySpawns.Count); // don't exceed available points

        List<Transform> enemyPoints = new List<Transform>(room.EnemySpawns);
        Shuffle(enemyPoints);

        for (int i = 0; i < enemyCount; i++)
        {
            Transform spawn = enemyPoints[i];
            GameObject prefab = PickEnemyPrefab();
            if (prefab == null) continue;

            GameObject enemy = Instantiate(prefab, spawn.position, Quaternion.identity, enemiesParent);
            room.RegisterEnemy(enemy);
            
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.Initialize(room);
            }
        }

        // --- Spawn Loot (HP for now) ---
        int lootCount = Random.Range(minLootPerRoom, maxLootPerRoom + 1);
        lootCount = Mathf.Min(lootCount, room.LootSpawns.Count);

        List<Transform> lootPoints = new List<Transform>(room.LootSpawns);
        Shuffle(lootPoints);

        for (int i = 0; i < lootCount; i++)
        {
            Transform spawn = lootPoints[i];
            if (hpLootPrefab == null) continue;

            GameObject loot = Instantiate(hpLootPrefab, spawn.position, Quaternion.identity, pickupsParent);
            room.RegisterLoot(loot);
        }

        // --- Spawn Torches ---
        if (torchPrefab == null || room.TorchSpawns.Count == 0) return;

        int torchCount = Random.Range(minTorchesPerRoom, maxTorchesPerRoom + 1);
        torchCount = Mathf.Min(torchCount, room.TorchSpawns.Count);

        List<Transform> torchPoints = new List<Transform>(room.TorchSpawns);
        Shuffle(torchPoints);

        for (int i = 0; i < torchCount; i++)
        {
            Transform spawn = torchPoints[i];
            Instantiate(torchPrefab, spawn.position, Quaternion.identity, torchesParent);
        }
    }

    private GameObject PickEnemyPrefab()
    {
        if (enemy1Prefab == null && enemy2Prefab == null) return null;
        if (enemy1Prefab == null) return enemy2Prefab;
        if (enemy2Prefab == null) return enemy1Prefab;

        float roll = Random.value;
        return (roll < enemy1Chance) ? enemy1Prefab : enemy2Prefab;
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int j = Random.Range(i, list.Count);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
    
    public void CollectTreasure()
    {
        treasuresCollected++;
        Debug.Log($"Treasure collected: {treasuresCollected}/{totalTreasuresToWin}");

        if (treasuresCollected >= totalTreasuresToWin)
        {
            Debug.Log("YOU WIN!");
            // Later: show win UI, freeze controls, etc.
        }
    }
}