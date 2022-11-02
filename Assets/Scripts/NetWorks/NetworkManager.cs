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
    private Dictionary<string, string> Nick_Session_key = new Dictionary<string, string>();
    private string mySessionID;
    private string myBetsId;
    private int readyCount = 0;
    private int myButtonNum = 0;

    ReadyState myReadyState = ReadyState.None;
    //This is 
    public enum ReadyState
    {
        None,
        Ready,
        UnReady,
    }











}
