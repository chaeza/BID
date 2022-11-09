using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviourPun
{
   Vector3 playerPos = new Vector3(379.9445f, 62.06178f, 326.5233f);

    private void Awake()
    {
        if(photonView.IsMine)
        PhotonNetwork.Instantiate("Player", playerPos,Quaternion.identity);
    }

}
