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
    public GameObject PunFindObject(int viewID3)//뷰아이디를 넘겨받아 포톤상의 오브젝트를 찾는다.
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
        //상태가 Die가 아니라면 살아있는 것이기 때문에 살아남은 인원 카운트 가능 
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
            //승자
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
        //API 승자
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
        //API 승자
        //  StartCoroutine(processRequestBetting_Zera_DeclareWinner());
        //photonView.RPC("EndGame", RpcTarget.All);
        //  EndGame();
        GameMgr.Instance.GameState = false;
        PhotonNetwork.AutomaticallySyncScene = false;
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        PhotonNetwork.LoadLevel("TitleScene");
    }


    //ESC나가기 버튼
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
            //API 베팅
            StartCoroutine(processRequestBetting_Zera());
    }

    [PunRPC]
    public void Get_NewBets(string ID)
    {
        NewBets = ID;
    }







    #region API
    [Header("[등록된 프로젝트에서 획득가능한 API 키]")]
    string API_KEY = "2tiQsCeKJphmdTcLIbPX0P";


    [Header("[Betting Backend Base URL]")]
    [SerializeField] string FullAppsProductionURL = "https://odin-api.browseosiris.com";
    [SerializeField] string FullAppsStagingURL = "https://odin-api-sat.browseosiris.com";

    string getBaseURL()
    {
        // 프로덕션 단계라면
        //return FullAppsProductionURL;

        // 스테이징 단계(개발)라면
        return FullAppsStagingURL;
    }

    //---------------
    // ZERA 베팅
    public void OnClick_Betting_Zera()//클릭시
    {
        StartCoroutine(processRequestBetting_Zera()); //게임 시작시 실행 
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
        Debug.Log("***********베팅완료**********");
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
                Debug.Log("^^^^^^배팅아이디 : " + NewBets);
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

        Debug.Log("돈 냈음");
    }


    //---------------
    // ZERA 베팅-승자
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

        Debug.Log("^^^^^^배팅아이디 : " + NewBets);
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
        Debug.Log("^^^^^^배팅아이디 : " + NewBets);
        Debug.Log("돈 가져와");
        EndGame();
    }

    #endregion







}
