using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Random_Enemy_Spawner : MonoBehaviour
{
    //IZHANE
    [SerializeField] private float spawnRate = 1f;

    [SerializeField] private GameObject[] enemyPrefabs;

    [SerializeField] private bool canSpawn = true;

    public Transform[] spawnPoint;

    //CADE
    GameObject enemyToSpawn;
    public float valueX = 1000;
    public float valueY = 1000;

    //IZHANE
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
            int rand = Random.Range(0, enemyPrefabs.Length);
            GameObject enemyToSpawn = enemyPrefabs[rand];

            Instantiate(enemyToSpawn, transform.position, Quaternion.identity);
        }
    }

    //CADE
    void SpawnEnemy()
    {
        float randomXPosition = Random.Range(-valueX, valueX);
        float randomYPosition = Random.Range(-valueY, valueY);
        Vector3 spawnPosition = new Vector3(randomXPosition, randomYPosition, 0);
        Instantiate(enemyToSpawn, spawnPosition, new Quaternion());
    }
    //IZHANE
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            int randEnemy = Random.Range(0, enemyPrefabs.Length);
            int randSpawnPoint = Random.Range(0, spawnPoint.Length);

            Instantiate(enemyPrefabs[randEnemy], spawnPoint[randSpawnPoint].position, transform.rotation);
        }
    }
}

