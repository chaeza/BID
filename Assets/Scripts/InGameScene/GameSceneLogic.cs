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
        //���°� Die�� �ƴ϶�� ����ִ� ���̱� ������ ��Ƴ��� �ο� ī��Ʈ ���� 
        for (int i = 0; i < AliveNum.Length; i++)
        {
            if (AliveNum[i].playerAlive != state.Die)
            {
                alivePlayerNum++;
                winner = i;
            }
        }
        Debug.Log("��Ƴ��� �÷��̾� �� = " + alivePlayerNum);
        Debug.Log("��Ƴ��� �÷��̾� ��ȣ =  " + winner);

        if (alivePlayerNum == 1)
           GameMgr.Instance.uIMgr.EndGame(PhotonNetwork.PlayerList[winner].NickName);
            //gameObject.GetPhotonView().RPC("EndGame", RpcTarget.All, PhotonNetwork.PlayerList[winner].NickName);
        
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        AliveNumCheck();
    }

}
