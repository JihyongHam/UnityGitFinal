using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] private TextMeshProUGUI enemyCountText;
    [SerializeField] private string enemyCountTextMessage = "Enemy Count: ";

    private int totalEnemiesSpawned = 0;

    private void Start()
    {
        UpdateEnemyCountText();
    }

    public void IncrementEnemyCount()
    {
        totalEnemiesSpawned++;
        UpdateEnemyCountText();
    }

    private void UpdateEnemyCountText()
    {
        if (enemyCountText != null)
        {
            enemyCountText.text = enemyCountTextMessage + totalEnemiesSpawned.ToString();
        }
    }
}
