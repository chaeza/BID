using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerSpawner : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject[] spawnPoint;
    private bool[] playerOn;
    private int spawnPointNum;
    private void Awake()
    {
        playerOn = new bool[spawnPoint.Length];
        Player[] playerList = PhotonNetwork.PlayerList;
        for (int i = 0; i < playerList.Length; i++)
        {
            if (playerList[i].NickName == PhotonNetwork.NickName) spawnPointNum = i;
        }
        if (photonView.IsMine)
        {
           GameObject player = PhotonNetwork.Instantiate("Player", spawnPoint[spawnPointNum].transform.position, Quaternion.identity);
            Debug.Log("생성완료");
        }
    }
    public int PlayerInstantiateLogic()
    {
        int ran = -1;
        while (true)
        {
            ran = Random.Range(0, spawnPoint.Length);
            if (playerOn[ran] == false)
            {
                playerOn[ran] = true;
                break;
            }
        }
        return ran;
    }
}
