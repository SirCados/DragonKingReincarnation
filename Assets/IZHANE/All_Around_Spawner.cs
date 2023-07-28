using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class All_Around_Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject orcPrefab;

    [SerializeField]
    private GameObject pigPrefab;

    [SerializeField]
    private float orcInterval = 10f; 

    [SerializeField]
    private float pigInterval = 3.5f;

    public float valueX = 100;
    public float valueY = 100;

    void Start()
    {        
    StartCoroutine(spawnEnemy(orcInterval, orcPrefab));
    StartCoroutine(spawnEnemy(pigInterval, orcPrefab));
    }

    private IEnumerator spawnEnemy(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);        
        Vector3 spawnLocation = new Vector3(Random.Range(-valueX, valueX), Random.Range(-valueY, valueY), 0);
        GameObject newEnemy = Instantiate(enemy, spawnLocation, Quaternion.identity);
        StartCoroutine(spawnEnemy(interval, enemy));
    }
    void Update()
    {
        
    }
}
