using UnityEngine;
using Photon.Pun;


public class introConnectionScript : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject _connectionCanvas;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
            _connectionCanvas.SetActive(false);
        }
    }
}
