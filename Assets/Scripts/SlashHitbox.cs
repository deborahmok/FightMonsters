using System.Collections.Generic;
using UnityEngine;

public class SlashHitbox : MonoBehaviour
{
    [SerializeField] private float lifetime = 0.12f;

    private HashSet<int> alreadyHitIds = new HashSet<int>();

    private void OnEnable()
    {
        alreadyHitIds.Clear();
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponentInParent<Enemy>();
        if (enemy == null) return;

        int id = enemy.gameObject.GetInstanceID();
        if (alreadyHitIds.Contains(id)) return;

        alreadyHitIds.Add(id);
        enemy.TakeHit(1);
    }
}