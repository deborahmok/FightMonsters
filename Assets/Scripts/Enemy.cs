using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int maxHP = 1;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float wanderSpeed = 1.2f;
    [SerializeField] private float wanderChangeInterval = 1.0f;

    private Vector2 wanderDir = Vector2.zero;
    private float nextWanderChangeTime = 0f;

    public bool IsAggro { get; private set; } = false;

    private int currentHP;
    private Room myRoom;
    private bool isActive = false;

    private Transform player;
    private Rigidbody2D rb;

    private void Awake()
    {
        currentHP = maxHP;
        rb = GetComponent<Rigidbody2D>();

        GameObject pObj = GameObject.FindGameObjectWithTag("Player");
        player = (pObj != null) ? pObj.transform : null;
    }

    public void Activate()
    {
        isActive = true;
    }

    public void Initialize(Room room)
    {
        isActive = false;
        rb.linearVelocity = Vector2.zero;
        myRoom = room;
    }

    public void TakeHit(int damage = 1)
    {
        currentHP -= damage;
        if (currentHP <= 0) Die();
    }

    private void FixedUpdate()
    {
        if (!isActive)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (player == null)
        {
            GameObject pObj = GameObject.FindGameObjectWithTag("Player");
            if (pObj != null) player = pObj.transform;
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (IsAggro)
        {
            Vector2 dir = ((Vector2)player.position - rb.position).normalized;
            rb.linearVelocity = dir * moveSpeed;
        }
        else
        {
            UpdateWander();
            rb.linearVelocity = wanderDir * wanderSpeed;
        }
    }

    private void Die()
    {
        if (myRoom != null)
            myRoom.NotifyEnemyDied();

        Destroy(gameObject);
    }

    public void SetAggro(bool aggro)
    {
        IsAggro = aggro;
    }

    private void UpdateWander()
    {
        if (Time.time >= nextWanderChangeTime)
        {
            wanderDir = Random.insideUnitCircle.normalized;
            nextWanderChangeTime = Time.time + wanderChangeInterval;
        }
    }
}