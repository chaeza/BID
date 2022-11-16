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
        //    PhotonNetwork.AutomaticallySyncScene = false;

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

    public void PunDes(GameObject desObject, float time)
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
            Player[] sortedPlayers = PhotonNetwork.PlayerList;
            //����
            if (sortedPlayers[winner].NickName == PhotonNetwork.NickName)
            {
                GameMgr.Instance.uIMgr.EndGame(true);

                WinnerEndGame();
            }


            else
            {
                GameMgr.Instance.uIMgr.EndGame(false);

                StartCoroutine(AliveNumCheck_EndTimer());
            }
        }
    }

    public void WinnerEndGame()
    {
        //API ����
        StartCoroutine(processRequestBetting_Zera_DeclareWinner());
    }


    IEnumerator AliveNumCheck_EndTimer()
    {
        yield return new WaitForSeconds(1);
        EndGame();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        StartCoroutine(OnPlayerLeftRoom_EndTimer());
    }
    IEnumerator OnPlayerLeftRoom_EndTimer()
    {
        yield return new WaitForSeconds(1);
        AliveNumCheck();
    }

    public void EndGame()
    {
        //API ����
        //  StartCoroutine(processRequestBetting_Zera_DeclareWinner());
        //photonView.RPC("EndGame", RpcTarget.All);
        //  EndGame();
        GameMgr.Instance.GameState = false;
        PhotonNetwork.AutomaticallySyncScene = false;
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        PhotonNetwork.LoadLevel("TitleScene");
    }


    //ESC������ ��ư
    public void OnClick_LeaveGame()
    {
        GameMgr.Instance.GameState = false;
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("TitleScene");
        GameMgr.Instance.GameSceneSettingInitializing();
    }











    List<string> sessionIDs = new List<string>();
    string NewBets;
    [PunRPC]
    public void RPC_All_SessionID(string ID)
    {
        sessionIDs.Add(ID);
        if (sessionIDs.Count >= PhotonNetwork.PlayerList.Length && PhotonNetwork.IsMasterClient)
            //API ����
            StartCoroutine(processRequestBetting_Zera());
    }

    [PunRPC]
    public void Get_NewBets(string ID)
    {
        NewBets = ID;
    }







    #region API
    [Header("[��ϵ� ������Ʈ���� ȹ�氡���� API Ű]")]
    string API_KEY = "2tiQsCeKJphmdTcLIbPX0P";


    [Header("[Betting Backend Base URL]")]
    [SerializeField] string FullAppsProductionURL = "https://odin-api.browseosiris.com";
    [SerializeField] string FullAppsStagingURL = "https://odin-api-sat.browseosiris.com";

    string getBaseURL()
    {
        // ���δ��� �ܰ���
        //return FullAppsProductionURL;

        // ������¡ �ܰ�(����)���
        return FullAppsStagingURL;
    }

    //---------------
    // ZERA ����
    public void OnClick_Betting_Zera()//Ŭ����
    {
        StartCoroutine(processRequestBetting_Zera()); //���� ���۽� ���� 
    }
    IEnumerator processRequestBetting_Zera()
    {
        Res_Initialize resBettingPlaceBet = null;
        Req_Initialize reqBettingPlaceBet = new Req_Initialize();
        //
        reqBettingPlaceBet.players_session_id = new string[PhotonNetwork.PlayerList.Length];
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            reqBettingPlaceBet.players_session_id[i] = sessionIDs[i];
        }
        Debug.Log("***********���ÿϷ�**********");
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            Debug.Log(reqBettingPlaceBet.players_session_id[i]);
        }
        Debug.Log("*****************************");
        reqBettingPlaceBet.bet_id = GameMgr.Instance.bets_ID;// resSettigns.data.bets[0]._id;
        Debug.Log(reqBettingPlaceBet.bet_id);
        Debug.Log("*****************************");
        yield return requestCoinPlaceBet(reqBettingPlaceBet, (response) =>
        {
            if (response != null)
            {
                Debug.Log("## CoinPlaceBet : " + response.message);
                resBettingPlaceBet = response;
                NewBets = response.data.betting_id;
                photonView.RPC("Get_NewBets", RpcTarget.All, NewBets);
                Debug.Log("^^^^^^���þ��̵� : " + NewBets);
            }
        });
    }
    delegate void resCallback_BettingPlaceBet(Res_Initialize response);
    IEnumerator requestCoinPlaceBet(Req_Initialize req, resCallback_BettingPlaceBet callback)
    {
        string url = getBaseURL() + "/v1/betting/" + "zera" + "/place-bet";

        string reqJsonData = JsonUtility.ToJson(req);
        Debug.Log(reqJsonData);


        UnityWebRequest www = UnityWebRequest.Post(url, reqJsonData);
        byte[] buff = System.Text.Encoding.UTF8.GetBytes(reqJsonData);
        www.uploadHandler = new UploadHandlerRaw(buff);
        www.SetRequestHeader("api-key", API_KEY);
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();

        // Debug.Log(www.downloadHandler.text);

        Res_Initialize res = JsonUtility.FromJson<Res_Initialize>(www.downloadHandler.text);
        callback(res);

        Debug.Log("�� ����");
    }


    //---------------
    // ZERA ����-����
    public void OnClick_Betting_Zera_DeclareWinner()
    {
        StartCoroutine(processRequestBetting_Zera_DeclareWinner());

    }
    IEnumerator processRequestBetting_Zera_DeclareWinner()
    {
        Res_BettingWinner resBettingDeclareWinner = null;
        Req_BettingWinner reqBettingDeclareWinner = new Req_BettingWinner();
        reqBettingDeclareWinner.betting_id = NewBets;// resSettigns.data.bets[0]._id;
        reqBettingDeclareWinner.winner_player_id = GameMgr.Instance.user_ID;

        Debug.Log("^^^^^^���þ��̵� : " + NewBets);
        yield return requestCoinDeclareWinner(reqBettingDeclareWinner, (response) =>
        {
            if (response != null)
            {
                Debug.Log("## CoinDeclareWinner : " + response.message);
                resBettingDeclareWinner = response;
            }
        });
    }
    delegate void resCallback_BettingDeclareWinner(Res_BettingWinner response);
    IEnumerator requestCoinDeclareWinner(Req_BettingWinner req, resCallback_BettingDeclareWinner callback)
    {
        string url = getBaseURL() + "/v1/betting/" + "zera" + "/declare-winner";

        string reqJsonData = JsonUtility.ToJson(req);
        Debug.Log(reqJsonData);


        UnityWebRequest www = UnityWebRequest.Post(url, reqJsonData);
        byte[] buff = System.Text.Encoding.UTF8.GetBytes(reqJsonData);
        www.uploadHandler = new UploadHandlerRaw(buff);
        www.SetRequestHeader("api-key", API_KEY);
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();

        Debug.Log(www.downloadHandler.text);

        Res_BettingWinner res = JsonUtility.FromJson<Res_BettingWinner>(www.downloadHandler.text);
        callback(res);
        Debug.Log("^^^^^^���þ��̵� : " + NewBets);
        Debug.Log("�� ������");
        EndGame();
    }

    #endregion







}
