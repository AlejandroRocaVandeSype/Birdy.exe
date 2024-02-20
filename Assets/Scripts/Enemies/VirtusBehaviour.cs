using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class VirtusBehaviour : MonoBehaviour
{

    private GameManager _gameManager = null;

    private GameObject _target;                     // Target to follow ( Player mouse )
    private NavMeshAgent _agent;
    private float _startSpeed;
    private float _startAccel;

    // Adjust these to setup virus movement
    [SerializeField] private float _maxAccel;
    [SerializeField] private float _maxSpeed;
    private float _incAccel = 0f;
    private float _incSpeed = 0f;
    private const float TARGET_RANGE = 0.3f;

    // Start is called before the first frame update
    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;  
        _agent.updateUpAxis = false;                    // Needed it for 2DNavMesh to properly work

        _startSpeed = _agent.speed;
        _startAccel = _agent.acceleration;
        _maxSpeed = 200f;
        _maxAccel = 1000f;

    }

    private void Start()
    {
        _target = FindObjectOfType<MouseFollow>().gameObject;
        _gameManager = GameManager.Instance;
    }


    void Update()
    {
        if (_gameManager == null || _target == null)
            return;

        if (_gameManager.gameStage == GameManager.GameStage.Wait)
            return;

        if (_gameManager.gameStage == GameManager.GameStage.Gameplay)
            _agent.SetDestination(_target.transform.position);
        

        if(_gameManager.VirusManager.Stage == VirusManager.VirusStage.FirstVirus)
        {
            // First virus will gradually increase its speed 
            UpdateFasterVirus();
        }
        else
        {
            // Rest of the enemies will move normal speed
            _agent.speed = _startSpeed;
            _agent.acceleration = _startAccel;
        }

        UpdateMovement();

    }


    private void UpdateFasterVirus()
    {
        
        if (_agent.acceleration < _maxAccel)
        {
            _incAccel += 0.005f;
            _agent.acceleration = _startAccel + _incAccel;
        }

        if (_agent.speed < _maxSpeed)
        {
            _incSpeed += 0.002f;
            _agent.speed = _startSpeed + _incSpeed;
        }
    }


    private void UpdateMovement()
    {
        // Use this instead of collision boxes so raycasting properly works
        if (Vector3.Distance(transform.position, _target.transform.position) < TARGET_RANGE)
        {
            MouseFollow player = _target.GetComponent<MouseFollow>();
            if (!player.WasHit)
            {
                Kill();
                player.PlayerHit();
            }

        }
    }

    public void Kill(bool lastKill = false)
    {
        Destroy(gameObject);
        SoundManager.Instance.PlaySound("EnemyCatch", false);

        if(lastKill == false)
            GameManager.Instance.SpawnManager.HasToSpawn = true;

    }



}
