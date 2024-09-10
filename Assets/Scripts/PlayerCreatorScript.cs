using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCreatorScript : MonoBehaviourPunCallbacks
{

   // public GameObject myPlayerPrefab;

    //public Material myMaterial;

    [SerializeField]
    private GameObject go;

    [SerializeField]
    private GameObject blueBase, redBase;

    [SerializeField]
    private Image _playerColorImage;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            if(PhotonNetwork.IsMasterClient) //create player units for master client
            {
                go = PhotonNetwork.Instantiate("NewPlayer", new Vector3(-12+(i*2),0.5f,12), Quaternion.Euler(0,-200,0));

                go.GetComponent<SelectableUnitScript>().isLocal = true;
                go.GetComponent<SelectableUnitScript>().baseObj = blueBase;
            }
            else  // create player units for other client
            {
                go = PhotonNetwork.Instantiate("NewPlayer", new Vector3(4 + (i * 2), 0.5f, -14), Quaternion.identity);

                go.GetComponent<SelectableUnitScript>().isLocal = true;
                go.GetComponent<SelectableUnitScript>().baseObj = redBase;
            }
           

           

            if (!PhotonNetwork.IsMasterClient)
            {
                go.GetComponent<PhotonView>().RPC("SyncMaterial", RpcTarget.All); // sets other clients color to red
                _playerColorImage.color= new Color32(166, 26, 54, 255);
            }
            else
            {
                _playerColorImage.color = new Color32(90, 117, 233, 255);
            }
                
        }
        
       
       
        
    }

   
}
