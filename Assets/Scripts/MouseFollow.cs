using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollow : MonoBehaviour
{

    private Vector3 _worldPosition;
    private Vector3 _screenPosition;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        _screenPosition = Input.mousePosition;
        _worldPosition = Camera.main.ScreenToWorldPoint(_screenPosition);
        
        Vector3 position = _worldPosition;
        position.z = 0;

        transform.position = position;
    }
}
