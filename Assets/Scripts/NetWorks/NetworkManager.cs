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
    [SerializeField] GameObject Postman;

    public AudioSource audioSource;
    public InputField nickNameInput;
    public GameObject logInPanel;
    public GameObject lobbyPanel;
    public GameObject[] readyButton;
    public Image[] lobbyTorchlightOn;
    public Image[] lobbyTorchlightOff;
    public RawImage brokenWindow;

    private GameObject postman;

    //세션ID 닉네임 연동 
    Dictionary<string, string> Nick_Session_key = new Dictionary<string, string>();
    string mySessionID;
    string myBetsId;


    ReadyState myReadyState = ReadyState.None;

    int readyCount = 0;
    int myButtonNum = 0;

    //준비완료 상태를 받는 변수 하나 필요
    public enum ReadyState
    {
        None,
        Ready,
        UnReady,
    }









}
