using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.Networking;
using Photon.Realtime;
using TMPro;
using Photon.Pun;
using System.Text.RegularExpressions;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField] Button btnConnect = null;
    [SerializeField] Text[] nickName = null;
    //API Balance & UserID
    [SerializeField] TextMeshProUGUI Balance_Disconnect;
    [SerializeField] TextMeshProUGUI Balance_Lobby;
    [SerializeField] TextMeshProUGUI UserID_Disconnect;
    [SerializeField] TextMeshProUGUI UserID_Lobby;
    //API ������ ���� ����Ʈ��
    [SerializeField] GameObject gameMgr;

    public AudioSource audioSource;
    public InputField nickNameInput;
    public GameObject logInPanel;
    public GameObject lobbyPanel;
    public GameObject[] readyButton;
    public GameObject[] lobbyTorchlightOn;
    public GameObject[] lobbyTorchlightOff;
    public GameObject vote;
    public GameObject nickText;
    public Text voteText;
    public Text voteCountText;

    [Header("LobbyNickNameScene")]
    public Button agreeButton;
    public Button theOppositeCountButton;
    public Button lobbyButton;
    public RawImage lobbyInsertImage;
    public RawImage lobbyGameLogo;
    public RawImage lobbyleftDoor;
    public RawImage lobbyRightDoor;
    public RawImage lobbyDarkHole;
    private bool fadeIn;
    private bool lobbyLogin;


    private GameObject postman;

    //����ID �г��� ���� 
    private Dictionary<string, string> Nick_Session_key = new Dictionary<string, string>();
    private string mySessionID;
    private string myBetsId;
    private int readyCount = 0;
    private int myButtonNum = 0;
    [Header("������")]
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
        Application.targetFrameRate = 60;
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
            readyButton[i].gameObject.SetActive(false);
            //  soulEff[i].SetActive(false);
            readyButton[i].GetComponent<Image>().color = Color.gray;
            readyButton[i].GetComponent<Button>().interactable = false;
        }
        btnConnect.interactable = false; // ��ư �Է� ����
        lobbyPanel.SetActive(false);
        //������ ���� ���� ��û
        PhotonNetwork.ConnectUsingSettings(); //Photon.Pun ���� Ŭ����
        Debug.Log(PhotonNetwork.NetworkClientState + "*********************");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log(PhotonNetwork.NetworkClientState + "*********************");

        //API ���� ������ , SessionID ��������
        StartCoroutine(processRequestGetUserInfo());

        // �Ҳ���
        ClearLobby();
        Debug.Log("## OnConnected to Master");
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        logInPanel.SetActive(true);
        lobbyButton.interactable = false;
        btnConnect.interactable = false;
        lobbyPanel.SetActive(false);
    }
    // �̸� �Է� ��Ʈ��(inputField)
    bool edit;
    public void OnEndEdit(string instr)
    {
        // if (Regex.IsMatch(instr, @"[��-����-�R]")!=true) return;
        edit = true;
        if (Regex.IsMatch(instr, @"^[a-zA-Z]+[0-9]*$") != true)
        {
            PhotonNetwork.NickName = instr;
            nickText.SetActive(true);
            return;
        }
        Debug.Log("!!!!!");
        PhotonNetwork.NickName = instr; //�г��� �Ҵ�
    }

    // �г��� �ؿ� Ŀ��Ʈ ��ư Ŭ���� 
    public void OnClick_Connected()
    {
        if (!edit) return;
        if (Regex.IsMatch(PhotonNetwork.NickName, @"^[a-zA-Z]+[0-9]*$") != true) return;
        nickText.SetActive(false);
        StartCoroutine(DoorPos());

        if (string.IsNullOrEmpty(PhotonNetwork.NickName) == true)
            return;

        //���η��������� ������ �켱 �������� 
        PhotonNetwork.JoinRandomRoom();
        //���� Ŀ��Ʈ ��ư ���� 
        logInPanel.SetActive(false);
        StartCoroutine(PannelOn());
    }

    IEnumerator PannelOn()
    {
        gameObject.GetPhotonView().RPC("DropOutBool", RpcTarget.All, true);
        yield return new WaitForSeconds(9.5f);
        lobbyPanel.SetActive(true);
        logoFadeOut.LobbyFadeIn();
        yield return new WaitForSeconds(1.5f);
        fadeIn = true;
        gameObject.GetPhotonView().RPC("DropOutBool", RpcTarget.All,false);

        //�κ��г� �� 
    }


    [SerializeField] LogoFadeOut logoFadeOut;

    IEnumerator DoorPos()
    {
        // lobbyButton.gameObject.GetComponent<RawImage>().enabled = false;
        lobbyButton.gameObject.SetActive(false);
        lobbyInsertImage.gameObject.SetActive(false);
        // lobbyGameLogo.gameObject.SetActive(false);

        //yield return new WaitForSeconds(0.8f);

        float time = 2f;
        while (time > 0)
        {
            if (time > 1f)
            {
                lobbyleftDoor.transform.position += Vector3.left * Time.deltaTime * 50f;
                lobbyRightDoor.transform.position += Vector3.right * Time.deltaTime * 50f;
            }
            else
            {
                lobbyleftDoor.transform.position += Vector3.left * Time.deltaTime * 800f;
                lobbyRightDoor.transform.position += Vector3.right * Time.deltaTime * 800f;
            }
            time -= Time.deltaTime;
            yield return new WaitForFixedUpdate();

        }
        logoFadeOut.DarkHoleFadeOut();
        lobbyleftDoor.gameObject.SetActive(false);
        lobbyRightDoor.gameObject.SetActive(false);
        lobbyLogin = true;
    }




    //������ ���� ������ ���ο� �� ����
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("���� ����");
        //�ƽ� �ο��� �� ���� ǥ�� (�������� �ƴ���)
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 5, IsOpen = true });
    }
    //�ڽ��� ���� 
    public override void OnJoinedRoom()
    {
        Debug.Log("���ο� �÷��̾ �����ϼ̽��ϴ�");
        //API �ܰ� ǥ��
        StartCoroutine(processRequestZeraBalance());
        //API ���Ǿ��̵�� �г��� ���� 
        Nick_Session_key.Add(PhotonNetwork.NickName, mySessionID);
        //���� ���ð� ��������
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
        gameObject.GetPhotonView().RPC("DropOutBool", RpcTarget.All, true);
        SortedPlayer();
    }

    //Ÿ���� ���ö�
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("���ο� �÷��̾ �����ϼ̽��ϴ�");
        SortedPlayer();
    }

    //�÷��̾ ������
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        ClearLobby();
        SortedPlayer();
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log("������ Ŭ���̾�Ʈ ����:" + newMasterClient.ToString());
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && PhotonNetwork.IsConnected&&(fadeIn==true|| lobbyLogin==false))
        {
            PhotonNetwork.Disconnect();
            PhotonNetwork.LoadLevel("TitleScene");
        }
    }

    #region �÷��̾� �ڸ� �ʱ�ȭ
    public void ClearLobby()
    {
        //���â �ʱ�ȭ 
        for (int i = 0; i < nickName.Length; i++)
        {
            nickName[i].text = " ";
            lobbyTorchlightOn[i].gameObject.SetActive(false);
            lobbyTorchlightOff[i].gameObject.SetActive(false);
            //soulEff[i].SetActive(false);
            readyButton[i].GetComponent<Image>().color = Color.gray;
            readyButton[i].GetComponent<Button>().interactable = false;
        }
    }
    #endregion


    #region �÷��̾� ����
    public void SortedPlayer()
    {
        gameObject.GetPhotonView().RPC("ZeroCounT", RpcTarget.MasterClient);
        Player[] sortedPlayers = PhotonNetwork.PlayerList;

        for (int i = 0; i < sortedPlayers.Length; i++)
        {
            nickName[i].text = sortedPlayers[i].NickName;
            lobbyTorchlightOn[i].gameObject.SetActive(true);
            lobbyTorchlightOff[i].gameObject.SetActive(true);
            readyButton[i].gameObject.SetActive(true);
            // soulEff[i].SetActive(true);
            //�ڽ��� ��ư�� Ȱ��ȭ �ϱ� 
            if (sortedPlayers[i].NickName == PhotonNetwork.NickName)
            {
                Debug.Log("i : " + i);
                myButtonNum = i;
                readyButton[myButtonNum].GetComponent<Button>().interactable = true; //���� ������ ���� Ȱ��ȭ

                //�� ���°� ����� ����� -->�׷��� �̰� �������� ǥ�� ����� �ϱ� ������ RPC�Լ� ���
                gameObject.GetPhotonView().RPC("ButtonColor", RpcTarget.All, myReadyState, myButtonNum);
            }

            if (readyButton[i].GetComponent<Image>().color == Color.yellow)
            {
                gameObject.GetPhotonView().RPC("ReadyCounT", RpcTarget.MasterClient);
            }
        }

    }
    #endregion
    //������ �÷��̾� ���¿� ���� �� ǥ�� 
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
    #region ���� ����
    public void LoadScene()
    {
        // �������϶��� �ش� �Լ� ���� ����
        if (PhotonNetwork.IsMasterClient)
        {
            if (readyCount == PhotonNetwork.PlayerList.Length && readyCount > 1)
            {
                Debug.Log("����");
                //5�� ���� �Ϸ�� 2���� ���� ���� �ڷ�ƾ 

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
            Debug.Log("���� ���� : " + readyCount);
        }
    }
    [PunRPC]
    void ZeroCounT()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            readyCount = 0;
            Debug.Log("���� ���� : " + readyCount);
        }
    }


    #region ��ư Ŭ��
    public void ButtonClick()
    {

        if (fadeIn == false) return;
        if (isDropOut == true) return;
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
    #region �����ư
    private int playerDropOutNum;
    private bool isDropOut;
    private bool isVote;
    private int agreeCount;
    private int theOppositeCount;

    public void DropOutClick0()
    {
        if (isDropOut == true||fadeIn == false) return;
        if (PhotonNetwork.PlayerList[0].NickName == PhotonNetwork.NickName) return;
        if (readyButton[0].GetComponent<Image>().color == Color.yellow) return;
        gameObject.GetPhotonView().RPC("DropOutNum", RpcTarget.All, 0);
    }
    public void DropOutClick1()
    {
        if (isDropOut == true || fadeIn == false) return;
        if (PhotonNetwork.PlayerList[1].NickName == PhotonNetwork.NickName) return;
        if (readyButton[1].GetComponent<Image>().color == Color.yellow) return;
        gameObject.GetPhotonView().RPC("DropOutNum", RpcTarget.All, 1);
    }
    public void DropOutClick2()
    {
        if (isDropOut == true || fadeIn == false) return;
        if (PhotonNetwork.PlayerList[2].NickName == PhotonNetwork.NickName) return;
        if (readyButton[2].GetComponent<Image>().color == Color.yellow) return;
        gameObject.GetPhotonView().RPC("DropOutNum", RpcTarget.All, 2);
    }
    public void DropOutClick3()
    {
        if (isDropOut == true || fadeIn == false) return;
        if (PhotonNetwork.PlayerList[3].NickName == PhotonNetwork.NickName) return;
        if (readyButton[3].GetComponent<Image>().color == Color.yellow) return;
        gameObject.GetPhotonView().RPC("DropOutNum", RpcTarget.All, 3);
    }
    public void DropOutClick4()
    {
        if (isDropOut == true || fadeIn == false) return;
        if (PhotonNetwork.PlayerList[4].NickName == PhotonNetwork.NickName) return;
        if (readyButton[4].GetComponent<Image>().color == Color.yellow) return;
        gameObject.GetPhotonView().RPC("DropOutNum", RpcTarget.All, 4);
    }
    public void Agree()
    {
        if (isVote == true) return;
        isVote = true;
        gameObject.GetPhotonView().RPC("VoteMaster", RpcTarget.All, true);
        agreeButton.GetComponent<Image>().color = Color.yellow;

    }
    public void TheOpposite()
    {
        if (isVote == true) return;
        isVote = true;
        gameObject.GetPhotonView().RPC("VoteMaster", RpcTarget.All, false);
        theOppositeCountButton.GetComponent<Image>().color = Color.yellow;
    }
    [PunRPC]
    public void VoteMaster(bool vote)
    {
        if (vote == true) agreeCount++;
        else if (vote == false) theOppositeCount++;

    }
    private int boolCount;
    [PunRPC]
    public void DropOutBool(bool isBool)
    {
        if (photonView.IsMine) return;
        if (isBool == true)
        {
            boolCount++;
            isDropOut = isBool;
        }
        else if (boolCount == 1 && isBool == false)
        {
            isDropOut = isBool;
            boolCount = 0;
        }
        else
        {
            boolCount--;
        }
    }
    [PunRPC]
    public void DropOutNum(int Num)
    {
        if (Num == 5)
        {
            if (PhotonNetwork.IsMasterClient == true) PhotonNetwork.CurrentRoom.IsOpen = true;
            isDropOut = false;
            isVote = false;
            vote.SetActive(false);
            agreeCount = 0;
            theOppositeCount = 0;
            theOppositeCountButton.GetComponent<Image>().color = Color.white;
            agreeButton.GetComponent<Image>().color = Color.white;

        }
        else
        {
            if (PhotonNetwork.IsMasterClient == true) PhotonNetwork.CurrentRoom.IsOpen = false;
            isDropOut = true;
            playerDropOutNum = Num;
            vote.SetActive(true);
            voteText.text = $"{PhotonNetwork.PlayerList[Num].NickName} Player Drop Out";
            StartCoroutine(DropOutNum_Delay());
        }
    }
    IEnumerator DropOutNum_Delay()
    {
        voteCountText.text = "5";
        yield return new WaitForSeconds(1f);
        voteCountText.text = "4";
        yield return new WaitForSeconds(1f);
        voteCountText.text = "3";
        yield return new WaitForSeconds(1f);
        voteCountText.text = "2";
        yield return new WaitForSeconds(1f);
        voteCountText.text = "1";
        yield return new WaitForSeconds(1f);
        if (PhotonNetwork.IsMasterClient == true)
        {
            if (agreeCount > theOppositeCount)
            {
                gameObject.GetPhotonView().RPC("GetOutHere", RpcTarget.All, playerDropOutNum);
            }
            gameObject.GetPhotonView().RPC("DropOutNum", RpcTarget.All, 5);
        }

    }
    [PunRPC]
    public void GetOutHere(int Num)
    {
        if (PhotonNetwork.PlayerList[Num].NickName != PhotonNetwork.NickName) return;
        StartCoroutine(GetOutHere_Delay());
    }
    IEnumerator GetOutHere_Delay()
    {
        yield return new WaitForSeconds(1f);
        PhotonNetwork.Disconnect();
        PhotonNetwork.LoadLevel("TitleScene");
    }
    #endregion 
    //���� ���� 2�� ����
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
            Debug.Log("������ ���� �����");
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

    #region APIȣ�� �Լ�
    [Header("[API ����]")]
    // [SerializeField] TextMeshProUGUI txtInputField;
    [SerializeField] string selectedBettingID;

    [Header("[��ϵ� ������Ʈ���� ȹ�氡���� API Ű]")]
    [SerializeField] string API_KEY = "";

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

    Res_UserProfile res_UserProfile = null;
    Res_UserSessionID res_UserSessionID = null;
    Res_BettingSetting res_BettingSetting = null;
    //---------------
    // ���� ����
    public void OnClick_GetUserProfile() //��ư ���� 
    {
        StartCoroutine(processRequestGetUserInfo());
    }
    IEnumerator processRequestGetUserInfo()
    {
        // ���� ����
        yield return requestGetUserInfo((response) =>
        {
            if (response != null)
            {
                Debug.Log("## " + response.ToString());
                res_UserProfile = response;
                Debug.Log(res_UserProfile.userProfile.username);
                lobbyButton.interactable = true;
                btnConnect.interactable = true;
            }
        });

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

        //�Ʒ� SessionID���� �ϰ� ó�� 
        StartCoroutine(processRequestGetSessionID());
    }

    //---------------
    // Session ID
    public void OnClick_GetSessionID() //��ư ���� 
    {
        StartCoroutine(processRequestGetSessionID());
    }
    IEnumerator processRequestGetSessionID()
    {
        // ���� ����
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
        Debug.Log("��.." + www.downloadHandler.text);
        //  txtInputField.text = www.downloadHandler.text;
        Res_UserSessionID res_getSessionID = JsonUtility.FromJson<Res_UserSessionID>(www.downloadHandler.text);

        mySessionID = res_getSessionID.sessionId;
        postman.SendMessage("Session_ID", mySessionID, SendMessageOptions.DontRequireReceiver);

        callback(res_getSessionID);

        //API �ܰ� ��������
        StartCoroutine(processRequestZeraBalance());
    }

    //---------------
    // ���ð��� ���� ������ ������
    public void OnClick_Settings()//��ư Ŭ����
    {
        StartCoroutine(processRequestSettings());//�� �����
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
        Debug.Log("�� ���� ���̵� : " + myBetsId);


        postman.SendMessage("Bets_ID", myBetsId, SendMessageOptions.DontRequireReceiver);


        callback(res);
        //UnityWebRequest www = new UnityWebRequest(URL);
    }

    //---------------
    // Zera �ܰ� Ȯ��
    public void OnClick_ZeraBalance() //��ư Ŭ���� ���
    {
        StartCoroutine(processRequestZeraBalance()); //�� ���� ���� �ĸ��� ȣ��
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
