using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.EventSystems;

public class WindowBasic : MonoBehaviour, IDragHandler
{
    [SerializeField] protected GameObject _base;
    [SerializeField] protected GameObject _closeButton;

    private Vector3 _dragOffset;




    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Vector3.MoveTowards(transform.position, GameManager.Instance.Mouse.gameObject.transform.position, 40*Time.deltaTime);
    }

    protected virtual void Update()
    {

    }

    protected virtual void CloseWindow()
    {
        Destroy(gameObject);

    }


}
