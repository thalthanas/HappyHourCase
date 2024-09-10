using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class SelectableUnitScript : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField]
    private GameObject selectionSprite;

    [SerializeField]
    private Animator _animController;

    [SerializeField]
    private float _agentSpeed;

    public GameObject baseObj;

    
    public bool isLocal;


    public Material myMaterial;

    [SerializeField]
    private GameObject _modelObject;

    private void Awake()
    {
        _animController = GetComponent<Animator>();
        SelectionManagerScript.Instance.AvailableUnits.Add(this);
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        _agentSpeed = agent.velocity.magnitude;
        _animController.SetFloat("agentSpeed", _agentSpeed);

        if(_agentSpeed>0.1)
        {
            _animController.SetBool("isWalking", true);
        }
        else if(_agentSpeed<=0.1)
        {
            _animController.SetBool("isWalking", false);
        }
    }

    public void MoveTo(Vector3 clickedPos)
    {
        agent.SetDestination(clickedPos);
    }

    public void OnSelected()
    {
        selectionSprite.SetActive(true);
    }

    public void OnDeselect()
    {
        selectionSprite.SetActive(false);
    }

    [PunRPC]
    private void SyncMaterial()
    {
        _modelObject.GetComponent<SkinnedMeshRenderer>().material = myMaterial;
    }
}
