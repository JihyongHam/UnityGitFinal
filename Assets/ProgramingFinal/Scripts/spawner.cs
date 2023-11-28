using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private float spawnRate = 1f;
    [SerializeField] private bool canSpawn = true;
    [SerializeField] private GameObject[] enemyPrefab;
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private bool spawnBossOrNot = true;
    [SerializeField] private int enemiesSpawnBeforeBoss = 1;

    private int enemiesSpawned = 1;

    [Header("Gizmo Settings")]
    [SerializeField] private Color gizmoColor = Color.clear;
    private float gizmoRadius = 1.0f;


    private void Start()
    {
        StartCoroutine(Spawner());
    }

    private IEnumerator Spawner()
    {
        WaitForSeconds wait = new WaitForSeconds(spawnRate);

        while (canSpawn)
        {
            yield return wait;

            int rand = Random.Range(0, enemyPrefab.Length);
            GameObject enemyToSpawn = enemyPrefab[rand];

            Instantiate(enemyToSpawn, transform.position, Quaternion.identity);

            enemiesSpawned++;

            if (enemiesSpawned >= enemiesSpawnBeforeBoss && spawnBossOrNot)
            {
                yield return new WaitForSeconds(spawnRate);

                Instantiate(bossPrefab, transform.position, Quaternion.identity);

                // can be removed after setting up the UI
                enemiesSpawned = 0;

                foreach (GameObject enemyInstance in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    canSpawn = false;
                    Destroy(enemyInstance);
                }
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, gizmoRadius);
    }
}
