using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;


public class GameSceneLogic : MonoBehaviourPunCallbacks
{
    private int alivePlayerNum;
    PlayerInfo[] AliveNum;
    //This function for Checking alive Player.
    //It is called when someone is Die Or LeftGame 
    public void Awake()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
    }
    public void AliveNumCheck()
    {
        AliveNum = FindObjectsOfType<PlayerInfo>();
        alivePlayerNum = 0; 
        int winner = 0;
        //상태가 Die가 아니라면 살아있는 것이기 때문에 살아남은 인원 카운트 가능 
        for (int i = 0; i < AliveNum.Length; i++)
        {
            if (AliveNum[i].playerAlive != state.Die)
            {
                alivePlayerNum++;
                winner = i;
            }
        }
        Debug.Log("살아남은 플레이어 수 = " + alivePlayerNum);
        Debug.Log("살아남은 플레이어 번호 =  " + winner);

        if (alivePlayerNum == 1)
           GameMgr.Instance.uIMgr.EndGame(PhotonNetwork.PlayerList[winner].NickName);
            //gameObject.GetPhotonView().RPC("EndGame", RpcTarget.All, PhotonNetwork.PlayerList[winner].NickName);
        
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        AliveNumCheck();
    }

}
