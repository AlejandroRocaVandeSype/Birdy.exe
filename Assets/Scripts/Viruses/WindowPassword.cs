using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static VirusManager;

public class WindowPassword : WindowBasic
{
    [SerializeField] private InputField _passwordField;
    private string _correctPassword = "123abc456abc";  // Admin password
    
    public void CheckPassword()
    {
        if(_passwordField != null && _passwordField.text == _correctPassword)
        {
            // Correct password
            CloseWindow();
            GameManager.Instance.VirusManager.Stage = VirusStage.DestroyWindows;
        }

    }

    public void DeletePassword()
    {
        _passwordField.text = "";
    }


}
