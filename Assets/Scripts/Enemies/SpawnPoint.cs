using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{

    [SerializeField] private GameObject _virusTemplate = null;
    private SpawnManager _spawnManager = null;

    private void Start()
    {
        _spawnManager = GameManager.Instance.SpawnManager;

        if (_spawnManager != null)
            _spawnManager.RegisterSpawnPoint(this);

    }

    private void OnDisable()
    {
        if (_spawnManager != null)
            _spawnManager.UnRegisterSpawnPoint(this);
    }

    public GameObject Spawn()
    {
        return Instantiate(_virusTemplate, transform.position, transform.rotation);
    }

    public GameObject VirusTemplate()
    { 
        return _virusTemplate; 
    }

}
