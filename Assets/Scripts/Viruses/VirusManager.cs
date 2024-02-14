using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UIElements;

public class VirusManager : MonoBehaviour
{
    public enum VirusStage { Wait, MultipleErrors, PasswordPhase, CounterPhase, DragAndDropPhase,
    VirusEnd };
    private VirusStage _stage;

    [SerializeField] private GameObject _windowErrorTemplate = null;
    [SerializeField] private GameObject _windowPasswordTemplate = null;
    [SerializeField] private List<GameObject> _windowPopUps = new List<GameObject>();
    private const int MAX_ERRORS = 9;
    private int _errorWindowsCount = 0;
    private int _windowPopUpCount = 0;
    private float _windowPopUpSeconds;
    private float _maxWindowPopUp = 0.2f;
    private bool _isDone = false;

    [SerializeField] private List<GameObject> _windowPositions = new List<GameObject>();
    [SerializeField] private List<GameObject> _firstWindowPos = new List<GameObject>();

    // Keep track of all windows from the game
    private List<GameObject> _windowsInGame = new List<GameObject>();

    private float _secondsDestroyWindows = 0f;
    private float _maxSecondsDestroyWindows = 0.5f;

    [SerializeField] private GameObject _deathAnimationTemplate = null;

    // Start is called before the first frame update
    void Awake()
    {
        _stage = VirusStage.Wait;
    }

    // Update is called once per frame
    void Update()
    {

        if(GameManager.Instance.gameStage == GameManager.GameStage.Start)
        {
            // Pop Up a single window error in the middle of the screen
            InstantiateWindow(_windowErrorTemplate,Vector3.zero, Quaternion.identity);
            GameManager.Instance.gameStage = GameManager.GameStage.WaitToStart;
        }
        else if(GameManager.Instance.gameStage == GameManager.GameStage.Gameplay)
        {
            VirusUpdate();
        }
        
    }

    public void VirusUpdate()
    {
        switch (_stage)
        {
            case VirusStage.MultipleErrors:
                WindowsErrorFase();
                break;

            case VirusStage.PasswordPhase:
                {
                    WindowPasswordDraw();
                    break;
                }
            case VirusStage.VirusEnd:
                {

                    break;
                }
        }

        if(_stage != VirusStage.Wait && _stage != VirusStage.MultipleErrors
            && _stage != VirusStage.VirusEnd)
        {
            RandomWindowPopUps();
        }
    }

    private void WindowsErrorFase()
    {
        if(_windowErrorTemplate != null )
        {
            _windowPopUpSeconds += Time.deltaTime;
            if(_windowPopUpSeconds > _maxWindowPopUp)
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
                            
                _windowPopUpSeconds = 0;
                _errorWindowsCount++;
                _windowPopUpCount++;
            }
            
            if (_errorWindowsCount >= MAX_ERRORS)
            {
                _stage = VirusStage.PasswordPhase;
                _maxWindowPopUp = 5f; 
            }                                             
        }
    }

    private void RandomWindowPopUps()
    {
        _windowPopUpSeconds += Time.deltaTime;
        if (_windowPopUpSeconds > _maxWindowPopUp)
        {
            int indexWindow = Random.Range(0, _windowPopUps.Count - 1);
            GameObject windowToPopUp = _windowPopUps[indexWindow];
            int indexPosition = Random.Range(0, _windowPositions.Count -1);
            GameObject position = _windowPositions[indexPosition];
            InstantiateWindow(windowToPopUp, position.transform.position, position.transform.rotation);
            _windowPopUpSeconds = 0;
        }
    }

    private void WindowPasswordDraw()
    {
        if(_windowPasswordTemplate != null)
        {
            InstantiateWindow(_windowPasswordTemplate, Vector3.zero, Quaternion.identity);
            _stage = VirusStage.CounterPhase;
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
                        _stage = VirusStage.VirusEnd;
                        Instantiate(_deathAnimationTemplate, Vector3.zero, Quaternion.identity);  
                        //GameManager.Instance.gameStage = GameManager.GameStage.WinScreen;
                    }
                }
            }
            _secondsDestroyWindows = 0;
        }
    }

    public VirusStage Stage
    {
        get { return _stage; }
    }


    public void PlayerHit()
    {
        if(_stage == VirusStage.Wait)
        {
            _stage = VirusStage.MultipleErrors;
        }
    }

}
