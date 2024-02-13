using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WindowBasic : MonoBehaviour
{
    [SerializeField] protected GameObject _base;
    [SerializeField] protected GameObject _closeButton;
    
    protected virtual void CloseWindow()
    {
        Destroy(gameObject);

    }


}
