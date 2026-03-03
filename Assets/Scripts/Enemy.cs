using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int maxHP = 1;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float wanderSpeed = 1.2f;
    [SerializeField] private float wanderChangeInterval = 1.0f;

    [Header("Phase / Visuals")]
    [SerializeField] private bool shrinkOnFirstHit = false; // enable for type 2
    [SerializeField] private float shrinkScaleFactor = 0.5f;

    private Vector2 wanderDir = Vector2.zero;
    private float nextWanderChangeTime = 0f;

    public bool IsAggro { get; private set; } = false;

    private int currentHP;
    private Room myRoom;
    private bool isActive = false;

    private Transform player;
    private Rigidbody2D rb;

    private Vector3 originalScale;
    private bool hasShrunk = false;

    private void Awake()
    {
        currentHP = maxHP;
        rb = GetComponent<Rigidbody2D>();

        originalScale = transform.localScale;

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

        currentHP = maxHP;
        transform.localScale = originalScale;
        hasShrunk = false;
    }

    public void TakeHit(int damage = 1)
    {
        int previousHP = currentHP;
        currentHP -= damage;

        if (shrinkOnFirstHit && !hasShrunk)
        {
            // "phase change" = went from full HP to something less, but still > 0
            bool wasFullHP = (previousHP == maxHP);
            bool stillAlive = (currentHP > 0);

            if (wasFullHP && stillAlive)
            {
                transform.localScale = originalScale * shrinkScaleFactor;
                hasShrunk = true;
            }
        }
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