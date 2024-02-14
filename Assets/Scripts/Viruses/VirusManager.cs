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
    [SerializeField] private List<GameObject> _windowPopUps = new List<GameObject>();
    private const int MAX_ERRORS = 5;
    private int _errorWindowsCount = 0;
    private float _windowPopUpSeconds;
    private float _maxWindowPopUp = 0.5f;
    private bool _isFirstStep = true;

    [SerializeField] private List<GameObject> _windowPositions = new List<GameObject>();

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

                int index = Random.Range(0, _windowPositions.Count - 1);
                GameObject positionToPopUp = _windowPositions[index];
                InstantiateWindow(_windowErrorTemplate, positionToPopUp.transform.position, positionToPopUp.transform.rotation);
                _windowPopUpSeconds = 0;
                _errorWindowsCount++;
            }
            
            if (_errorWindowsCount >= MAX_ERRORS)
            {
                _stage = VirusStage.PasswordPhase;
                _maxWindowPopUp = 3f; 
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
