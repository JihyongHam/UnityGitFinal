using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    // UI Settings
    [Header("UI Settings")]
    // Should assign enemycount texts on canvas
    [SerializeField] private TextMeshProUGUI enemyCountText;
    // Can adjust text message
    [SerializeField] private string enemyCountTextMessage = "Enemy Count: ";

    // Enemy counts for UI script
    private int totalEnemiesSpawned = 0;

    // count enemy spawning when game starts
    private void Start()
    {
        UpdateEnemyCountText();
    }

    // Increment enemy counts
    public void IncrementEnemyCount()
    {
        // totalEnemiesSpawned = totalEnemiesSpawned + 1;
        totalEnemiesSpawned++;

        // Update enemy counts
        UpdateEnemyCountText();
    }

    // Enemy counts fucntion
    private void UpdateEnemyCountText()
    {
        if (enemyCountText != null)
        {
            // Number UI changes only
            enemyCountText.text = enemyCountTextMessage + totalEnemiesSpawned.ToString();
        }
    }
}
