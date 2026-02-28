using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("Anchors")]
    [SerializeField] private Transform enemySpawnParent;
    [SerializeField] private Transform lootSpawnParent;
    [SerializeField] private Transform torchSpawnParent;   // optional
    [SerializeField] private Transform treasureSpot;
    [SerializeField] private GameObject treasureObject;
    [SerializeField] private RoomLogic roomLogic;
    
    public List<Transform> EnemySpawns { get; private set; } = new();
    public List<Transform> LootSpawns { get; private set; } = new();
    public List<Transform> TorchSpawns { get; private set; } = new();

    public Transform TreasureSpot => treasureSpot;

    // Track spawned objects so we can clear them if needed
    private readonly List<GameObject> spawnedEnemies = new();
    private readonly List<GameObject> spawnedLoot = new();

    public int AliveEnemies { get; private set; } = 0;

    private void Awake()
    {
        EnemySpawns = CollectChildren(enemySpawnParent);
        LootSpawns  = CollectChildren(lootSpawnParent);
        TorchSpawns = CollectChildren(torchSpawnParent);
    }

    private List<Transform> CollectChildren(Transform parent)
    {
        List<Transform> list = new();
        if (parent == null) return list;
        for (int i = 0; i < parent.childCount; i++)
            list.Add(parent.GetChild(i));
        return list;
    }

    public void RegisterEnemy(GameObject enemy)
    {
        if (enemy == null) return;
        spawnedEnemies.Add(enemy);
        AliveEnemies++;
    }

    public void RegisterLoot(GameObject loot)
    {
        if (loot == null) return;
        spawnedLoot.Add(loot);
    }

    public void ClearSpawns()
    {
        foreach (var e in spawnedEnemies)
            if (e != null) Destroy(e);
        foreach (var l in spawnedLoot)
            if (l != null) Destroy(l);

        spawnedEnemies.Clear();
        spawnedLoot.Clear();
        AliveEnemies = 0;
    }
    
    public void NotifyEnemyDied()
    {
        AliveEnemies = Mathf.Max(0, AliveEnemies - 1);
        Debug.Log($"{name} enemy died. AliveEnemies now: {AliveEnemies}");

        if (AliveEnemies == 0)
        {
            Debug.Log($"{name} cleared! Unlocking treasure.");
            UnlockTreasure();
            roomLogic?.UnlockRoom();
        }
    }
    
    public void LockTreasure()
    {
        if (treasureObject != null)
            treasureObject.SetActive(false);
    }

    public void UnlockTreasure()
    {
        if (treasureObject != null)
            treasureObject.SetActive(true);
    }
}