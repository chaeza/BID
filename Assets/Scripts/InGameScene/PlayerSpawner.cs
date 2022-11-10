using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using RPG_Indicator;

public class PlayerSpawner : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject[] spawnPoint;
    private bool[] playerOn;
    private int spawnPointNum;
    private void Start()
    {
        playerOn = new bool[spawnPoint.Length];
        Player[] playerList = PhotonNetwork.PlayerList;
        Debug.Log(playerList.Length + "vvvvvvvvvvvv");
        for (int i = 0; i < playerList.Length; i++)
        {
            if (playerList[i].NickName == PhotonNetwork.NickName) spawnPointNum = i;
        }
        GameObject player = PhotonNetwork.Instantiate("Player", spawnPoint[spawnPointNum].transform.position, Quaternion.identity);
        GameMgr.Instance.followCam.SetPlayerPos(player.transform);
        GameMgr.Instance.codeExample.PlayerIndicator = player.GetComponentInChildren<RpgIndicator>();

        Debug.Log("생성완료");
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
