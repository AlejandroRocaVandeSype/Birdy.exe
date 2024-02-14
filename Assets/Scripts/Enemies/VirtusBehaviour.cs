using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class VirtusBehaviour : MonoBehaviour
{
   
    private GameObject _target;
    private NavMeshAgent _agent;
    private float _startSpeed;
    private float _startAccel;


    // Start is called before the first frame update
    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        
        _agent.updateUpAxis = false;

        _target = FindObjectOfType<MouseFollow>().gameObject;

        _startSpeed = _agent.speed;
        _startAccel = _agent.acceleration;

    }

   

    // Update is called once per frame
    void Update()
    {

        if (GameManager.Instance.gameStage == GameManager.GameStage.Wait)
            return;

        if (GameManager.Instance.gameStage == GameManager.GameStage.Gameplay)
        {
            _agent.SetDestination(_target.transform.position);
        }

        if(GameManager.Instance.VirusManager.Stage == VirusManager.VirusStage.Wait)
        {
            // First enemy will move faster
            _agent.speed = _startSpeed * 2;
            _agent.acceleration = _startAccel * 2;
        }
        else
        {
            // Rest of the enemies will move normal speed
            _agent.speed = _startSpeed;
            _agent.acceleration = _startAccel;
        }

        if (Vector3.Distance(transform.position, _target.transform.position) < 0.3f)
        {
            if (!_target.GetComponent<MouseFollow>().WasHit)
            {
                Kill();
                _target.GetComponent<MouseFollow>().PlayerHit();
            }

        }

    }




    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Friendly")
        {
            MouseFollow mouse = collision.gameObject.GetComponent<MouseFollow>();
            if (mouse != null && !mouse.WasHit)
            {
                Kill();
            }
            
        }
    }


    private void Kill()
    {
        Destroy(gameObject);
        GameManager.Instance.SpawnManager.SpawnNewWave();

    }
}
