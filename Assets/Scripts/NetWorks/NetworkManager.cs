using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.Networking;
using Photon.Realtime;
using TMPro;
using Photon.Pun;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField] Button btnConnect = null;
    [SerializeField] TextMeshProUGUI[] nickName = null;
    //API Balance & UserID
    [SerializeField] TextMeshProUGUI Balance_Disconnect;
    [SerializeField] TextMeshProUGUI Balance_Lobby;
    [SerializeField] TextMeshProUGUI UserID_Disconnect;
    [SerializeField] TextMeshProUGUI UserID_Lobby;
    //API 데이터 전달 포스트맨
    [SerializeField] GameObject gameMgr;

    public AudioSource audioSource;
    public InputField nickNameInput;
    public GameObject logInPanel;
    public GameObject lobbyPanel;
    public GameObject[] readyButton;
    public RawImage[] lobbyTorchlightOn;
    public Image[] lobbyTorchlightOff;

    [Header("LobbyNickNameScene")]
    public Button lobbyButton;
    public RawImage lobbyInsertImage;
    public RawImage lobbyGameLogo;
    public RawImage lobbyleftDoor;
    public RawImage lobbyRightDoor;
    public RawImage lobbyDarkHole;

    private bool lobbyLogin = false;


    private GameObject postman;

    //세션ID 닉네임 연동 
    private Dictionary<string, string> Nick_Session_key = new Dictionary<string, string>();
    private string mySessionID;
    private string myBetsId;
    private int readyCount = 0;
    private int myButtonNum = 0;
    [Header("내상태")]
    public ReadyState myReadyState = ReadyState.None;
    //This is 
    public enum ReadyState
    {
        None,
        Ready,
        UnReady,
    }
    public void SoloClick()
    {
        PhotonNetwork.LoadLevel("LoadingScene");
        //  PhotonNetwork.LoadLevel("GameScene");
    }
    private void Awake()
    {
        //    DontDestroyOnLoad(this);
        ClearLobby();
        photonView.StartCoroutine(AutoSyncDelay());
        if (FindObjectOfType<GameMgr>() == null)
        {
            postman = Instantiate(gameMgr);
        }
        else
        {
            postman = FindObjectOfType<GameMgr>().gameObject;
        }
        Screen.SetResolution(1920, 1080, false);
        PhotonNetwork.SendRate = 300;
        PhotonNetwork.SerializationRate = 150;
    }
    IEnumerator AutoSyncDelay()
    {
        yield return new WaitForSeconds(3f);
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        audioSource.gameObject.SetActive(false);
        for (int i = 0; i < readyButton.Length; i++)
        {
            lobbyTorchlightOn[i].gameObject.SetActive(false);
            lobbyTorchlightOff[i].gameObject.SetActive(false);
            //  soulEff[i].SetActive(false);
            readyButton[i].GetComponent<Image>().color = Color.gray;
            readyButton[i].GetComponent<Button>().interactable = false;
        }
        btnConnect.interactable = false; // 버튼 입력 막기
        lobbyPanel.SetActive(false);
        //마스터 서버 접속 요청
        PhotonNetwork.ConnectUsingSettings(); //Photon.Pun 내부 클래스
    }

    public override void OnConnectedToMaster()
    {
        //API 유저 프로필 , SessionID 가져오기
        StartCoroutine(processRequestGetUserInfo());

        // 불끄기
        ClearLobby();
        Debug.Log("## OnConnected to Master");
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        logInPanel.SetActive(true);
        btnConnect.interactable = false;
        lobbyPanel.SetActive(false);
    }
    // 이름 입력 컨트롤(inputField)
    public void OnEndEdit(string instr)
    {
        Debug.Log("!!!!!");
        PhotonNetwork.NickName = instr; //닉네임 할당
    }

    // 닉네임 밑에 커넥트 버튼 클릭시 
    public void OnClick_Connected()
    {
        StartCoroutine(DoorPos());

        if (string.IsNullOrEmpty(PhotonNetwork.NickName) == true)
            return;

        //조인랜덤룸으로 생성방 우선 참가로직 
        PhotonNetwork.JoinRandomRoom();

        //기존 커넥트 버튼 오프 
        logInPanel.SetActive(false);
        //로비패널 온 
        lobbyPanel.SetActive(true);
    }

    IEnumerator DoorPos()
    {
       // lobbyButton.gameObject.GetComponent<RawImage>().enabled = false;


        lobbyButton.gameObject.SetActive(false);
        lobbyInsertImage.gameObject.SetActive(false);
        lobbyGameLogo.gameObject.SetActive(false);

        //yield return new WaitForSeconds(0.8f);

        float time = 2f;
        while (time > 0)
        {
            lobbyleftDoor.transform.position += Vector3.left * Time.deltaTime * 50f;
            lobbyRightDoor.transform.position += Vector3.right * Time.deltaTime * 50f;
            time -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
          
        }

        lobbyleftDoor.gameObject.SetActive(false);
        lobbyDarkHole.gameObject.SetActive(false);
        lobbyRightDoor.gameObject.SetActive(false);
        lobbyLogin = true;
    }




    //입장할 방이 없으면 새로운 방 생성
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("조인 실패");
        //맥스 인원과 방 상태 표현 (시작인지 아닌지)
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 5, IsOpen = true });
    }
    //자신이 들어갈때 
    public override void OnJoinedRoom()
    {
        Debug.Log("새로운 플레이어가 참가하셨습니다");
        //API 잔고 표시
        StartCoroutine(processRequestZeraBalance());
        //API 세션아이디랑 닉네임 연동 
        Nick_Session_key.Add(PhotonNetwork.NickName, mySessionID);
        //배팅 세팅값 가져오기
        StartCoroutine(processRequestSettings());

        Player[] nickNameCheck = PhotonNetwork.PlayerList;
        int checkNum = 0;
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (nickNameCheck[i].NickName == PhotonNetwork.NickName)
            {
                checkNum++;
                if (checkNum > 1)
                {
                    PhotonNetwork.LeaveRoom();
                    PhotonNetwork.LoadLevel("TitleScene");
                }
            }
        }

        myReadyState = ReadyState.UnReady;
        SortedPlayer();
    }

    //타인이 들어올때
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("새로운 플레이어가 참가하셨습니다");
        SortedPlayer();
    }

    //플레이어가 나갈때
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        ClearLobby();
        SortedPlayer();
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log("마스터 클라이언트 변경:" + newMasterClient.ToString());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
            PhotonNetwork.LoadLevel("TitleScene");
        }
    }

    #region 플레이어 자리 초기화
    public void ClearLobby()
    {
        //대기창 초기화 
        for (int i = 0; i < nickName.Length; i++)
        {
            nickName[i].text = " ";
            lobbyTorchlightOn[i].gameObject.SetActive(false);
            lobbyTorchlightOff[i].gameObject.SetActive(true);
            //soulEff[i].SetActive(false);
            readyButton[i].GetComponent<Image>().color = Color.gray;
            readyButton[i].GetComponent<Button>().interactable = false;
        }
    }
    #endregion


    #region 플레이어 정렬
    public void SortedPlayer()
    {
        gameObject.GetPhotonView().RPC("ZeroCounT", RpcTarget.MasterClient);
        Player[] sortedPlayers = PhotonNetwork.PlayerList;

        for (int i = 0; i < sortedPlayers.Length; i++)
        {
            nickName[i].text = sortedPlayers[i].NickName;
            lobbyTorchlightOn[i].gameObject.SetActive(true);
            lobbyTorchlightOff[i].gameObject.SetActive(false);
            // soulEff[i].SetActive(true);
            //자신의 버튼만 활성화 하기 
            if (sortedPlayers[i].NickName == PhotonNetwork.NickName)
            {
                Debug.Log("i : " + i);
                myButtonNum = i;
                readyButton[myButtonNum].GetComponent<Button>().interactable = true; //나만 누르기 위해 활성화

                //내 상태가 레디면 노란색 -->그런데 이건 서버에서 표현 해줘야 하기 때문에 RPC함수 사용
                gameObject.GetPhotonView().RPC("ButtonColor", RpcTarget.All, myReadyState, myButtonNum);
            }

            if (readyButton[i].GetComponent<Image>().color == Color.yellow)
            {
                gameObject.GetPhotonView().RPC("ReadyCounT", RpcTarget.MasterClient);
            }
        }

    }
    #endregion
    //각각의 플레이어 상태에 따른 색 표현 
    [PunRPC]
    public void ButtonColor(ReadyState readyState, int buttonNum)
    {
        if (readyState == ReadyState.Ready)
            readyButton[buttonNum].GetComponent<Image>().color = Color.yellow;
        else
            readyButton[buttonNum].GetComponent<Image>().color = Color.grey;
    }
    /// <summary>
    /// Start game with in playerList players
    /// </summary>
    #region 게임 실행
    public void LoadScene()
    {
        // 마스터일때만 해당 함수 실행 가능
        if (PhotonNetwork.IsMasterClient)
        {
            if (readyCount == PhotonNetwork.PlayerList.Length && readyCount > 1)
            {
                Debug.Log("시작");
                //5명 레디 완료시 2초후 게임 실행 코루틴 

                PhotonNetwork.CurrentRoom.IsOpen = false;
                photonView.StartCoroutine(MainStartTimer());
            }
        }
    }
    #endregion

    [PunRPC]
    void ReadyCounT()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            readyCount++;
            LoadScene();
            Debug.Log("레디 숫자 : " + readyCount);
        }
    }
    [PunRPC]
    void ZeroCounT()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            readyCount = 0;
            Debug.Log("레디 숫자 : " + readyCount);
        }
    }


    #region 버튼 클릭
    public void ButtonClick()
    {
        gameObject.GetPhotonView().RPC("ZeroCounT", RpcTarget.MasterClient);
        if (myReadyState == ReadyState.Ready)
        {
            myReadyState = ReadyState.UnReady;
            SortedPlayer();
        }
        else
        {
            myReadyState = ReadyState.Ready;
            SortedPlayer();
        }
    }
    #endregion 
    //게임 시작 2초 지연
    [PunRPC]
    public void RPC_ClearLobby()
    {
        ClearLobby();
    }
    IEnumerator MainStartTimer()
    {
        yield return new WaitForSeconds(2);
        if (readyCount == PhotonNetwork.PlayerList.Length && readyCount > 1)
        {
            PhotonNetwork.LoadLevel("LoadingScene");
        }
        else
        {
            Debug.Log("누군가 레디 취소함");
            PhotonNetwork.CurrentRoom.IsOpen = true;
        }
    }

    /* IEnumerator broken()
     {
         int ran = Random.Range(12, 22);
         yield return new WaitForSeconds(ran);

         brokenWindow.gameObject.SetActive(true);
         audioSource.gameObject.SetActive(true);
     }*/

    //---------------------------------------------------------------------------------------------------------------------------------------------

    #region API호출 함수
    [Header("[API 관련]")]
    // [SerializeField] TextMeshProUGUI txtInputField;
    [SerializeField] string selectedBettingID;

    [Header("[등록된 프로젝트에서 획득가능한 API 키]")]
    [SerializeField] string API_KEY = "";

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

    Res_UserProfile res_UserProfile = null;
    Res_UserSessionID res_UserSessionID = null;
    Res_BettingSetting res_BettingSetting = null;
    //---------------
    // 유저 정보
    public void OnClick_GetUserProfile() //버튼 사용시 
    {
        StartCoroutine(processRequestGetUserInfo());
    }
    IEnumerator processRequestGetUserInfo()
    {
        // 유저 정보
        yield return requestGetUserInfo((response) =>
        {
            if (response != null)
            {
                Debug.Log("## " + response.ToString());
                res_UserProfile = response;
                Debug.Log(res_UserProfile.userProfile.username);
            }
        });
        btnConnect.interactable = true;
    }
    delegate void resCallback_GetUserInfo(Res_UserProfile response);
    IEnumerator requestGetUserInfo(resCallback_GetUserInfo callback)
    {
        // get user profile
        UnityWebRequest www = UnityWebRequest.Get("http://localhost:8546/api/getuserprofile");
        yield return www.SendWebRequest();
        Debug.Log(www.downloadHandler.text);
        //  txtInputField.text = www.downloadHandler.text;
        Res_UserProfile res_getUserProfile = JsonUtility.FromJson<Res_UserProfile>(www.downloadHandler.text);
        UserID_Disconnect.text = "User ID : " + res_getUserProfile.userProfile.username;
        UserID_Lobby.text = "User ID : " + res_getUserProfile.userProfile.username;

        postman.SendMessage("User_ID", res_getUserProfile.userProfile._id, SendMessageOptions.DontRequireReceiver);

        callback(res_getUserProfile);

        //아래 SessionID까지 일괄 처리 
        StartCoroutine(processRequestGetSessionID());
    }

    //---------------
    // Session ID
    public void OnClick_GetSessionID() //버튼 사용시 
    {
        StartCoroutine(processRequestGetSessionID());
    }
    IEnumerator processRequestGetSessionID()
    {
        // 유저 정보
        yield return requestGetSessionID((response) =>
        {
            if (response != null)
            {
                Debug.Log("## " + response.ToString());
                res_UserSessionID = response;
            }
        });
    }
    delegate void resCallback_GetSessionID(Res_UserSessionID response);
    IEnumerator requestGetSessionID(resCallback_GetSessionID callback)
    {
        // get session id
        UnityWebRequest www = UnityWebRequest.Get("http://localhost:8546/api/getsessionid");
        yield return www.SendWebRequest();
        Debug.Log("음.." + www.downloadHandler.text);
        //  txtInputField.text = www.downloadHandler.text;
        Res_UserSessionID res_getSessionID = JsonUtility.FromJson<Res_UserSessionID>(www.downloadHandler.text);

        mySessionID = res_getSessionID.sessionId;
        postman.SendMessage("Session_ID", mySessionID, SendMessageOptions.DontRequireReceiver);

        callback(res_getSessionID);

        //API 잔고 가져오기
        StartCoroutine(processRequestZeraBalance());
    }

    //---------------
    // 베팅관련 셋팅 정보를 얻어오기
    public void OnClick_Settings()//버튼 클릭시
    {
        StartCoroutine(processRequestSettings());//방 입장시
    }
    IEnumerator processRequestSettings()
    {
        yield return requestSettings((response) =>
        {
            if (response != null)
            {
                Debug.Log("## Settings : " + response.ToString());
                res_BettingSetting = response;
            }
        });
    }
    delegate void resCallback_Settings(Res_BettingSetting response);
    IEnumerator requestSettings(resCallback_Settings callback)
    {
        string url = getBaseURL() + "/v1/betting/settings";


        UnityWebRequest www = UnityWebRequest.Get(url);
        www.SetRequestHeader("api-key", API_KEY);
        yield return www.SendWebRequest();
        Debug.Log(www.downloadHandler.text);
        //  txtInputField.text = www.downloadHandler.text;

        Res_BettingSetting res = JsonUtility.FromJson<Res_BettingSetting>(www.downloadHandler.text);
        myBetsId = res.data.bets[0]._id;
        Debug.Log("내 베팅 아이디 : " + myBetsId);


        postman.SendMessage("Bets_ID", myBetsId, SendMessageOptions.DontRequireReceiver);


        callback(res);
        //UnityWebRequest www = new UnityWebRequest(URL);
    }

    //---------------
    // Zera 잔고 확인
    public void OnClick_ZeraBalance() //버튼 클릭시 사용
    {
        StartCoroutine(processRequestZeraBalance()); //방 입장 전과 후마다 호출
    }
    IEnumerator processRequestZeraBalance()
    {
        yield return requestZeraBalance(res_UserSessionID.sessionId, (response) =>
        {
            if (response != null)
            {
                Debug.Log("## Response Zera Balance : " + response.ToString());
            }
        });
    }
    delegate void resCallback_BalanceInfo(Res_ZeraBalance response);
    IEnumerator requestZeraBalance(string sessionID, resCallback_BalanceInfo callback)
    {
        string url = getBaseURL() + ("/v1/betting/" + "zera" + "/balance/" + sessionID);

        UnityWebRequest www = UnityWebRequest.Get(url);
        www.SetRequestHeader("api-key", API_KEY);
        yield return www.SendWebRequest();
        Debug.Log(www.downloadHandler.text);
        // txtInputField.text = www.downloadHandler.text;
        Balance_Disconnect.text = www.downloadHandler.text;
        Balance_Lobby.text = www.downloadHandler.text;

        Res_ZeraBalance res = JsonUtility.FromJson<Res_ZeraBalance>(www.downloadHandler.text);
        Balance_Disconnect.text = "Balance : " + res.data.balance.ToString();
        Balance_Lobby.text = "Balance : " + res.data.balance.ToString();
        callback(res);
        //UnityWebRequest www = new UnityWebRequest(URL);
    }
    #endregion
}
