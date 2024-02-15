using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollow : MonoBehaviour
{

    private Vector3 _worldPosition;
    private Vector3 _screenPosition;
    private Vector2 _screenBounds;
    [SerializeField] private GameObject _mouseGO;
    [SerializeField] private Sprite _mouseNoHit;
    [SerializeField] private Sprite _mouseHit;
    [SerializeField] private GameObject _mouseAntivirus;
    [SerializeField] private GameObject _damageImage;
    private Sprite _currentSprite;

    // Sprite swap/color when hit
    private float _maxWait = 2.5f;
    private float _swapTime = 0.2f;
    private float _seconds = 0f;
    private float _maxSeconds = 0f;
    private Color _currentColor = Color.red;
    private bool _wasHit = false;

    private const float BORDERS = 0.18f;


    private const string ENEMY_TAG = "Enemy";
    private GameManager _gameManager = null;


    private float _maxDamageScreen = 0.1f;
    private float _secondsDamageScreen = 0f;
    bool _active = false;
    bool _done = false;
    int _count = 1;

    // Start is called before the first frame update
    void Start()
    {
       Cursor.visible = false;         // Don't show mouse cursor
        _screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        _gameManager = GameManager.Instance;

        _currentSprite = _mouseHit;
    }

    // Update is called once per frame
    void Update()
    {
        GetMousePos();

        if(_wasHit)
        {
            InvincibilityUpdate();
            _mouseAntivirus.SetActive(true);

            if(_done == false)
            {
                _secondsDamageScreen += Time.deltaTime;
                if (_secondsDamageScreen > _maxDamageScreen)
                {

                    _damageImage.SetActive(!_active);
                    _active = !_active;
                    _secondsDamageScreen = 0f;

                    if(_active == false)
                    {
                        _count++;
                        if(_count > 1)
                        {
                            _done = true;
                            _count = 1;
                        }                    
                    }

                }
            }
            
        }
        else
        {
            _mouseAntivirus.SetActive(false);
            _done = false;
            _active = false;
        }
    }

    private void GetMousePos()
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

        if (position.x > _screenBounds.x - BORDERS - 0.7f)
        {
            position.x = _screenBounds.x - BORDERS - 0.7f;
        }
        else if (position.x < 0 - _screenBounds.x + BORDERS + 0.7f)
        {
            position.x = 0 - _screenBounds.x + BORDERS + 0.7f;
        }

        if (position.y > _screenBounds.y - BORDERS)
        {
            position.y = _screenBounds.y - BORDERS;
        }
        else if (position.y < 0 - _screenBounds.y + BORDERS)
        {
            position.y = 0 - _screenBounds.y + BORDERS;
        }
        return position;
    }

    private void InvincibilityUpdate()
    {
        _seconds += Time.deltaTime;
        _maxSeconds += Time.deltaTime;
        if (_maxSeconds < _maxWait)
        {
            if (_seconds > _swapTime)
            {
                _seconds = 0f;
               // _mouseGO.GetComponent<SpriteRenderer>().color = _currentColor;
                _mouseGO.GetComponent<SpriteRenderer>().sprite = _currentSprite;
                if (_currentColor == Color.red)
                {
                    _currentColor = Color.white;
                    _currentSprite = _mouseHit;
                }
                else
                {
                    _currentColor = Color.red;
                    _currentSprite = _mouseNoHit;
                }
            }
        }
        else
        {
            // Finish max time
            _seconds = 0f;
            _maxSeconds = 0f;
            _wasHit = false;
            //_mouseGO.GetComponent<SpriteRenderer>().color = Color.white;
            _currentSprite = _mouseNoHit;
            _currentColor = Color.red;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == ENEMY_TAG && !_wasHit)
        {

            PlayerHit();
        }
    }

    public void PlayerHit()
    {
        _wasHit = true;
        _mouseGO.GetComponent<SpriteRenderer>().color = _currentColor;
        _currentColor = Color.white;
        _gameManager.VirusManager.PlayerHit();      // Sent a notifaction that the player was hit
        _gameManager.Storage.IncreaseCorruption();
    }

    public bool WasHit
    {
        get { return _wasHit; }
        set { _wasHit = true; }
    }
}
