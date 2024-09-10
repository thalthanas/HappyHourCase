using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

public class PlayerInteractionScript : MonoBehaviour
{
    [SerializeField]
    private Animator _animController;

    [SerializeField]
    private PlayerWoodCollectorScript _woodCollector;

    [SerializeField]
    private Transform _lastResourceTransform;

    [SerializeField]
    private NavMeshAgent _agent;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void LookAtResource()
    {
        transform.LookAt(_lastResourceTransform.transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name =="ResourceTree")
        {
            _animController.SetBool("isReadyToChop", true);

            _lastResourceTransform = other.transform;
        }

        if(PhotonNetwork.IsMasterClient)
        {
            if(other.gameObject.name =="BlueBase")
            {
                _woodCollector.GetWood();  //player reached the base, run calculations for wood
                if(_lastResourceTransform!=null)
                {
                    _agent.SetDestination(_lastResourceTransform.position);
                }
               

            }
        }
        else
        {
            if (other.gameObject.name == "RedBase")
            {
                _woodCollector.GetWood(); //player reached the base, run calculations for wood
                if (_lastResourceTransform != null)
                {
                    _agent.SetDestination(_lastResourceTransform.position);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "ResourceTree")
        {
            _animController.SetBool("isReadyToChop", false);
        }
    }

}
