using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Variables
    private static GameManager _i;
    public static GameManager i { get { return _i; } }
    [SerializeField] private Transform sysMessagePoint;
    [SerializeField] private Transform spawnPoint;
    private Map map;
    private bool isPaused;


    #endregion
    
    #region Initialize
    private void Awake() 
    {
        _i = this;  
        Initialize();
    }

    private void Initialize() 
    {
        CreateMap();
    }

    private void CreateMap()
    {
        map = new Map();
    }
    #endregion

    public void PauseGame(){if(isPaused) return; else isPaused = true;}
    public void UnPauseGame(){if(isPaused) isPaused = false; else return;}
    
    public Transform GetSysMessagePoint(){ return sysMessagePoint;}
    public Map GetMap() { return map; }
    public bool GetIsPaused() { return isPaused; }

}
