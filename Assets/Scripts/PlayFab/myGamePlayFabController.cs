using System.Collections.Generic;
using PlayFab.ClientModels;
using PlayFab;
using UnityEngine;
using TMPro;

public class myGamePlayFabController : MonoBehaviour
{
    public static myGamePlayFabController Instance;


    [SerializeField]
    private TextMeshProUGUI _messageText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GetWoodInitially();
    }

    public void GetWoodInitially()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataRecivedAfter, OnError);
    }

    void OnDataRecivedAfter(GetUserDataResult result)
    {
        
        if (result.Data != null && result.Data.ContainsKey("Wood"))
        {
           
            updateUserWoodUI(result.Data["Wood"].Value);
        }
        else
        {
            Debug.Log("Player data not complete!");
            var request = new UpdateUserDataRequest  // player has no Wood save before, create Wood key on Playfab
            {
                Data = new Dictionary<string, string>
            {
                {"Wood", "0"}
            }
            };

            PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);

        }
    }
    void OnDataSend(UpdateUserDataResult result)
    {
        GetWoodInitially(); //Call again to check if user has Wood key
    }

    public void updateUserWoodUI(string incomingStr)
    {
        _messageText.text = "Current wood is: "+incomingStr;
    }

    void OnError(PlayFabError error)
    {
        Debug.Log("Error while logging in/creating account!");
        //_messageText.text = error.ErrorMessage;
        Debug.Log(error.GenerateErrorReport());
    }

}

