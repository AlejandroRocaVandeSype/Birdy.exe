using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class VirtusBehaviour : MonoBehaviour
{
   
    private Transform _target;
    private NavMeshAgent _agent;

    // Start is called before the first frame update
    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        
        _agent.updateUpAxis = false;

        _target = FindObjectOfType<MouseFollow>().gameObject.transform;
    }

   

    // Update is called once per frame
    void Update()
    {

        if(GameManager.Instance.gameStage == GameManager.GameStage.Gameplay)
        {
            _agent.SetDestination(_target.position);
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
