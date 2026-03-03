using TMPro;
using UnityEngine;

public class TreasureUI : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private TMP_Text treasureText;

    private void Update()
    {
        if (gameManager == null || treasureText == null) return;

        int collected = gameManager.GetTreasuresCollected();
        int total = gameManager.GetTotalTreasures();

        treasureText.text = "Treasure collected " + collected + " / " + total;
    }
}