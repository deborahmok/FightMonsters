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
    [SerializeField] private int minEnemiesPerRoom = 3;
    [SerializeField] private int maxEnemiesPerRoom = 6;

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

    // -------- NEW: Dynamic HP Loot spawning --------
    [Header("Dynamic HP Loot Spawning")]
    [SerializeField] private bool enableDynamicHpLootSpawn = true;

    [Tooltip("When HP is LOW, wait about this many seconds between spawn attempts.")]
    [SerializeField] private float lootSpawnIntervalLowHP = 3f;

    [Tooltip("When HP is HIGH, wait about this many seconds between spawn attempts.")]
    [SerializeField] private float lootSpawnIntervalHighHP = 12f;

    [Tooltip("Spawn chance per attempt when HP is HIGH (0-1).")]
    [Range(0f, 1f)]
    [SerializeField] private float lootSpawnChanceHighHP = 0.15f;

    [Tooltip("Spawn chance per attempt when HP is LOW (0-1).")]
    [Range(0f, 1f)]
    [SerializeField] private float lootSpawnChanceLowHP = 0.85f;

    [Tooltip("How close we check for an existing HP loot to avoid stacking spawns.")]
    [SerializeField] private float lootOverlapRadius = 0.2f;

    [Header("Player Reference (for HP weighting)")]
    [SerializeField] private PlayerState player;

    [Header("Dynamic Loot Lifetime Budget")]
    [Tooltip("Max number of dynamically spawned HP loot in this run.")]
    [SerializeField] private int maxDynamicLootEver = 10;

    private int dynamicLootSpawnedTotal = 0;
    // -----------------------------------------------

    public int GetTreasuresCollected()
    {
        return treasuresCollected;
    }

    public int GetTotalTreasures()
    {
        return totalTreasuresToWin;
    }

    private void Awake()
    {
        // For safety, auto-find PlayerState if not wired in Inspector
        if (player == null)
            player = FindObjectOfType<PlayerState>();
    }

    private void Start()
    {
        Debug.Log("Rooms found: " + rooms.Count);

        foreach (Room room in rooms)
        {
            room.LockTreasure();
            GenerateRoomContents(room);
            Debug.Log($"{room.name}: EnemySpawns={room.EnemySpawns.Count}, LootSpawns={room.LootSpawns.Count}, SpawnedEnemies={room.AliveEnemies}");
        }

        if (enableDynamicHpLootSpawn)
            StartCoroutine(DynamicHpLootSpawnLoop());
    }

    private void GenerateRoomContents(Room room)
    {
        if (room == null) return;

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
            // If you want Room to track these, you could call room.RegisterLoot(loot) here
        }

        // --- Spawn Torches (UNCHANGED from original) ---
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

    public List<Enemy> SpawnEnemiesForRoom(Room room, int count)
    {
        List<Enemy> spawned = new List<Enemy>();
        if (room == null) return spawned;

        List<Transform> points = new List<Transform>(room.EnemySpawns);
        Shuffle(points);

        for (int i = 0; i < count; i++)
        {
            Transform spawn = points[i % points.Count];
            GameObject prefab = PickEnemyPrefab();
            if (prefab == null) continue;

            GameObject enemyObj = Instantiate(prefab, spawn.position, Quaternion.identity, enemiesParent);

            Enemy enemy = enemyObj.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.Initialize(room);      // sets room reference
                room.RegisterEnemy(enemy);   // counts + tracking
                spawned.Add(enemy);
            }
        }

        return spawned;
    }

    // --------------- Dynamic HP Loot logic ---------------

    private System.Collections.IEnumerator DynamicHpLootSpawnLoop()
    {
        while (true)
        {
            if (!enableDynamicHpLootSpawn || dynamicLootSpawnedTotal >= maxDynamicLootEver)
                yield break;

            float wait = GetNextLootSpawnInterval();
            yield return new WaitForSeconds(wait);

            TrySpawnHpLootWeightedByHP();
        }
    }

    private float GetNextLootSpawnInterval()
    {
        float hp01 = GetPlayerHP01();
        // Low HP => smaller wait (more frequent). High HP => bigger wait (less frequent).
        return Mathf.Lerp(lootSpawnIntervalLowHP, lootSpawnIntervalHighHP, hp01);
    }

    private void TrySpawnHpLootWeightedByHP()
    {
        if (!enableDynamicHpLootSpawn) return;
        if (dynamicLootSpawnedTotal >= maxDynamicLootEver) return;

        if (hpLootPrefab == null) return;
        if (rooms == null || rooms.Count == 0) return;

        float hp01 = GetPlayerHP01();
        float chance = Mathf.Lerp(lootSpawnChanceLowHP, lootSpawnChanceHighHP, hp01); // low HP => high chance
        if (Random.value > chance) return;

        // Pick a random room that has LootSpawns
        List<Room> validRooms = new List<Room>();
        foreach (var r in rooms)
        {
            if (r != null && r.LootSpawns != null && r.LootSpawns.Count > 0)
                validRooms.Add(r);
        }
        if (validRooms.Count == 0) return;

        Room room = validRooms[Random.Range(0, validRooms.Count)];
        Transform spawn = room.LootSpawns[Random.Range(0, room.LootSpawns.Count)];
        if (spawn == null) return;

        // Prevent stacking HP loot on top of itself (simple radius check)
        Collider2D hit = Physics2D.OverlapCircle(spawn.position, lootOverlapRadius);
        if (hit != null)
        {
            // If you have a specific tag/layer for HP loot, you can refine this:
            // if (hit.CompareTag("HPLoot")) return;
            // For now, just skip if *anything* is sitting exactly there.
            return;
        }

        Transform parent = (pickupsParent != null) ? pickupsParent : null;
        Instantiate(hpLootPrefab, spawn.position, Quaternion.identity, parent);
        dynamicLootSpawnedTotal++;
    }

    private float GetPlayerHP01()
    {
        if (player == null) return 1f;

        float max = player.MaxHP;
        float cur = player.CurrentHP;

        if (max <= 0f) return 1f;
        return Mathf.Clamp01(cur / max);
    }
}