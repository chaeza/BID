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
    public int playerNumCount;
    //This function for Checking alive Player.
    //It is called when someone is Die Or LeftGame 
    public void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = false;

        GameMgr.Instance.GameState = true;
        GameMgr.Instance.GameSceneSetting(gameObject);
        GameMgr.Instance.del_DestroyTarget = PunDes;
        GameMgr.Instance.del_PunFindObject = PunFindObject;
    }
    public void PlayerCheck2()
    {
        playerNumCount++;
        if (playerNumCount == PhotonNetwork.CurrentRoom.PlayerCount && PhotonNetwork.IsMasterClient)
            PhotonNetwork.CurrentRoom.IsOpen = false;
    }

    public void PunDes(GameObject desObject, float time )
    {
        gameObject.GetPhotonView().RPC("PunDestroyObject", RpcTarget.All, desObject.GetPhotonView().ViewID, time);
    }
    [PunRPC]
    public void PunDestroyObject(int viewid, float time)
    {
        Destroy(PunFindObject(viewid), time);
    }
    public GameObject PunFindObject(int viewID3)//����̵� �Ѱܹ޾� ������� ������Ʈ�� ã�´�.
    {
        GameObject find = null;
        PhotonView[] viewObject = FindObjectsOfType<PhotonView>();
        for (int i = 0; i < viewObject.Length; i++)
        {
            if (viewObject[i].ViewID == viewID3) find = viewObject[i].gameObject;
        }
        if (find != null) return find;
        else return null;
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
        if (alivePlayerNum == 1)
        {
            if (PhotonNetwork.PlayerList[winner].NickName == PhotonNetwork.NickName) GameMgr.Instance.uIMgr.EndGame(true);
            else GameMgr.Instance.uIMgr.EndGame(false);
            WinnerEndGame();
        }
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        AliveNumCheck();
    }
    public void WinnerEndGame()
    {
        //API ����
        //  StartCoroutine(processRequestBetting_Zera_DeclareWinner());
        //photonView.RPC("EndGame", RpcTarget.All);
        //  EndGame();
        StartCoroutine(endTimer());
    }

    //[PunRPC]
    public void EndGame()
    {
        StartCoroutine(endTimer());
    }

    IEnumerator endTimer()
    {
        GameMgr.Instance.GameState = false;
        yield return new WaitForSeconds(2);
        PhotonNetwork.LoadLevel("TitleScene");
        PhotonNetwork.LeaveRoom();
        GameMgr.Instance.GameSceneSettingInitializing();
    }

    //ESC������ ��ư
    public void OnClick_LeaveGame()
    {
        GameMgr.Instance.GameState = false;
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("TitleScene");
        GameMgr.Instance.GameSceneSettingInitializing();
    }

}
