using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static VirusManager;

public class WindowPassword : WindowBasic
{
    [SerializeField] private InputField _passwordField;
    [SerializeField] private string _password;
    private string _correctPasswordUser = "123abc456abc";       // User password
    private string _correctPasswordAdmin = "GoodieGarden1943";  // Admin password
    bool _firstPassword = true;

    [SerializeField] private GameObject _storyPrefab = null;

    public void CheckPassword()
    {
        if (_passwordField != null)
        {
            
                if (_passwordField.text == _password  && _password == _correctPasswordUser)
                {
                    // Correct password
                    CloseWindow();
                    GameManager.Instance.VirusManager.Stage = VirusStage.AntivirusStart;

                    Instantiate(_storyPrefab, GameManager.Instance.StorySpawnPosition.transform.position, Quaternion.identity);
                _firstPassword = false;
                }


            
                if (_passwordField.text == _password && _password == _correctPasswordAdmin)
                {
                    // Correct password
                    CloseWindow();
                    GameManager.Instance.VirusManager.Stage = VirusStage.AntivirusStart;
                }
            

        }
    }

    public void DeletePassword()
    {
        _passwordField.text = "";
    }


}
