using System.Collections.Generic;
using UnityEngine;


public class VirusManager : MonoBehaviour
{
    public enum VirusStage { Wait, Intro, FirstVirus, LaunchingAntivirus, MultipleErrors, PasswordPhase, 
        AntivirusStart,DestroyWindows, WaitSeconds, VirusEnd };
    private VirusStage _virusStage;

    private GameManager _gameManager = null;

    [SerializeField] private GameObject _storyPrefab = null;
    [SerializeField] private GameObject _storySpawnPoint = null;

    [SerializeField] private GameObject _windowErrorTemplate = null;
    [SerializeField] private GameObject _windowPasswordTemplateUser = null;
    [SerializeField] private GameObject _windowPasswordTemplateAdmin = null;
    [SerializeField] private List<GameObject> _windowPopUps = new List<GameObject>();
    private const int MAX_ERRORS = 9;
    private int _errorWindowsCount = 0;
    private int _windowPopUpCount = 0;
    private float _windowPopUpSeconds;
    private float _maxWindowPopUp = 0f;
    private float _fastWindowPopUps = 0f;
    private float _maxFastWindowPopUp = 0.3f;
    private bool _isDone = false;

    [SerializeField] private List<GameObject> _windowPositions = new List<GameObject>();
    [SerializeField] private List<GameObject> _firstWindowPos = new List<GameObject>();

    // Keep track of all windows from the game
    private List<GameObject> _windowsInGame = new List<GameObject>();

    private float _secondsDestroyWindows = 0f;
    private float _maxSecondsDestroyWindows = 0.5f;

    [SerializeField] private GameObject _deathAnimationTemplate = null;

    [SerializeField] private List <GameObject> _antivirusLaunchingImgs = new List<GameObject> {};
    private float _currentSecAntivirus = 0f;
    private int _currentImageAntivirus = -1;
    private float timePerImage = 0f;

    private VirusStage _stageToChange = VirusStage.VirusEnd;
    private float _secondsToWait = 0f;                      // Seconds to wait in the WaitSeconds
    private float _currentSeconds = 0f;


    private float _antivirusMaxSeconds = 0f;
    private float _currentSecondsAntivirus = 0f;

    [SerializeField] private List<GameObject> _antivirusImages = new List<GameObject>();


    private int _limitAntivirus = 4;
    private int _currentIndexAntivirus = 1;
    bool _firstTimeAntivirus = true;
    bool _doOnce = false;

    bool _playMusic = true;

    int _passwordVersion = 1;   // User at start

    // Start is called before the first frame update
    void Awake()
    {
        _virusStage = VirusStage.Intro;
    }

    private void Start()
    {
        _gameManager = GameManager.Instance;
    }

    public void StartVirus()
    {
        // Pop Up a single window error in the middle of the screen
        CreateWindow(_windowErrorTemplate, Vector3.zero);
        _gameManager.gameStage = GameManager.GameStage.WaitToStart;
        _virusStage = VirusStage.FirstVirus;
        _secondsToWait = 5f;
    }

    public void UpdateVirus()
    {
        if (_gameManager == null)
            return;

        if (_gameManager.gameStage == GameManager.GameStage.Gameplay)
        {
            if (_virusStage != VirusStage.Wait && _virusStage != VirusStage.Intro
           && _playMusic)
            {
                SoundManager.Instance.PlaySound("Music", false);
                _playMusic = false;
            }

            switch (_virusStage)
            {
                case VirusStage.LaunchingAntivirus:
                    float totalTime = 30f;
                    LaunchingAntivirus(totalTime);
                    break;

                case VirusStage.PasswordPhase:
                    WindowPasswordDraw(_passwordVersion);
                    break;

                case VirusStage.AntivirusStart:
                    StartAntivirus();
                    break;

                case VirusStage.MultipleErrors:
                    WindowsErrorFase();
                    break;

                case VirusStage.DestroyWindows:
                    DestroyActiveWindows();
                    break;

                case VirusStage.VirusEnd:
                    _gameManager.gameStage = GameManager.GameStage.UserWin;
                    break;

                case VirusStage.WaitSeconds:
                    WaitSeconds();
                    break;

            }


            if (_virusStage == VirusStage.PasswordPhase || _virusStage == VirusStage.AntivirusStart
                      || _virusStage == VirusStage.LaunchingAntivirus || _virusStage == VirusStage.Wait)
            {
                RandomWindowPopUps();
            }

        }
    }
    public void VirusUpdate()
    {

        // Always popUp Windows 
        if ( _virusStage != VirusStage.FirstVirus && _virusStage != VirusStage.VirusEnd
            && _virusStage != VirusStage.MultipleErrors && _virusStage != VirusStage.DestroyWindows
            && _virusStage != VirusStage.WaitSeconds && _virusStage != VirusStage.WaitSeconds)
        {
            RandomWindowPopUps();
        }

        switch (_virusStage)
        {
            case VirusStage.LaunchingAntivirus:
                //LaunchingAntivirus(_secondsForPassword);
                break;
            case VirusStage.MultipleErrors:
                WindowsErrorFase();
                break;

            case VirusStage.PasswordPhase:
                {
                    WindowPasswordDraw(1);
                    break;
                }
            case VirusStage.AntivirusStart:
                {
                    StartAntivirus();
                    break;
                }
            case VirusStage.DestroyWindows:
                {
                    DestroyActiveWindows();
                    break;
                }
            case VirusStage.WaitSeconds:
                {
                    WaitSeconds();
                    break;
                }
            case VirusStage.VirusEnd:
                {
                    GameManager.Instance.gameStage = GameManager.GameStage.UserWin;
                    break;
                }


        }
    }


