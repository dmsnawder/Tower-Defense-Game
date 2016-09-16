using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

[System.Serializable]
public class Wave
{
    public GameObject[] enemies;
    private float spawnInterval;
    public float SpawnInterval
    {
        get { return spawnInterval; }
        set { spawnInterval = value; }
    }

    private int maxEnemies;
    public int MaxEnemies
    {
        get { return maxEnemies; }
        set { maxEnemies = value; }
    }

    public Wave(GameObject[] enemyPrefabs, float spawninterval, int maxenemies)
    {
        enemies = new GameObject[enemyPrefabs.Length];
        Array.Copy(enemyPrefabs, enemies, enemyPrefabs.Length);
        spawnInterval = spawninterval;
        maxEnemies = maxenemies;
    }
}

public class SpawnEnemies : MonoBehaviour {

    public GameObject[] waypoints;
    public GameObject[] enemyPrefabs;
    public List<Wave> waves = new List<Wave>();
    public float spawnInterval = 5;
    public float spawnIntervalMin = 4f;
    public float spawnIntervalMax = 6f;
    public int maxEnemies = 6;
    public AudioClip bossEnterSound;

    private bool haveSpawnedBoss = false;
    private int dawn = 6;

    public int introTime = 3;
    public int timeBetweenWaves = 5;
    public GameManager gameManager;
    public  float lastSpawnTime;
    private int enemiesSpawned = 0;
    public int currentWave = 0;
    public bool startSpawning = false;
    public float timeInterval;

    public bool canSpawn = true;

	// Use this for initialization
	void Start () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        waves.Add(new Wave(enemyPrefabs, spawnInterval, maxEnemies));

        lastSpawnTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {

        if (canSpawn)
        {
            if (!gameManager.nightSurvived)
            {
                if (gameManager.ClockTime < dawn || gameManager.ClockTime == 12)
                {
                    timeInterval = Time.time - lastSpawnTime;

                    // Set 3 seconds for the intro of the level to fade in before 
                    // starting the timeBetweenWaves countdown
                    if (timeInterval > introTime && !startSpawning)
                    {
                        startSpawning = true;
                        lastSpawnTime = Time.time;
                    }

                    if (startSpawning && ((enemiesSpawned == 0 && timeInterval > timeBetweenWaves) || timeInterval > spawnInterval) &&
                        enemiesSpawned < waves[currentWave].MaxEnemies)
                    {
                        //spawnInterval = Random.Range(spawnIntervalMin, spawnIntervalMax);

                        lastSpawnTime = Time.time;
                        GameObject newEnemy;

                        if (gameManager.Night == 1 && currentWave <= 2)
                        {
                            newEnemy = Instantiate(waves[currentWave].enemies[0]);
                        }
                        else if (gameManager.ClockTime <= 1 || gameManager.ClockTime == 12)
                        {
                            int enemyIndex = Random.Range(0, 2);
                            newEnemy = Instantiate(waves[currentWave].enemies[enemyIndex]);
                        }
                        else if (gameManager.ClockTime <= 2)
                        {
                            int enemyIndex = Random.Range(0, 3);
                            newEnemy = Instantiate(waves[currentWave].enemies[enemyIndex]);
                        }
                        else
                        {
                            // If it is 5 AM and the boss enemy hasn't been spawned already,
                            // send out the boss enemy for one wave.
                            if (gameManager.Night != 1 && gameManager.ClockTime == 5 && !haveSpawnedBoss)
                            {
                                haveSpawnedBoss = true;
                                newEnemy = Instantiate(waves[currentWave].enemies[4]);
                                AudioSource.PlayClipAtPoint(bossEnterSound, transform.position);
                            }
                            // otherwise do what you been doin'
                            else
                            {
                                int enemyIndex = ReturnRandomEnemy();
                                newEnemy = Instantiate(waves[currentWave].enemies[enemyIndex]);
                            }
                        }

                        newEnemy.GetComponent<MoveEnemy>().waypoints = waypoints;
                        enemiesSpawned++;

                        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
                        audioSource.PlayOneShot(audioSource.clip);
                    }

                    if (enemiesSpawned == waves[currentWave].MaxEnemies && GameObject.FindGameObjectWithTag("Enemy") == null)
                    {
                        currentWave++;
                        enemiesSpawned = 0;

                        //spawnIntervalMin -= 0.2f; //Random.Range(0.1f, 0.3f);
                        //spawnIntervalMax -= 0.2f; //Random.Range(0.1f, 0.3f);
                        //if (spawnIntervalMin < 0.2f) spawnIntervalMin = 0.2f;
                        //if (spawnIntervalMax < 0.8f) spawnIntervalMax = 0.8f;
                        spawnInterval -= Random.Range(0.1f, 0.2f);
                        if (spawnInterval < 0.5f) spawnInterval = 0.5f;

                        maxEnemies += Random.Range(1, 4);
                        waves.Add(new Wave(enemyPrefabs, spawnInterval, maxEnemies));

                        lastSpawnTime = Time.time;
                    }
                }
                else
                {
                    gameManager.nightSurvived = true;
                    ClearGameBoard();
                }
            }
        }
	}

    void ClearGameBoard()
    {
        GameObject[] currentEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (currentEnemies != null)
        {
            for (int i = 0; i < currentEnemies.Length; i++)
            {
                Destroy(currentEnemies[i]);
            }
        }
        // TODO: play audio clip for all enemies poofing away at dawn

    }

    int ReturnRandomEnemy()
    {
        int retVal = Random.Range(0, 101);
        int enemyIndex = 0;

        if (retVal < 97 - currentWave)
        {
            enemyIndex = Random.Range(0, 3);
        }
        else
        {
            enemyIndex = 3;
        }

        return enemyIndex;
    }

}
