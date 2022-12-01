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
    //API µ¥ÀÌÅÍ Àü´Þ Æ÷½ºÆ®¸Ç
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

    //¼¼¼ÇID ´Ð³×ÀÓ ¿¬µ¿ 
    private Dictionary<string, string> Nick_Session_key = new Dictionary<string, string>();
    private string mySessionID;
    private string myBetsId;
    private int readyCount = 0;
    private int myButtonNum = 0;
    [Header("³»»óÅÂ")]
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
        btnConnect.interactable = false; // ¹öÆ° ÀÔ·Â ¸·±â
        lobbyPanel.SetActive(false);
        //¸¶½ºÅÍ ¼­¹ö Á¢¼Ó ¿äÃ»
        PhotonNetwork.ConnectUsingSettings(); //Photon.Pun ³»ºÎ Å¬·¡½º
        Debug.Log(PhotonNetwork.NetworkClientState + "*********************");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log(PhotonNetwork.NetworkClientState + "*********************");

        //API À¯Àú ÇÁ·ÎÇÊ , SessionID °¡Á®¿À±â
        StartCoroutine(processRequestGetUserInfo());

        // ºÒ²ô±â
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
    // ÀÌ¸§ ÀÔ·Â ÄÁÆ®·Ñ(inputField)
    bool edit;
    public void OnEndEdit(string instr)
    {
        // if (Regex.IsMatch(instr, @"[¤¡-¤¾°¡-ÆR]")!=true) return;
        edit = true;
        if (Regex.IsMatch(instr, @"^[a-zA-Z]+[0-9]*$") != true)
        {
            PhotonNetwork.NickName = instr;
            nickText.SetActive(true);
            return;
        }
        Debug.Log("!!!!!");
        PhotonNetwork.NickName = instr; //´Ð³×ÀÓ ÇÒ´ç
    }

    // ´Ð³×ÀÓ ¹Ø¿¡ Ä¿³ØÆ® ¹öÆ° Å¬¸¯½Ã 
    public void OnClick_Connected()
    {
        if (!edit) return;
        if (Regex.IsMatch(PhotonNetwork.NickName, @"^[a-zA-Z]+[0-9]*$") != true) return;
        nickText.SetActive(false);
        StartCoroutine(DoorPos());

        if (string.IsNullOrEmpty(PhotonNetwork.NickName) == true)
            return;

        //Á¶ÀÎ·£´ý·ëÀ¸·Î »ý¼º¹æ ¿ì¼± Âü°¡·ÎÁ÷ 
        PhotonNetwork.JoinRandomRoom();
        //±âÁ¸ Ä¿³ØÆ® ¹öÆ° ¿ÀÇÁ 
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

        //·ÎºñÆÐ³Î ¿Â 
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




    //ÀÔÀåÇÒ ¹æÀÌ ¾øÀ¸¸é »õ·Î¿î ¹æ »ý¼º
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Á¶ÀÎ ½ÇÆÐ");
        //¸Æ½º ÀÎ¿ø°ú ¹æ »óÅÂ Ç¥Çö (½ÃÀÛÀÎÁö ¾Æ´ÑÁö)
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 5, IsOpen = true });
    }
    //ÀÚ½ÅÀÌ µé¾î°¥¶§ 
    public override void OnJoinedRoom()
    {
        Debug.Log("»õ·Î¿î ÇÃ·¹ÀÌ¾î°¡ Âü°¡ÇÏ¼Ì½À´Ï´Ù");
        //API ÀÜ°í Ç¥½Ã
        StartCoroutine(processRequestZeraBalance());
        //API ¼¼¼Ç¾ÆÀÌµð¶û ´Ð³×ÀÓ ¿¬µ¿ 
        Nick_Session_key.Add(PhotonNetwork.NickName, mySessionID);
        //¹èÆÃ ¼¼ÆÃ°ª °¡Á®¿À±â
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

    //Å¸ÀÎÀÌ µé¾î¿Ã¶§
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("»õ·Î¿î ÇÃ·¹ÀÌ¾î°¡ Âü°¡ÇÏ¼Ì½À´Ï´Ù");
        SortedPlayer();
    }

    //ÇÃ·¹ÀÌ¾î°¡ ³ª°¥¶§
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        ClearLobby();
        SortedPlayer();
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log("¸¶½ºÅÍ Å¬¶óÀÌ¾ðÆ® º¯°æ:" + newMasterClient.ToString());
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && PhotonNetwork.IsConnected&&(fadeIn==true|| lobbyLogin==false))
        {
            PhotonNetwork.Disconnect();
            PhotonNetwork.LoadLevel("TitleScene");
        }
    }

    #region ÇÃ·¹ÀÌ¾î ÀÚ¸® ÃÊ±âÈ­
    public void ClearLobby()
    {
        //´ë±âÃ¢ ÃÊ±âÈ­ 
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


    #region ÇÃ·¹ÀÌ¾î Á¤·Ä
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
            //ÀÚ½ÅÀÇ ¹öÆ°¸¸ È°¼ºÈ­ ÇÏ±â 
            if (sortedPlayers[i].NickName == PhotonNetwork.NickName)
            {
                Debug.Log("i : " + i);
                myButtonNum = i;
                readyButton[myButtonNum].GetComponent<Button>().interactable = true; //³ª¸¸ ´©¸£±â À§ÇØ È°¼ºÈ­

                //³» »óÅÂ°¡ ·¹µð¸é ³ë¶õ»ö -->±×·±µ¥ ÀÌ°Ç ¼­¹ö¿¡¼­ Ç¥Çö ÇØÁà¾ß ÇÏ±â ¶§¹®¿¡ RPCÇÔ¼ö »ç¿ë
                gameObject.GetPhotonView().RPC("ButtonColor", RpcTarget.All, myReadyState, myButtonNum);
            }

            if (readyButton[i].GetComponent<Image>().color == Color.yellow)
            {
                gameObject.GetPhotonView().RPC("ReadyCounT", RpcTarget.MasterClient);
            }
        }

    }
    #endregion
    //°¢°¢ÀÇ ÇÃ·¹ÀÌ¾î »óÅÂ¿¡ µû¸¥ »ö Ç¥Çö 
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
    #region °ÔÀÓ ½ÇÇà
    public void LoadScene()
    {
        // ¸¶½ºÅÍÀÏ¶§¸¸ ÇØ´ç ÇÔ¼ö ½ÇÇà °¡´É
        if (PhotonNetwork.IsMasterClient)
        {
            if (readyCount == PhotonNetwork.PlayerList.Length && readyCount > 1)
            {
                Debug.Log("½ÃÀÛ");
                //5¸í ·¹µð ¿Ï·á½Ã 2ÃÊÈÄ °ÔÀÓ ½ÇÇà ÄÚ·çÆ¾ 

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
            Debug.Log("·¹µð ¼ýÀÚ : " + readyCount);
        }
    }
    [PunRPC]
    void ZeroCounT()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            readyCount = 0;
            Debug.Log("·¹µð ¼ýÀÚ : " + readyCount);
        }
    }


    #region ¹öÆ° Å¬¸¯
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
    #region °­Åð¹öÆ°
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
    //°ÔÀÓ ½ÃÀÛ 2ÃÊ Áö¿¬
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
            Debug.Log("´©±º°¡ ·¹µð Ãë¼ÒÇÔ");
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

    #region APIÈ£Ãâ ÇÔ¼ö
    [Header("[API °ü·Ã]")]
    // [SerializeField] TextMeshProUGUI txtInputField;
    [SerializeField] string selectedBettingID;

    [Header("[µî·ÏµÈ ÇÁ·ÎÁ§Æ®¿¡¼­ È¹µæ°¡´ÉÇÑ API Å°]")]
    [SerializeField] string API_KEY = "";

    [Header("[Betting Backend Base URL]")]
    [SerializeField] string FullAppsProductionURL = "https://odin-api.browseosiris.com";
    [SerializeField] string FullAppsStagingURL = "https://odin-api-sat.browseosiris.com";

    string getBaseURL()
    {
        // ÇÁ·Î´ö¼Ç ´Ü°è¶ó¸é
        //return FullAppsProductionURL;

        // ½ºÅ×ÀÌÂ¡ ´Ü°è(°³¹ß)¶ó¸é
        return FullAppsStagingURL;
    }

    Res_UserProfile res_UserProfile = null;
    Res_UserSessionID res_UserSessionID = null;
    Res_BettingSetting res_BettingSetting = null;
    //---------------
    // À¯Àú Á¤º¸
    public void OnClick_GetUserProfile() //¹öÆ° »ç¿ë½Ã 
    {
        StartCoroutine(processRequestGetUserInfo());
    }
    IEnumerator processRequestGetUserInfo()
    {
        // À¯Àú Á¤º¸
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

        //¾Æ·¡ SessionID±îÁö ÀÏ°ý Ã³¸® 
        StartCoroutine(processRequestGetSessionID());
    }

    //---------------
    // Session ID
    public void OnClick_GetSessionID() //¹öÆ° »ç¿ë½Ã 
    {
        StartCoroutine(processRequestGetSessionID());
    }
    IEnumerator processRequestGetSessionID()
    {
        // À¯Àú Á¤º¸
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
        Debug.Log("À½.." + www.downloadHandler.text);
        //  txtInputField.text = www.downloadHandler.text;
        Res_UserSessionID res_getSessionID = JsonUtility.FromJson<Res_UserSessionID>(www.downloadHandler.text);

        mySessionID = res_getSessionID.sessionId;
        postman.SendMessage("Session_ID", mySessionID, SendMessageOptions.DontRequireReceiver);

        callback(res_getSessionID);

        //API ÀÜ°í °¡Á®¿À±â
        StartCoroutine(processRequestZeraBalance());
    }

    //---------------
    // º£ÆÃ°ü·Ã ¼ÂÆÃ Á¤º¸¸¦ ¾ò¾î¿À±â
    public void OnClick_Settings()//¹öÆ° Å¬¸¯½Ã
    {
        StartCoroutine(processRequestSettings());//¹æ ÀÔÀå½Ã
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
        Debug.Log("³» º£ÆÃ ¾ÆÀÌµð : " + myBetsId);


        postman.SendMessage("Bets_ID", myBetsId, SendMessageOptions.DontRequireReceiver);


        callback(res);
        //UnityWebRequest www = new UnityWebRequest(URL);
    }

    //---------------
    // Zera ÀÜ°í È®ÀÎ
    public void OnClick_ZeraBalance() //¹öÆ° Å¬¸¯½Ã »ç¿ë
    {
        StartCoroutine(processRequestZeraBalance()); //¹æ ÀÔÀå Àü°ú ÈÄ¸¶´Ù È£Ãâ
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
