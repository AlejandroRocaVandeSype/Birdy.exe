using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

public class VirusManager : MonoBehaviour
{
    public enum VirusStage { Wait, MultipleErrors, PasswordPhase, CounterPhase, DragAndDropPhase};
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
        }

        if(_stage != VirusStage.Wait && _stage != VirusStage.MultipleErrors)
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

                //int index = Random.Range(0, _windowPositions.Count - 1);
                //GameObject positionToPopUp = _windowPositions[index];
                //InstantiateWindow(_windowErrorTemplate, positionToPopUp.transform.position, positionToPopUp.transform.rotation);
               
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
        Instantiate(windowTemplate, position, rotation);
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
