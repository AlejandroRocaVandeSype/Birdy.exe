using UnityEngine;
using UnityEngine.UI;
using static VirusManager;

public class WindowPassword : WindowBasic
{
    enum PasswordType { User, Admin };
    [SerializeField] private PasswordType _passwordType;
    [SerializeField] private InputField _inputPassword;
    [SerializeField] private string _correctPassword;

    [SerializeField] private GameObject _storyPrefab = null;
    [SerializeField] private Sprite _correctPasswordSprite = null;
    [SerializeField] private Sprite _wrongPasswordSprite = null;

    public void CheckPassword()
    {
        if (_inputPassword == null)
            return;

       if(_inputPassword.text == _correctPassword)
        {
            _base.GetComponent<SpriteRenderer>().sprite = _correctPasswordSprite;
            //Correct password
            CloseWindow();
            GameManager.Instance.VirusManager.Stage = VirusStage.AntivirusStart;

            if(_passwordType == PasswordType.User)
                Instantiate(_storyPrefab, GameManager.Instance.StorySpawnPosition.transform.position, Quaternion.identity);
        }
       else
        {
            // Wrong password
            _base.GetComponent<SpriteRenderer>().sprite = _wrongPasswordSprite;
        }

    }


    // Empty the Text field
    public void DeletePassword()
    {
        _inputPassword.text = "";
    }


}
