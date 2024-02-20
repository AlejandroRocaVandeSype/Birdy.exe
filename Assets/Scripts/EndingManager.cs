using UnityEngine;

public class EndingManager : MonoBehaviour
{
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    public void CloseGame()
    {
        if(_gameManager != null ) 
            _gameManager.CloseGame();
    }

    public void RestartGame()
    {
        if (_gameManager != null)
            _gameManager.RestartGame("WindowsPlay");
    }

    public void RestartFromFirstScene()
    {
        if (_gameManager != null)
            _gameManager.RestartGame("Intro");
    }
}
