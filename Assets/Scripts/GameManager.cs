using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    // SINGLETON CLASS. IT WILL EXIST BETWEEN SCNENES
    #region SINGLETON
    private static GameManager m_Instance;

    public static GameManager Instance
    {
        get
        {
            if (m_Instance == null && !m_IsApplicationQuiting)
            {
                m_Instance = FindObjectOfType<GameManager>();
                if (m_Instance == null)
                {
                    // There was no instance found of this class. Create a new one
                    GameObject newGameManager = new GameObject("Singleton_GameManager");
                    m_Instance = newGameManager.AddComponent<GameManager>();
                }
            }

            return m_Instance;
        }
    }

    private static bool m_IsApplicationQuiting = false;
    public void OnApplicationQuit()
    {
        m_IsApplicationQuiting = true;
    }

    #endregion


    private SpawnManager _spawnManager = null;
    private VirusManager _virusManager = null;
    [SerializeField] private HealthBar _healthBar = null;
    [SerializeField] private GameObject _gameOverImage = null;
    [SerializeField] private GameObject _endScreenImg = null;
    [SerializeField] private MouseFollow _mouse = null;
    [SerializeField] private GameObject _spawnPosition;

    public enum GameStage { Intro, Gameplay, WaitToStart, GameOver, Wait, UserWin, EndScreen }
    private GameStage _gameStage;
    private GameStage _previousStage;   // When pause remember in which stage it was
    private GameObject _endScreenG0;

    // Start is called before the first frame update
    void Awake()
    {
        //DontDestroyOnLoad(gameObject);

        if (m_Instance == null)
        {
            m_Instance = this;
        }
        else
        {
            if (m_Instance != this)
            {
                //Duplicate. We only want one
                Destroy(gameObject);
            }
        }

        _spawnManager = GetComponent<SpawnManager>();
        _virusManager = GetComponent<VirusManager>();
        _gameStage = GameStage.Intro;
    }

    public void Update()
    {
        if (_virusManager == null || _spawnManager == null)
            return;


        switch (_gameStage)
        {
            case GameStage.Intro:
                _virusManager.StartVirus();
                break;
            case GameStage.Gameplay:
                _virusManager.UpdateVirus();
                break;
            case GameStage.GameOver:
                {
                    LoadGameOverScreen();
                    StartCoroutine(WaitSeconds(5f));
                    break;
                }

            case GameStage.UserWin:
                {
                    _virusManager.UserWin();
                    StartCoroutine(WaitSeconds(5f));
                    break;
                }
            case GameStage.EndScreen:
                {
                    if(_endScreenG0 == null)  // Save it so we can delete it in case is Pause game
                        _endScreenG0 = Instantiate(_endScreenImg, Vector3.zero, Quaternion.identity);                  
                    break;
                }
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {            
            PauseGame();
        }
    }

    private void PauseGame()
    {
        if (_gameStage != GameStage.EndScreen)
        {
            _previousStage = _gameStage;
            _gameStage = GameStage.EndScreen;
        }
        else
        {
            if (_endScreenG0 != null)
                Destroy(_endScreenG0);

            // Go back to the stage the player was
            _gameStage = _previousStage;
        }
    }

    private void LoadGameOverScreen()
    {
        _gameOverImage.SetActive(true);
    }

    private IEnumerator WaitSeconds(float amount)
    {
        yield return new WaitForSeconds(amount);
        _gameStage = GameStage.EndScreen;
    }

    public void CloseGame()
    {
        // Works in build but not on editor
        Application.Quit();
    }

    public void RestartGame(string sceneToRestart)
    {
        SceneManager.LoadScene(sceneToRestart);
    }

    public SpawnManager SpawnManager
    {
        get { return _spawnManager; }
    }

    public VirusManager VirusManager
    {
        get { return _virusManager; }
    }

    public HealthBar Storage
    {
        get { return _healthBar; }
    }

    public GameStage gameStage
    {
        get { return _gameStage; }
        set { _gameStage = value; }
    }

    public MouseFollow Mouse
    {
        get { return _mouse; }
    }

    public GameObject StorySpawnPosition
    {
        get { return _spawnPosition; }
    }

}
