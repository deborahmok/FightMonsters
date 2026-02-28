using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("Anchors")]
    [SerializeField] private Transform enemySpawnParent;
    [SerializeField] private Transform lootSpawnParent;
    [SerializeField] private GameObject treasureObject;
    [SerializeField] private RoomLogic roomLogic;

    [Header("Waves")]
    [SerializeField] private int totalEnemiesToSpawn = 12;  // total over time
    [SerializeField] private int minWaveSize = 4;
    [SerializeField] private int maxWaveSize = 6;

    [Header("Enemy Behavior")]
    [SerializeField] private float activationStagger = 0.25f;
    [SerializeField] private int maxAggroAtOnce = 2;
    [SerializeField] private float aggroUpdateInterval = 0.5f;

    public List<Transform> EnemySpawns { get; private set; } = new();
    public List<Transform> LootSpawns { get; private set; } = new();

    private readonly List<GameObject> spawnedLoot = new();
    private readonly List<Enemy> enemyScripts = new();

    private GameManager gm;
    private Coroutine aggroRoutine;

    private int spawnedSoFar = 0;
    public int AliveEnemies { get; private set; } = 0;
    private bool roomStarted = false;

    // private void Start()
    // {
    //     OnPlayerEnteredRoom();
    // }
    
    private void Awake()
    {
        EnemySpawns = CollectChildren(enemySpawnParent);
        LootSpawns = CollectChildren(lootSpawnParent);
        gm = FindObjectOfType<GameManager>();
    }

    private List<Transform> CollectChildren(Transform parent)
    {
        List<Transform> list = new();
        if (parent == null) return list;
        for (int i = 0; i < parent.childCount; i++)
            list.Add(parent.GetChild(i));
        return list;
    }

    // Called by RoomLogic when player enters
    public void OnPlayerEnteredRoom()
    {
        if (roomStarted) return;
        roomStarted = true;

        spawnedSoFar = 0;

        if (aggroRoutine != null) StopCoroutine(aggroRoutine);
        aggroRoutine = StartCoroutine(AggroManager());

        SpawnNextWave();
    }

    private void SpawnNextWave()
    {
        if (gm == null) return;

        int remaining = totalEnemiesToSpawn - spawnedSoFar;
        if (remaining <= 0)
        {
            // Room is fully done
            UnlockTreasure();
            roomLogic?.UnlockRoom();
            return;
        }

        int waveSize = Random.Range(minWaveSize, maxWaveSize + 1);
        waveSize = Mathf.Min(waveSize, remaining);
        waveSize = Mathf.Min(waveSize, EnemySpawns.Count);

        List<Enemy> newWave = gm.SpawnEnemiesForRoom(this, waveSize);

        spawnedSoFar += waveSize;

        StartCoroutine(ActivateWaveStaggered(newWave));

        Debug.Log($"{name} spawned wave {waveSize}. SpawnedSoFar={spawnedSoFar}/{totalEnemiesToSpawn}");
    }

    private IEnumerator ActivateWaveStaggered(List<Enemy> wave)
    {
        float delay = 0f;
        foreach (Enemy e in wave)
        {
            if (e != null)
            {
                yield return new WaitForSeconds(delay);
                e.Activate();
                delay += activationStagger;
            }
        }
    }

    private IEnumerator AggroManager()
    {
        while (true)
        {
            GameObject pObj = GameObject.FindGameObjectWithTag("Player");
            if (pObj == null)
            {
                yield return new WaitForSeconds(aggroUpdateInterval);
                continue;
            }

            Transform p = pObj.transform;

            // consider only active + alive enemies
            List<Enemy> alive = new List<Enemy>();
            foreach (Enemy e in enemyScripts)
            {
                if (e == null) continue;
                e.SetAggro(false);
                alive.Add(e);
            }

            alive.Sort((a, b) =>
            {
                float da = Vector2.Distance(a.transform.position, p.position);
                float db = Vector2.Distance(b.transform.position, p.position);
                return da.CompareTo(db);
            });

            for (int i = 0; i < Mathf.Min(maxAggroAtOnce, alive.Count); i++)
                alive[i].SetAggro(true);

            yield return new WaitForSeconds(aggroUpdateInterval);
        }
    }

    // Called by GameManager when it spawns an enemy
    public void RegisterEnemy(Enemy e)
    {
        if (e == null) return;
        enemyScripts.Add(e);
        AliveEnemies++;
    }

    // Called by Enemy.Die()
    public void NotifyEnemyDied()
    {
        AliveEnemies = Mathf.Max(0, AliveEnemies - 1);

        if (AliveEnemies == 0)
        {
            // wave cleared
            if (spawnedSoFar < totalEnemiesToSpawn)
                SpawnNextWave();
            else
            {
                UnlockTreasure();
                roomLogic?.UnlockRoom();
            }
        }
    }

    public void RegisterLoot(GameObject loot)
    {
        if (loot == null) return;
        spawnedLoot.Add(loot);
    }

    public void ClearLootOnly()
    {
        foreach (var l in spawnedLoot)
            if (l != null) Destroy(l);
        spawnedLoot.Clear();
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