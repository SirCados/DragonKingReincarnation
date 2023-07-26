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


    void Start()
    {
    StartCoroutine(spawnEnemy(orcInterval, orcPrefab));
    StartCoroutine(spawnEnemy(pigInterval, orcPrefab));
    }

    private IEnumerator spawnEnemy(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);
        GameObject newEnemy = Instantiate(enemy, new Vector3(Random.Range(-5f,5), Random.Range(-6f, 6f), 0), Quaternion.identity);
        StartCoroutine(spawnEnemy(interval, enemy));
    }
    void Update()
    {
        
    }
}
