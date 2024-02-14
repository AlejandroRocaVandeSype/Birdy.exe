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
    private const int MAX_ERRORS = 5;
    private int _errorWindowsCount = 0;
    private float _errorSeconds;
    private float _maxErrorSeconds = 0.5f;
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

            //case VirusStage.ErrorFase2:
            //    {
            //        WindowsErrorFase2();
            //        break;
            //    }
        }
    }

    private void WindowsErrorFase()
    {
        if(_windowErrorTemplate != null )
        {
            _errorSeconds += Time.deltaTime;
            if(_errorSeconds > _maxErrorSeconds)
            {
                InstantiateWindow(_windowErrorTemplate);
                _errorSeconds = 0;
            }
            
            if (_errorWindowsCount >= MAX_ERRORS)
            {
                _stage = VirusStage.PasswordPhase;
                _maxErrorSeconds = 2f; 
            }                                             
        }
    }

    private void WindowsErrorFase2()
    {
        if (_windowErrorTemplate != null)
        {
            _errorSeconds += Time.deltaTime;
            if (_errorSeconds > _maxErrorSeconds)
            {
                InstantiateWindow(_windowErrorTemplate);
                _errorSeconds = 0;
            }
        }
    }

    private void InstantiateWindow(GameObject windowTemplate)
    {
       
        int index = Random.Range(0, _windowPositions.Count - 1);
        Instantiate(windowTemplate, _windowPositions[index].transform.localPosition, _windowPositions[index].transform.rotation);
        _errorWindowsCount++;
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
