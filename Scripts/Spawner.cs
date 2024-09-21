using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    [SerializeField] private float minDistance = 5f;
    [SerializeField] private float maxDistance = 10f;
    [SerializeField] private float spawnDelay = 2;
    [SerializeField] private float scale = 1.1f;


    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnDelay);
            Vector2 randomCircle = UnityEngine.Random.insideUnitCircle.normalized * UnityEngine.Random.Range(minDistance, maxDistance);
            Vector3 spawnPosition = new Vector3(transform.position.x + randomCircle.x, transform.position.y, transform.position.z + randomCircle.y);
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            
        }
    }
}
