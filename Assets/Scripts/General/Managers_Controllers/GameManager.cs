using UnityEngine;
using System;


public class GameManager : MonoBehaviour
{
    public static event Action OnBaseSpawned;

    #region Variables
    private static GameManager _i;
    public static GameManager i { get { return _i; } }
    [SerializeField] private Transform sysMessagePoint;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform playerPool;
    [SerializeField] private Transform enemyPool;
    private GameObject baseGO;
    private bool isPaused;
    #endregion
    
    #region Initialize
    private void OnDisable() 
    {
        EnemySpawner.OnAllEnemiesRemoved -= StartDayCycle;    
    }
    private void Awake() 
    {
        _i = this;  
        Initialize();
        SetupObjectPools();
    }

    private void Initialize() 
    {
        baseGO = Instantiate(GameAssets.i.pfbaseGO, spawnPoint.position, Quaternion.identity);
        baseGO.GetComponent<BaseHandler>().InitializePlayer(); 
        OnBaseSpawned?.Invoke(); 
        UIController.i.Initialize();
        EnemySpawner.OnAllEnemiesRemoved += StartDayCycle;
        DayNightSystem.i.Initialize();
    }

    public void SetupObjectPools()
    {
        ObjectPooler.SetupPool(GameAssets.i.pfPlayerProjectile.GetComponent<PlayerProjectile>(), 20, "Player Projectile");
        ObjectPooler.SetupPool(GameAssets.i.pfEnemyProjectile.GetComponent<EnemyProjectile>(), 20, "Enemy Projectile");
        ObjectPooler.SetupPool(GameAssets.i.pfEnemy.GetComponent<EnemyHandler>(), 20, "Enemy");
    }

    private void StartDayCycle()
    {
        DayNightSystem.i.InitializeDay();
    }
    #endregion

    public void PauseGame(){if(isPaused) return; else isPaused = true;}
    public void UnPauseGame(){if(isPaused) isPaused = false; else return;}
    public GameObject GetBaseGO(){return baseGO;}
    public Transform GetSysMessagePoint(){ return sysMessagePoint;}
    public Transform GetPoolLocation(string _key){if(_key == "Enemy") return enemyPool; else return playerPool;}
    public bool GetIsPaused() { return isPaused; }

}
