using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class OpenIcon : MonoBehaviour
{
    [SerializeField] private GameObject _windowToOpen = null;
    [SerializeField] private GameObject _storyPrefabNotepad = null;
    GameObject _window = null;

    bool _alreadyOpened = false;
    public void OpenWindow()
    {
        if (_windowToOpen != null && _window == null )
        {
            _window = Instantiate(_windowToOpen, Vector3.zero, Quaternion.identity);

            if(_alreadyOpened == false)
            {
                _alreadyOpened = true;

                Instantiate(_storyPrefabNotepad, GameManager.Instance.StorySpawnPosition.transform.position, Quaternion.identity);
            }
        }
    }
}
