using System.Collections.Generic;
using UnityEngine;


public class VirusManager : MonoBehaviour
{
    public enum VirusStage { Wait, FirstVirus, WindowPopUps, MultipleErrors, PasswordPhase, 
        AntivirusStart,DestroyWindows, WaitSeconds, VirusEnd };
    private VirusStage _stage;

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

    private float _secondsForPassword = 30f;
    private float _currentSecondsPass = 0f;

    private VirusStage _stageToChange = VirusStage.VirusEnd;
    private float _secondsToWait = 0f;
    private float _currentSeconds = 0f;


    private float _antivirusMaxSeconds = 0f;
    private float _currentSecondsAntivirus = 0f;
    [SerializeField] private List<GameObject> _antivirusImages = new List<GameObject>();
    private int _limitAntivirus = 4;
    private int _currentIndexAntivirus = 1;
    bool _firstTimeAntivirus = true;
    bool _doOnce = false;

    bool _playMusic = true;

    bool _firstWait = true;
    // Start is called before the first frame update
    void Awake()
    {
        _stage = VirusStage.FirstVirus;
    }

    // Update is called once per frame
    void Update()
    {

        if(GameManager.Instance.gameStage == GameManager.GameStage.Start)
        {
            // Pop Up a single window error in the middle of the screen
            InstantiateWindow(_windowErrorTemplate,Vector3.zero, Quaternion.identity);
            GameManager.Instance.gameStage = GameManager.GameStage.WaitToStart;
            _secondsToWait = 5f;
        }
        else if(GameManager.Instance.gameStage == GameManager.GameStage.Gameplay)
        {
            if(_playMusic)
            {
                SoundManager.Instance.PlaySound("Music", false);
                _playMusic = false;
            }

            if(_firstWait)
            {
                _currentSeconds += Time.deltaTime;
                if (_currentSeconds > _secondsToWait)
                {
                    _currentSeconds = 0;
                    _firstWait = false;
                }
            }
            else
            {
                VirusUpdate();
            }
                    
        }
        
    }

    public void VirusUpdate()
    {
        // Always popUp Windows 
        if ( _stage != VirusStage.FirstVirus && _stage != VirusStage.VirusEnd
            && _stage != VirusStage.MultipleErrors && _stage != VirusStage.DestroyWindows
            && _stage != VirusStage.WaitSeconds && _stage != VirusStage.WaitSeconds)
        {
            RandomWindowPopUps();
        }

        switch (_stage)
        {
            case VirusStage.WindowPopUps:
                {
                    // Wait a few seconds before changing stage to Password
                    _currentSecondsPass += Time.deltaTime;
                    if(_currentSecondsPass > _secondsForPassword)
                    {
                        _currentSecondsPass = 0;
                        _stage = VirusStage.PasswordPhase;
                    }
                    break;
                }
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
                        _stage = VirusStage.WaitSeconds;
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
                        for (int index = 1; index <= _antivirusImages.Count - 1; ++index)
                        {
                            _antivirusImages[index].SetActive(false);
                        }

                        _stage = VirusStage.WaitSeconds;
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
            _stage = _stageToChange;
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
                   InstantiateWindow(_windowErrorTemplate, _firstWindowPos[_errorWindowsCount].transform.position, _firstWindowPos[_errorWindowsCount].transform.rotation);
                    _errorWindowsCount--; // To avoid array out of bounds for the windowPopUps
                    _isDone = true;
                }
               else
               {
                    
                   InstantiateWindow(_windowPopUps[_errorWindowsCount], _firstWindowPos[_windowPopUpCount].transform.position, _firstWindowPos[_windowPopUpCount].transform.rotation);
               }
                            
                _fastWindowPopUps = 0;
                _errorWindowsCount++;
                _windowPopUpCount++;
            }
            
            if (_errorWindowsCount >= MAX_ERRORS)
            {
                WindowPasswordDraw(2);
                _stage = VirusStage.WaitSeconds;
                _stageToChange = VirusStage.Wait;
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
                _maxWindowPopUp = 8f;   // After first window appears, the next ones will appear after short times

            int indexWindow = Random.Range(0, _windowPopUps.Count - 1);
            GameObject windowToPopUp = _windowPopUps[indexWindow];
            int indexPosition = Random.Range(0, _windowPositions.Count -1);
            GameObject position = _windowPositions[indexPosition];
            InstantiateWindow(windowToPopUp, position.transform.position, position.transform.rotation);
            _windowPopUpSeconds = 0;
        }
    }

    private void WindowPasswordDraw(int version)
    {
        if(_windowPasswordTemplateUser != null)
        {
            if(version == 1)
            {
                InstantiateWindow(_windowPasswordTemplateUser, Vector3.zero, Quaternion.identity);
                _stage = VirusStage.Wait;
            }
            else
            {
                InstantiateWindow(_windowPasswordTemplateAdmin, Vector3.zero, Quaternion.identity);
            }
           
        }
        
    }


    private void InstantiateWindow(GameObject windowTemplate, Vector3 position, Quaternion rotation)
    {
        _windowsInGame.Add(Instantiate(windowTemplate, position, rotation));
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
                            _stage = VirusStage.FirstVirus;

                            GameManager.Instance.SpawnManager.KillAll();

                            Instantiate(_deathAnimationTemplate, Vector3.zero, Quaternion.identity);
                            _doOnce = true;
                        }
                       
                        //GameManager.Instance.gameStage = GameManager.GameStage.WinScreen;
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
                    if (count == _windowsInGame.Count)
                    {
                        //_stage = VirusStage.WaitSeconds;
                        //_stageToChange = VirusStage.MultipleErrors;
                        //_secondsToWait = 2f;
                    }
                }
            }
            _secondsDestroyWindows = 0;
        }
    }


    public VirusStage Stage
    {
        get { return _stage; }
        set { _stage = value; }
    }


    public void PlayerHit()
    {
        if(_stage == VirusStage.FirstVirus)
        {
            _stage = VirusStage.WindowPopUps;
        }
    }

}