    private void LaunchingAntivirus(float totalTime)
    {
        // First image will inmediatly appear
        _currentSecAntivirus += Time.deltaTime;
        if(_currentSecAntivirus > timePerImage)
        {
            _currentSecAntivirus = 0f;
            _currentImageAntivirus++;

            int previousImag = _currentImageAntivirus - 1;
            if (previousImag >= 0)
            {
                // Stop showing previous one
                _antivirusLaunchingImgs[previousImag].SetActive(false);
            }

            if (_currentImageAntivirus < _antivirusLaunchingImgs.Count)
            {
                _antivirusLaunchingImgs[_currentImageAntivirus].SetActive(true);
            }
            else
            {
                _virusStage = VirusStage.PasswordPhase;  // Show password
            }

        }

        // Time each image will be shown based on the available total time
        timePerImage = totalTime / _antivirusLaunchingImgs.Count - 1;
    }

    private void StartAntivirus()
    {
        DestroyActiveWindows();

        _currentSecondsAntivirus += Time.deltaTime;
        if(_currentSecondsAntivirus > _antivirusMaxSeconds)
        {

            if (_antivirusMaxSeconds == 0f)
                _antivirusMaxSeconds = 3f;

                if (_currentIndexAntivirus <= _limitAntivirus)
                {
                    if(_currentIndexAntivirus == 4 && _firstTimeAntivirus == true)  // Error Antivirus
                    {
                        _antivirusImages[1].SetActive(false);
                        _antivirusImages[2].SetActive(false);
                        _antivirusImages[3].SetActive(false);
                        _antivirusImages[4].SetActive(false);
                        _antivirusImages[0].SetActive(true);
                        _virusStage = VirusStage.WaitSeconds;
                        _stageToChange = VirusStage.MultipleErrors;
                        _secondsToWait = 3f;
                        _firstTimeAntivirus = false;
                    _currentIndexAntivirus = 1;
                    _antivirusMaxSeconds = 0f;
                    _limitAntivirus = _antivirusImages.Count;

                    }
                    else if (_currentIndexAntivirus == _antivirusImages.Count && _firstTimeAntivirus == false)
                    {
                    // Win
                        for (int index = 1; index <= _antivirusImages.Count - 2; ++index)
                        {
                            _antivirusImages[index].SetActive(false);
                        }

                        _virusStage = VirusStage.WaitSeconds;
                        _stageToChange = VirusStage.VirusEnd;
                        _secondsToWait = 3f;
                    }
                    else
                    {
                        // Not reached yet
                        _antivirusImages[_currentIndexAntivirus -1].SetActive(false);
                        _antivirusImages[_currentIndexAntivirus].SetActive(true);
                    }
                }


            _currentIndexAntivirus++;
            _currentSecondsAntivirus = 0f;
        }
    }

