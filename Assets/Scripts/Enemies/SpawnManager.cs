using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    private List<SpawnPoint> _spawnPoints = new List<SpawnPoint>();
    private bool _hasToSpawn = true;
    private int _amountToSpawn = 1;             // Spawn only one enemy at start
  
   private List<GameObject> _viruses = new List<GameObject>();
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


    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.gameStage != GameManager.GameStage.Start &&
            GameManager.Instance.gameStage != GameManager.GameStage.WaitToStart)
        {
            // Remove any objects that are null
            _spawnPoints.RemoveAll(s => s == null);

            if (_hasToSpawn == true)
            {
                for (int spawnCount = 0; spawnCount < _amountToSpawn; ++spawnCount)
                {

                    int spawnIdx = Random.Range(0, _spawnPoints.Count - 1);
                    _viruses.Add(_spawnPoints[spawnIdx].Spawn());
                }

                _hasToSpawn = false;
                _amountToSpawn = 2;
            }
        }     
    }



    public void SpawnOne()
    {
        int spawnIdx = Random.Range(0, _spawnPoints.Count - 1);
       _viruses.Add( _spawnPoints[spawnIdx].Spawn());
    }

    public void SpawnNewWave()
    {
        _hasToSpawn = true;
    }


    public void KillAll()
    {
        foreach(var viruses in _viruses)
        {
            if(viruses != null)
                viruses.GetComponent<VirtusBehaviour>().Kill(true);
        }
        _viruses.Clear();
    }
}
