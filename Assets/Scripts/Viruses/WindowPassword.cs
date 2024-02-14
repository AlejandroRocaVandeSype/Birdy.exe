using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
            GameManager.Instance.gameStage = GameManager.GameStage.UserWin;
        }

    }

    public void DeletePassword()
    {
        _passwordField.text = "";
    }


}
