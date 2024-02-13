using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusManager : MonoBehaviour
{
    private enum VirusStage { ErrorFase, ErrorFase2, Fase1, Fase2, FinalFase};
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
        _stage = VirusStage.ErrorFase;
    }

    // Update is called once per frame
    void Update()
    {
        switch (_stage)
        {
            case VirusStage.ErrorFase:
                    WindowsErrorFase();
                break;

            case VirusStage.ErrorFase2:
                {
                    WindowsErrorFase2();
                    break;
                }
            


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
                _stage = VirusStage.ErrorFase2;
                _maxErrorSeconds = 5f; 
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
        Instantiate(windowTemplate, _windowPositions[index].transform);
        _errorWindowsCount++;
    }


}
