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
    [SerializeField] private int maxEnemiesEnemySpawn = 0;

    [Header("" + "")]
    [SerializeField] private GameObject[] enemyPrefab;
    [SerializeField] private GameObject[] bossPrefab;


    private int enemiesSpawned = 1;


    [Header("" + "")]
    [Header("Gizmo Settings")]
    [SerializeField] private Color gizmoColor = Color.green;
    private float gizmoRadius = 1.0f;

    [Header("" + "")]
    [Header("Music Settings")]
    [SerializeField] private AudioClip mainMusic;
    [SerializeField] private AudioClip bossMusic;
        
    private AudioSource audioSource;


    private void Start()
    {
        StartCoroutine(Spawner());

        audioSource = GetComponent<AudioSource>();
        PlayMainMusic();
    
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

        while (canSpawn && enemiesSpawned <= maxEnemiesEnemySpawn)
        {
            yield return wait;

            int randomOne = Random.Range(0, enemyPrefab.Length);
            GameObject enemyToSpawn = enemyPrefab[randomOne];

            Instantiate(enemyToSpawn, transform.position, Quaternion.identity);

            enemiesSpawned++;

            // UIcontoroller
            if (uiControllerInstance != null)
            {
                uiControllerInstance.IncrementEnemyCount();
            }


            if (enemiesSpawned >= howManyEnemiesBeforeBoss && spawnBossOrNot)
            {
                yield return wait;
                int randomTwo = Random.Range(0, bossPrefab.Length);
                GameObject bossToSpawn = bossPrefab[randomTwo];

                audioSource.Stop();

                Instantiate(bossToSpawn, transform.position, Quaternion.identity);

                if (audioSource != null && bossMusic != null)
                {
                        audioSource.clip = bossMusic;
                        audioSource.Play();
                }
 
                foreach (GameObject enemyInstance in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    canSpawn = false;
                    Destroy(enemyInstance);
                }
            }

        }
    }
    private void PlayMainMusic()
    {
        if (audioSource != null && mainMusic != null)
        {
            audioSource.clip = mainMusic;
            audioSource.Play(); 
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, gizmoRadius);
    }

}
