using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorageClick : MonoBehaviour
{
    private bool _wasClickedAlready = false;


    public void Awake()
    {
        _wasClickedAlready = false; 
    }

    public void OnStorageClick()
    {
        if (_wasClickedAlready == true)
            return;

        Instantiate(this.gameObject, Vector3.zero, Quaternion.identity);

        GameManager.Instance.SpawnManager.SpawnOne();

        _wasClickedAlready = true;

    }
}
