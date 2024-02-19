using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class MouseFollow : MonoBehaviour
{

    // Mouse coord will be converted from screen to world position
    private Vector3 _worldPosition;
    private Vector3 _screenPosition;

    private Vector2 _screenBounds;          // Limits for the mouse
    private Vector2 _spriteSize;


    [SerializeField] private GameObject _mouseGObject;
    [SerializeField] private Sprite _mouseNoHit;
    [SerializeField] private Sprite _mouseHit;
    [SerializeField] private GameObject _invencibilitySprite;
    [SerializeField] private GameObject _hitSprite;
    private Sprite _currentSprite;

    // Sprite swap when hit
    private float _invencibilityDuration = 2.5f;
    private float _swapDuration = 0.1f;
    private float secondsSwap = 0f;
    private float _invencibilitySeconds = 0f;
    private Color _currentColor = Color.red;                // Used to check which sprite is currently being rendered
    private bool _wasHit = false;

    private GameManager _gameManager = null;


    private float _maxDamageScreen = 0.1f;
    private float _secondsDamageScreen = 0f;
    bool _active = false;
    bool _wasHitRendered = false;                           // Only render the damage overlay if it wasn't already


    public void Awake()
    {
        Cursor.visible = false;              // Hide the real cursor
        _screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        _currentSprite = _mouseHit;
    }

    void Start()
    {    
        _gameManager = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMousePos();

        if(_wasHit)
        {
            UpdateInvencibility();
            UpdateDamageSprite();
            
        }
        else
        {
            _invencibilitySprite.SetActive(false);
            _wasHitRendered = false;
            _active = false;
        }
    }

    private void UpdateMousePos()
    {
        _screenPosition = Input.mousePosition;
        _worldPosition = Camera.main.ScreenToWorldPoint(_screenPosition);

        Vector3 position = _worldPosition;
        position.z = 0;

        _spriteSize.x = _currentSprite.bounds.size.x;
        _spriteSize.y = _currentSprite.bounds.size.y/2.5f;

        BoundaryCheck(ref position);

        // Update Gameobject's position
        transform.position = position;
    }
    private void BoundaryCheck(ref Vector3 position)
    {
        float minValue = 0 - _screenBounds.x + _spriteSize.x;
        float maxValue = _screenBounds.x - _spriteSize.x;
        position.x = Mathf.Clamp(position.x, minValue, maxValue);

        minValue = 0 - _screenBounds.y + _spriteSize.y;
        maxValue = _screenBounds.y - _spriteSize.y;
        position.y = Mathf.Clamp(position.y, minValue, maxValue);
    }

    private void UpdateInvencibility()
    {
        secondsSwap += Time.deltaTime;
        _invencibilitySeconds += Time.deltaTime;
        if (_invencibilitySeconds < _invencibilityDuration)
        {
            if (secondsSwap > _swapDuration)
            {
                secondsSwap = 0f;
                _mouseGObject.GetComponent<SpriteRenderer>().sprite = _currentSprite;
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
            secondsSwap = 0f;
            _invencibilitySeconds = 0f;
            _wasHit = false;
            _currentSprite = _mouseNoHit;
            _currentColor = Color.red;
        }

        _invencibilitySprite.SetActive(true);
    }

    private void UpdateDamageSprite()
    {
         if(_wasHitRendered == false)
         {
            _secondsDamageScreen += Time.deltaTime;
            if (_secondsDamageScreen > _maxDamageScreen)
            {

                _hitSprite.SetActive(!_active);
                _active = !_active;
                _secondsDamageScreen = 0f;

                if(_active == false)
                {
                    _wasHitRendered = true;                                         
                }
            }
         }
    }

    public void PlayerHit()
    {
        _wasHit = true;
        _mouseGObject.GetComponent<SpriteRenderer>().color = _currentColor;
        _currentColor = Color.white;

        if (_gameManager == null)
            return;

        _gameManager.VirusManager.PlayerHit();      // Sent a notifaction that the player was hit
        _gameManager.Storage.IncreaseCorruption();
    }

    public bool WasHit
    {
        get { return _wasHit; }
    }
}