    private void WaitSeconds()
    {
        _currentSeconds += Time.deltaTime;
        if(_currentSeconds > _secondsToWait)
        {
            _virusStage = _stageToChange;
            _currentSeconds = 0;
        }
    }
    private void WindowsErrorFase()
    {
        _antivirusImages[0].SetActive(false);
        if (_windowErrorTemplate != null )
        {
            _fastWindowPopUps += Time.deltaTime;
            if(_fastWindowPopUps > _maxFastWindowPopUp)
            {  
               if(_errorWindowsCount == 3 && _isDone == false)
               {
                    CreateWindow(_windowErrorTemplate, _firstWindowPos[_errorWindowsCount].transform.position);
                    _errorWindowsCount--; // To avoid array out of bounds for the windowPopUps
                    _isDone = true;
                }
               else
               {

                    CreateWindow(_windowPopUps[_errorWindowsCount], _firstWindowPos[_windowPopUpCount].transform.position);
               }
                            
                _fastWindowPopUps = 0;
                _errorWindowsCount++;
                _windowPopUpCount++;
            }
            
            if (_errorWindowsCount >= MAX_ERRORS)
            {
                _virusStage = VirusStage.WaitSeconds;
                _stageToChange = VirusStage.PasswordPhase;
                _secondsToWait = 3f;
            }                                             
        }
    }

    private void RandomWindowPopUps()
    {
        _windowPopUpSeconds += Time.deltaTime;
        if (_windowPopUpSeconds > _maxWindowPopUp)
        {
            if (_maxWindowPopUp == 0f)
            {
                CreateWindow(_storyPrefab, _storySpawnPoint.transform.position);
                _maxWindowPopUp = 8f;   // After first window appears, the next ones will appear after short times
            }
            else
            {
                int indexWindow = Random.Range(0, _windowPopUps.Count - 1);
                GameObject windowToPopUp = _windowPopUps[indexWindow];
                int indexPosition = Random.Range(0, _windowPositions.Count - 1);
                GameObject position = _windowPositions[indexPosition];
                CreateWindow(windowToPopUp, position.transform.position);
                _windowPopUpSeconds = 0;
            }
               
        }
    }

    private void WindowPasswordDraw(int version)
    {
        if(_windowPasswordTemplateUser != null)
        {
            if(version == 1)  // User password
            {
                CreateWindow(_windowPasswordTemplateUser, Vector3.zero);
                _virusStage = VirusStage.Wait;
                _passwordVersion++;
            }
            else  // Admin password
            {
                CreateWindow(_windowPasswordTemplateAdmin, Vector3.zero);
                _virusStage = VirusStage.Wait;
            }
           
        }
        
    }


    private void CreateWindow(GameObject windowTemplate, Vector3 position)
    {
        _windowsInGame.Add(Instantiate(windowTemplate, position, Quaternion.identity));
    }



    public void UserWin()
    {
        // Destroy all opened windows
        _secondsDestroyWindows += Time.deltaTime;
        if(_secondsDestroyWindows > _maxSecondsDestroyWindows)
        {
            int count = 0;
            for (int index = 0; index < _windowsInGame.Count; index++)
            {
                if (_windowsInGame[index] != null)
                {
                    Destroy(_windowsInGame[index].gameObject);
                    _windowsInGame[index] = null;
                    break;
                }
                else
                {
                    count++;
                    if(count == _windowsInGame.Count)
                    {
                        if(_doOnce == false)
                        {
                            _virusStage = VirusStage.FirstVirus;

                            GameManager.Instance.SpawnManager.KillAll();
                            SoundManager.Instance.StopSound("Music");
                            SoundManager.Instance.PlaySound("Victory", true);
                            Instantiate(_deathAnimationTemplate, Vector3.zero, Quaternion.identity);
                            _antivirusImages[_antivirusImages.Count - 1].SetActive(false);
                            _doOnce = true;
                        }
                       
                    }
                }
            }
            _secondsDestroyWindows = 0;
        }
    }

    public void DestroyActiveWindows()
    {
        // Destroy all opened windows
        _secondsDestroyWindows += Time.deltaTime;
        if (_secondsDestroyWindows > _maxSecondsDestroyWindows)
        {
            for (int index = 0; index < _windowsInGame.Count; index++)
            {
                if (_windowsInGame[index] != null)
                {
                    Destroy(_windowsInGame[index].gameObject);
                    _windowsInGame[index] = null;
                    break;
                }
            }
            _secondsDestroyWindows = 0;
        }
    }

    public VirusStage Stage
    {
        get { return _virusStage; }
        set { _virusStage = value; }
    }

    public void PlayerHit()
    {
        if(_virusStage == VirusStage.FirstVirus)
        {
            _virusStage = VirusStage.LaunchingAntivirus;
        }
    }

}
