using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    private List<SpawnPoint> _spawnPoints = new List<SpawnPoint>();

    private bool _hasToSpawn = true;
    private int _amountToSpawn = 1;             // Spawn only one enemy at start
   
    public void RegisterSpawnPoint(SpawnPoint spawPoint)
    {
       AddSpawnPoint(ref _spawnPoints, spawPoint);
              
    }

    public void UnRegisterSpawnPoint(SpawnPoint spawPoint)
    {
        _spawnPoints.Remove(spawPoint);

    }

    private void AddSpawnPoint(ref List<SpawnPoint> spawnPoints, SpawnPoint spawnPoint)
    {
        if (!spawnPoints.Contains(spawnPoint))
        {
            spawnPoints.Add(spawnPoint); // Only if it is not already registered
        }
    }


    public void SpawnVirus()
    {

        foreach (SpawnPoint point in _spawnPoints)
        {
            //m_NrEnemiesAlive++;
            point.Spawn();
        }
    }



    // Update is called once per frame
    void Update()
    {
        // Remove any objects that are null
        _spawnPoints.RemoveAll(s => s == null);

        if(_hasToSpawn == true)
        {
            for(int spawnCount = 0; spawnCount < _amountToSpawn; ++spawnCount)
            {
                
                int spawnIdx = Random.Range(0, _spawnPoints.Count - 1);
                Debug.Log(spawnIdx);
                _spawnPoints[spawnIdx].Spawn();
            }

            _hasToSpawn= false;
        }
    }


    public void SpawnNewWave()
    {
        _hasToSpawn = true;
        _amountToSpawn++;
    }
}
