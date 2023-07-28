using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Random_Spawner : MonoBehaviour
{

    public Transform[] spawnPoint;
    public GameObject[] enemyPrefabs;


   
    void Start()
    {
        
    }

    
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            int randEnemy = Random.Range(0, enemyPrefabs.Length);
            int randSpawnPoint = Random.Range(0,spawnPoint.Length);

            Instantiate(enemyPrefabs[randEnemy], spawnPoint[randSpawnPoint].position, transform.rotation);
        }
    }
}
