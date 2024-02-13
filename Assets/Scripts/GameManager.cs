using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public enum GameStage { Menu, FirstScene, GamePlay }
    private GameStage _gameStage;

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);

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
        _gameStage = GameStage.GamePlay;
    }


    public SpawnManager SpawnManager
    {
        get { return _spawnManager; }
    }

    public VirusManager VirusManager
    {
        get { return _virusManager; }
    }

}
