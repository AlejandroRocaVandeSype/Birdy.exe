using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static VirusManager;

public class WindowPassword : WindowBasic
{
    [SerializeField] private InputField _passwordField;
    [SerializeField] private string _password;
    private string _correctPasswordUser = "123abc";       // User password
    private string _correctPasswordAdmin = "Garden1943";  // Admin password
    bool _firstPassword = true;

    [SerializeField] private GameObject _storyPrefab = null;

    [SerializeField] private Sprite _correctPasswordSprite = null;
    [SerializeField] private Sprite _wrongPasswordSprite = null;

    public void CheckPassword()
    {
        if (_passwordField != null)
        {
            
                if (_passwordField.text == _password)
                {
                    if(_password == _correctPasswordUser)
                     {
                        _base.GetComponent<SpriteRenderer>().sprite = _correctPasswordSprite;
                        // Correct password
                        CloseWindow();
                        GameManager.Instance.VirusManager.Stage = VirusStage.AntivirusStart;

                        Instantiate(_storyPrefab, GameManager.Instance.StorySpawnPosition.transform.position, Quaternion.identity);
                        _firstPassword = false;
                     }
                    else
                    {
                        // Wrong password
                        _base.GetComponent<SpriteRenderer>().sprite = _wrongPasswordSprite;
                     }
                   
                    
                }
                else
                {
                    // Wrong password
                    _base.GetComponent<SpriteRenderer>().sprite = _wrongPasswordSprite;
                }




            if (_passwordField.text == _password)
                {
                   if(_password == _correctPasswordAdmin)
                    {
                        // Correct password
                        CloseWindow();
                        GameManager.Instance.VirusManager.Stage = VirusStage.AntivirusStart;
                    }    
                   else
                    {
                    // Wrong password
                    _base.GetComponent<SpriteRenderer>().sprite = _wrongPasswordSprite;
                    }
                    
                }
            else
            {
                // Wrong password
                _base.GetComponent<SpriteRenderer>().sprite = _wrongPasswordSprite;
            }
            

        }
    }

    public void DeletePassword()
    {
        _passwordField.text = "";
    }


}
