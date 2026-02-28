using UnityEngine;

public class TreasurePickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerState>() == null) return;

        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null)
            gm.CollectTreasure();

        Destroy(gameObject);
    }
}
