using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.CloudScriptModels;
using TMPro;

public class PlayFabManager : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI _messageText;
    [SerializeField]
    private TMP_InputField _emailInput;
    [SerializeField]
    private TMP_InputField _passwordInput;

    [SerializeField]
    private TextMeshProUGUI _counterText;

    [SerializeField]
    private GameObject _introPanel;
    [SerializeField]
    private GameObject _gamePanel;


    [SerializeField]
    private TextMeshProUGUI _welcomeText;


    public static string SessionTicket;
    public static string EntityId;
    public void RegisterButton()
    {
        if (_passwordInput.text.Length < 6)
        {
            _messageText.text = "Password too short!";
            return;
        }
        var request = new RegisterPlayFabUserRequest
        {
            Email = _emailInput.text,
            Password = _passwordInput.text,
            RequireBothUsernameAndEmail = false
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
    }

    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        _messageText.text = "Registered and logged in!";
        Debug.Log("Successful account create!");
        SessionTicket = result.SessionTicket;
        EntityId = result.EntityToken.Entity.Id;

        PlayFabHttpTriggerCloudFunction();

        _introPanel.SetActive(false);
        _gamePanel.SetActive(true);

    }

    public void LoginButton()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = _emailInput.text,
            Password = _passwordInput.text
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
    }

    void OnLoginSuccess(LoginResult result)
    {
        _messageText.text = "Logged in!";
        Debug.Log("Successful login!");

        SessionTicket = result.SessionTicket;
        EntityId = result.EntityToken.Entity.Id;

        PlayFabHttpTriggerCloudFunction();

        _introPanel.SetActive(false);
        _gamePanel.SetActive(true);

       
    }

    public void ResetPasswordButton()
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = _emailInput.text,
            TitleId = "1AAFE"
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnPasswordReset, OnError);
    }

    void OnPasswordReset(SendAccountRecoveryEmailResult result)
    {
        _messageText.text = "Password reset mail sent!";
    }


    private void PlayFabHttpTriggerCloudFunction()
    {
        ExecuteFunctionRequest cloudFunction = new ExecuteFunctionRequest()
        {
            FunctionName = "httpTriggerCloudFunction",
            GeneratePlayStreamEvent = false
        };
        PlayFabCloudScriptAPI.ExecuteFunction(cloudFunction, OnCloudFunctionRun, OnError);
    }

    void OnCloudFunctionRun(ExecuteFunctionResult result)
    {
        Debug.Log(result.FunctionResult);
        _counterText.text = "Game Open Counter: " +result.FunctionResult;
    }
   

    void OnError(PlayFabError error)
    {
        _messageText.text = error.ErrorMessage;
        Debug.Log(error.GenerateErrorReport());
    }
}
