using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static event Action OnAllEnemiesRemoved;
    [SerializeField] private GameObject spawnLocation_North;
    [SerializeField] private GameObject spawnLocation_South;
    [SerializeField] private GameObject spawnLocation_East;
    [SerializeField] private GameObject spawnLocation_West;
    [SerializeField] private float timeBetweenSpawns;

    private int xMin, xMax, yMin, yMax;
    private float spawnTimer;
    private bool canSpawn=false;
    private bool staticY, staticX;

    private GameObject currentSpawnLocation, lastSpawnLocation;
    private Collider2D currentSpawnCollider;
    private Vector2 actualSpawnPoint;
    private List<EnemyHandler> spawnedEnemies;

    private void OnEnable() 
    {
        GameManager.OnBaseSpawned += Initialize; 
        EnemyHandler.OnEnemyDeath += RemoveEnemyFromList;   
        DayNightSystem.OnCycleComplete += StopSpawning;
        DayNightSystem.OnNightStarted += StartSpawning;
    }
    private void Disable() 
    {
        GameManager.OnBaseSpawned -= Initialize;  
        EnemyHandler.OnEnemyDeath -= RemoveEnemyFromList;  
        DayNightSystem.OnCycleComplete -= StopSpawning;
        DayNightSystem.OnNightStarted -= StartSpawning;
    }

    private void Initialize()
    {
        canSpawn = false;
    }

    private void StartSpawning()
    {
        spawnedEnemies = new List<EnemyHandler>();
        canSpawn = true;
    }
    private void Update() 
    {
        if(!canSpawn) return;

        if(spawnTimer <= 0) SpawnEnemy();
        else spawnTimer -= Time.deltaTime;    
    }
    private void ChooseSpawnRegion()
    {
        int randomLocation = UnityEngine.Random.Range(0,4);
        if (randomLocation == 0) {currentSpawnLocation = spawnLocation_North; staticY = true; staticX = false;}
        if (randomLocation == 1) {currentSpawnLocation = spawnLocation_South; staticY = true; staticX = false;}
        if (randomLocation == 2) {currentSpawnLocation = spawnLocation_East; staticY = false; staticX = true;}
        if (randomLocation == 3) {currentSpawnLocation = spawnLocation_West; staticY = false; staticX = true;}

        if(lastSpawnLocation == currentSpawnLocation && currentSpawnLocation != null) ChooseSpawnRegion();
        lastSpawnLocation = currentSpawnLocation;
    }

    private void ChooseSpawnLocation()
    {
        currentSpawnCollider = currentSpawnLocation.GetComponent<Collider2D>();
        if(staticX)
        {
            float randomY = UnityEngine.Random.Range(-currentSpawnCollider.bounds.size.y/2, currentSpawnCollider.bounds.size.y/2);
            actualSpawnPoint = new Vector2(currentSpawnLocation.transform.position.x, randomY);
        }
        if(staticY)
        {
            float randomX = UnityEngine.Random.Range(-currentSpawnCollider.bounds.size.x/2, currentSpawnCollider.bounds.size.x/2);
            actualSpawnPoint = new Vector2( randomX, currentSpawnLocation.transform.position.y);
        }
    }

    private void SpawnEnemy()
    {
        EnemyHandler newEnemy = ObjectPooler.DequeueObject<EnemyHandler>("Enemy");
        ChooseSpawnRegion();
        ChooseSpawnLocation();
        newEnemy.transform.position = actualSpawnPoint;
        newEnemy.gameObject.SetActive(true);
        newEnemy.Initialize();
        spawnedEnemies.Add(newEnemy);

        spawnTimer = timeBetweenSpawns;
    }

    private void RemoveEnemyFromList(EnemyHandler _handler)
    {
        foreach(EnemyHandler enemyHandler in spawnedEnemies)
        {
            if (enemyHandler == _handler)
            {
                spawnedEnemies.Remove(enemyHandler);
                if(spawnedEnemies.Count == 0) OnAllEnemiesRemoved?.Invoke();
                return;
            }
        }
    }

    private void StopSpawning(bool _isDaytime){if(!_isDaytime) canSpawn = false;}
}
