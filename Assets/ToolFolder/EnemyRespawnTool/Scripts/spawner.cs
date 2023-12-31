using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class spawner : MonoBehaviour
{
    // Spawner settings
    [Header("Spawner Settings")]
    [SerializeField] private bool canSpawn = false;
    [SerializeField] private float spawnRate = 1f;
    [SerializeField] private int maxEnemySpawn = 0;

    [Header("" + "")]
    [SerializeField] private bool spawnBossOrNot = true;
    [SerializeField] private int howManyEnemiesBeforeBoss = 0;

    [Header("" + "")]
    [SerializeField] private bool removeEnemiesForBoss = true;

    // Enemy and Boss type settings
    [Header("" + "")]
    [SerializeField] private GameObject[] enemyPrefab;
    [SerializeField] private GameObject[] bossPrefab;


    // Enemy counts for spawner script
    private int enemiesSpawned = 0;

    // Event System
    [Header("" + "")]
    [Header("Boss and Trigger Volume Event Settings")]
    public UnityEvent eventSettings;


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
    [SerializeField] private bool keepMainMusic = false;
        
    private AudioSource audioSource;

    private void Start()
    {
        // spawning starts
        StartCoroutine(Spawner());

        // music starts
        audioSource = GetComponent<AudioSource>();
        PlayMainMusic();
    
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

            // event for trigger
            eventSettings.Invoke();
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

                // boss spawns
                Instantiate(bossToSpawn, transform.position, Quaternion.identity);

                // event for boss
                eventSettings.Invoke();

                // keep main music or not
                if (keepMainMusic == false)
                {
                    // play boss music
                    PlayBossMusic();
                }
                

                // when boss appears, all the enemies are removed
                foreach (GameObject enemyInstance in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    // to avoid additional respawns for enemies
                    canSpawn = false;
                    
                    // when this bool sets to true
                    if (removeEnemiesForBoss == true)
                    {
                        // destory enmies taged with Enemy
                        Destroy(enemyInstance);
                    }
                    
                }

            }

        }
    }

    // play main music function
    private void PlayMainMusic()
    {
        // main music play
        if (audioSource != null)
        {
            audioSource.clip = mainMusic;
            audioSource.Play(); 
        }
    }

    // play boss music function
    private void PlayBossMusic()
    {
        // boss music play
        if (audioSource != null)
        {
            audioSource.clip = bossMusic;
            audioSource.Play();
        }
    }

    // If you want to stop playing music after you kill boss. You can call this function. You can just type "StopMusic();" after you set up the condition.
    public void StopPlayingMusic()
    {
        audioSource.Stop();
    }

    // gizmo function
    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, gizmoRadius);
    }

}
