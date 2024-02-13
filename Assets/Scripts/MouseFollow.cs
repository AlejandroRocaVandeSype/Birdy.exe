using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollow : MonoBehaviour
{

    private Vector3 _worldPosition;
    private Vector3 _screenPosition;
    private Vector2 _screenBounds;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        _screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    // Update is called once per frame
    void Update()
    {
        _screenPosition = Input.mousePosition;
        _worldPosition = Camera.main.ScreenToWorldPoint(_screenPosition);
        
        Vector3 position = _worldPosition;
        position.z = 0;

        position = BoundaryCheck(position);

        transform.position = position;
    }


    private Vector3 BoundaryCheck(Vector3 position)
    {

        if (position.x > _screenBounds.x - 0.5f)
        {
            position.x = _screenBounds.x - 0.5f;
        }
        else if (position.x < 0 - _screenBounds.x + 0.5f)
        {
            position.x = 0 - _screenBounds.x + 0.5f;
        }

        if (position.y > _screenBounds.y - 0.5f)
        {
            position.y = _screenBounds.y - 0.5f;
        }
        else if (position.y < 0 - _screenBounds.y + 0.5f)
        {
            position.y = 0 - _screenBounds.y + 0.5f;
        }
        return position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Enemy")
        {
            Debug.Log("Enemy Hit");
        }
    }
}
