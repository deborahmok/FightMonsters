using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int maxHP = 1;
    private int currentHP;

    private Room myRoom;

    private void Awake()
    {
        currentHP = maxHP;
        Debug.Log($"{name} spawned with maxHP={maxHP}");
    }

    // Called right after spawning
    public void Initialize(Room room)
    {
        myRoom = room;
    }

    // Called by slash hitbox
    public void TakeHit(int damage = 1)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (myRoom != null)
            myRoom.NotifyEnemyDied();

        Destroy(gameObject);
    }
}