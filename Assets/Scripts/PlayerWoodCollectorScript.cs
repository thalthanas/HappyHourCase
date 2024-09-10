using System.Collections;
using System.Collections.Generic;
using PlayFab.ClientModels;
using PlayFab;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

public class PlayerWoodCollectorScript : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private int _currentCollectedWood;
    [SerializeField]
    private NavMeshAgent _agent;

    [SerializeField]
    private SelectableUnitScript _selectableUnitScript;

    [SerializeField]
    private PlayerInteractionScript _playerInteractionScript;

    [SerializeField]
    private Animator _animControlller;

    [SerializeField]
    private int _userTotalWood;

    [SerializeField]
    private PhotonView _photonView;

    // Start is called before the first frame update
    void Start()
    {
        _selectableUnitScript = GetComponent<SelectableUnitScript>();
        _photonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void collectWood() //this function is triggered on Animation Event
    {
        if(_photonView.IsMine)
        {
            if (_currentCollectedWood < 5)
            {
                _currentCollectedWood += 1;
            }
            else if (_currentCollectedWood >= 5)
            {
                _agent.SetDestination(_selectableUnitScript.baseObj.transform.position);
                _animControlller.SetBool("isReadyToChop", false);
            }
        }
        
        
    }


    public void LookAtResource()  //this function is triggered on Animation Event
    {
        _playerInteractionScript.LookAtResource();
    }







    public void GetWood()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataRecived, OnError);
    }

    void OnDataRecived(GetUserDataResult result)  
    {
        myGamePlayFabController.Instance.updateUserWoodUI(result.Data["Wood"].Value);
        if (result.Data != null && result.Data.ContainsKey("Wood"))
        {
            Debug.Log("Current wood is: " + result.Data["Wood"].Value);
            int.TryParse(result.Data["Wood"].Value,out _userTotalWood);

            deployWood();
        }
        else
        {
            Debug.Log("Player data not complete!");
        }
    }


    public void deployWood()
    {
        int toBeSendWoodValue = _userTotalWood + _currentCollectedWood;
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"Wood", toBeSendWoodValue.ToString()}
            }
        };

        PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);
    }

    void OnDataSend(UpdateUserDataResult result)
    {
        _currentCollectedWood = 0;
        GetWoodAfter();
    }


    public void GetWoodAfter()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataRecivedAfter, OnError);
    }

    void OnDataRecivedAfter(GetUserDataResult result)
    {
        myGamePlayFabController.Instance.updateUserWoodUI(result.Data["Wood"].Value);
        if (result.Data != null && result.Data.ContainsKey("Wood"))
        {
            Debug.Log("Current wood is: " + result.Data["Wood"].Value);
            int.TryParse(result.Data["Wood"].Value, out _userTotalWood);
        }
        else
        {
            Debug.Log("Player data not complete!");
        }
    }

    void OnError(PlayFabError error)
    {
        Debug.Log("Error while logging in/creating account!");
        //_messageText.text = error.ErrorMessage;
        Debug.Log(error.GenerateErrorReport());
    }
}
