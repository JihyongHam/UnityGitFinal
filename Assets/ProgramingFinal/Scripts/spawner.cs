    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using TMPro;

    public class spawner : MonoBehaviour
    {
        [Header("Spawner Settings")]
        [SerializeField] private float spawnRate = 1f;
        [SerializeField] private bool canSpawn = false;
        [SerializeField] private bool spawnBossOrNot = true;
        [SerializeField] private int howManyEnemiesBeforeBoss = 1;

        [Header("" + "")]
        [SerializeField] private GameObject[] enemyPrefab;
        [SerializeField] private GameObject bossPrefab;


        private int enemiesSpawned = 1;


        [Header("" + "")]
        [Header("Gizmo Settings")]
        [SerializeField] private Color gizmoColor = Color.green;
        private float gizmoRadius = 1.0f;


        private void Start()
        {
            StartCoroutine(Spawner());
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                canSpawn = true; 
                StartCoroutine(Spawner());
            }
        }

        private IEnumerator Spawner()
        {
            WaitForSeconds wait = new WaitForSeconds(spawnRate);

            // call UIcontoroller class
            UIController uiControllerInstance = FindObjectOfType<UIController>();

            while (canSpawn)
            {
                yield return wait;

                int rand = Random.Range(0, enemyPrefab.Length);
                GameObject enemyToSpawn = enemyPrefab[rand];

                Instantiate(enemyToSpawn, transform.position, Quaternion.identity);

                enemiesSpawned++;

                // UIcontoroller
                if (uiControllerInstance != null)
                {
                    uiControllerInstance.IncrementEnemyCount();
                }


                if (enemiesSpawned >= howManyEnemiesBeforeBoss && spawnBossOrNot)
                {
                    yield return new WaitForSeconds(spawnRate);

                    Instantiate(bossPrefab, transform.position, Quaternion.identity);
 
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
