using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{
    // Spawner settings
    [Header("Spawner Settings")]
    [SerializeField] private float spawnRate = 1f;
    [SerializeField] private bool canSpawn = false;
    [SerializeField] private bool spawnBossOrNot = true;
    [SerializeField] private int howManyEnemiesBeforeBoss = 1;
    [SerializeField] private int maxEnemySpawn = 0;

    // Enemy sort Settings
    [Header("" + "")]
    [SerializeField] private GameObject[] enemyPrefab;
    [SerializeField] private GameObject[] bossPrefab;


    // Enemy counts for spawner script
    private int enemiesSpawned = 1;


    // Gizmo settings
    [Header("" + "")]
    [Header("Gizmo Settings")]
    [SerializeField] private Color gizmoColor = Color.green;
    [SerializeField] private float gizmoRadius = 1.0f;


    // Music settings
    [Header("" + "")]
    [Header("Music Settings")]
    [SerializeField] private AudioClip mainMusic;
    [SerializeField] private AudioClip bossMusic;
        
    private AudioSource audioSource;


    private void Start()
    {
        // spawning starts
        StartCoroutine(Spawner());

        // music starts
        audioSource = GetComponent<AudioSource>();
        PlayMusic();
    
    }

    // Trigger Volume
    private void OnTriggerEnter(Collider other)
    {
        // Trigger when player enter
        if (other.CompareTag("Player"))
        {   
            // canSpawn should check to false to use trigger volume
            canSpawn = true;
            // spawning starts for trigger volume
            StartCoroutine(Spawner());
        }
    }

    // IEnumerator spawner method
    private IEnumerator Spawner()
    {
        // wait for second based on spawnRate assigned by me for next enemy spawn
        WaitForSeconds wait = new WaitForSeconds(spawnRate);

        // call UIcontoroller class
        UIController uiControllerInstance = FindObjectOfType<UIController>();

        // looping condition canSpawn should be turned on
        while (canSpawn && enemiesSpawned <= maxEnemySpawn)
        {
            yield return wait;

            // randomizing enemies spawning
            int randomOne = Random.Range(0, enemyPrefab.Length);
            GameObject enemyToSpawn = enemyPrefab[randomOne];

            // enemy spawns
            Instantiate(enemyToSpawn, transform.position, Quaternion.identity);

            // enemiesSpawned = enemiesSpawned + 1, which is caculating
            enemiesSpawned++;

            // About UIcontoroller script
            if (uiControllerInstance != null)
            {
                // call IncrementEnemyCount() script inside of the UIController
                uiControllerInstance.IncrementEnemyCount();
            }

            // looping condition spawnBossOrNot should be turned on
            if (enemiesSpawned >= howManyEnemiesBeforeBoss && spawnBossOrNot)
            {
                yield return wait;

                // randomizing enemies spawning
                int randomTwo = Random.Range(0, bossPrefab.Length);
                GameObject bossToSpawn = bossPrefab[randomTwo];

                // audio should stop to get new audio for boss
                audioSource.Stop();

                // boss spawns
                Instantiate(bossToSpawn, transform.position, Quaternion.identity);

                // play boss music
                if (audioSource != null && bossMusic != null)
                {
                        audioSource.clip = bossMusic;
                        audioSource.Play();
                }
                
                // when boss appears, all the enemies are removed
                foreach (GameObject enemyInstance in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    // to avoid additional respawns for enemies
                    canSpawn = false;
                    
                    // destory enmies taged with Enemy
                    Destroy(enemyInstance);
                }
            }

        }
    }

    // play music function
    private void PlayMusic()
    {
        // main music play
        if (audioSource != null && mainMusic != null)
        {
            audioSource.clip = mainMusic;
            audioSource.Play(); 
        }
    }


    // gizmo function
    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, gizmoRadius);
    }

}
