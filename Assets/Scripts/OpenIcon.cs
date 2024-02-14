using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class OpenIcon : MonoBehaviour
{
    [SerializeField] private GameObject _windowToOpen = null;
    GameObject _window = null;
    public void OpenWindow()
    {
        if (_windowToOpen != null && _window == null)
        {
            _window = Instantiate(_windowToOpen, Vector3.zero, Quaternion.identity);
        }
    }
}
