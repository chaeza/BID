using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;


public class GameSceneLogic : MonoBehaviourPunCallbacks
{
    private int alivePlayerNum;
    PlayerInfo[] AliveNum;
    UIMgr uIMgr;
    //This function for Checking alive Player.
    //It is called when someone is Die Or LeftGame 
    public void Awake()
    {
        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.CurrentRoom.IsOpen = false;
        uIMgr = FindObjectOfType<UIMgr>();
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
        if (alivePlayerNum == 1&&PhotonNetwork.IsMasterClient)
            uIMgr.photonView.RPC("EndGame", RpcTarget.All, PhotonNetwork.PlayerList[winner].NickName);

        //        GameMgr.Instance.uIMgr.photonView.RPC("EndGame", RpcTarget.All, PhotonNetwork.PlayerList[winner].NickName);
        // GameMgr.Instance.uIMgr.EndGame(PhotonNetwork.PlayerList[winner].NickName);

    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        AliveNumCheck();
    }
    public void WinnerEndGame()
    {
        //API 승자
        //  StartCoroutine(processRequestBetting_Zera_DeclareWinner());
        //photonView.RPC("EndGame", RpcTarget.All);
        EndGame();
    }

    //[PunRPC]
    public void EndGame()
    {
        StartCoroutine(endTimer());
    }

    IEnumerator endTimer()
    {
        yield return new WaitForSeconds(2);
        PhotonNetwork.LoadLevel("TitleScene");
        PhotonNetwork.LeaveRoom();
    }

    //ESC나가기 버튼
    public void OnClick_LeaveGame()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("TitleScene");

    }

}
