using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("Anchors")]
    [SerializeField] private Transform enemySpawnParent;
    [SerializeField] private Transform lootSpawnParent;
    [SerializeField] private Transform torchSpawnParent;   // optional
    [SerializeField] private Transform treasureSpot;

    public List<Transform> EnemySpawns { get; private set; } = new();
    public List<Transform> LootSpawns { get; private set; } = new();
    public List<Transform> TorchSpawns { get; private set; } = new();

    public Transform TreasureSpot => treasureSpot;

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
}