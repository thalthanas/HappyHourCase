using System.Collections;
using PlayFab.MultiplayerModels;
using PlayFab;
using UnityEngine;
using TMPro;

public class MatchMaker : MonoBehaviour
{
    [SerializeField]
    private GameObject _battleButton;
    [SerializeField]
    private TextMeshProUGUI _messageText;

    private string ticketId;

    private Coroutine pollTicketCoroutine;

    private const string QueueName = "DefaultQueue";  //Queue name on Playfab

   
    public void StartMatchMaking()
    {
        _battleButton.SetActive(false);
        _messageText.text = "Submitting Ticket";

        PlayFabMultiplayerAPI.CreateMatchmakingTicket(
            new CreateMatchmakingTicketRequest
            {
                Creator = new MatchmakingPlayer
                {
                    Entity = new EntityKey
                    {
                        Id = PlayFabManager.EntityId,
                        Type = "title_player_account"
                    },
                    Attributes = new MatchmakingPlayerAttributes
                    {
                        DataObject = new { }
                    }
                },

                GiveUpAfterSeconds = 120,

                QueueName = QueueName
            },
            OnMatchmakingTicketCreated, OnMatchmakingError);
    }

    private void OnMatchmakingTicketCreated(CreateMatchmakingTicketResult result)
    {
        ticketId = result.TicketId;
        pollTicketCoroutine = StartCoroutine(PollTicket());
        _messageText.text = "Ticket Created";
    }


    private void OnMatchmakingError(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }

    private IEnumerator PollTicket()
    {
        while (true)
        {
            PlayFabMultiplayerAPI.GetMatchmakingTicket(
                new GetMatchmakingTicketRequest
                {
                    TicketId = ticketId,
                    QueueName = QueueName

                }, OnGetMatchmakingTicket, OnMatchmakingError);

            yield return new WaitForSeconds(6);

        }
    }

    private void OnGetMatchmakingTicket(GetMatchmakingTicketResult result)
    {
        _messageText.text = $"Status: {result.Status}";

        switch(result.Status)
        {
            case "Matched":
                StopCoroutine(pollTicketCoroutine);
                StartMatch(result.MatchId);
                break;

            case "Canceled":
                break;

        }
    }

    private void StartMatch(string matchId)
    {
        _messageText.text = "Starting Match";

        PlayFabMultiplayerAPI.GetMatch(
            new GetMatchRequest
            {
                MatchId = matchId,
                QueueName = QueueName
            },
            OnGetMatch, OnMatchmakingError);
    }

    private void OnGetMatch(GetMatchResult result)
    {
        _messageText.text = $"{result.Members[0].Entity.Id} vs {result.Members[1].Entity.Id}";

        GameLoader.CreateRoom(_messageText.text); // creates a room with a name like player1 vs player2
    }
}
