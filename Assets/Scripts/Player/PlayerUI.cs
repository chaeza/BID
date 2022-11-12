using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerUI : MonoBehaviourPun
{
    //[SerializeField] TextMeshProUGUI nickname = null;
    [SerializeField] Slider Hpbar = null;
    [SerializeField] Image[] bee = null;
    private Transform cam = null;

    void Start()
    {
        //ī�޶�
        cam = FindObjectOfType<FollowCam>().gameObject.transform;
        if (photonView.IsMine)
        {
            gameObject.GetPhotonView().RPC("SetName",RpcTarget.All,PhotonNetwork.NickName);
        }
    }
    [PunRPC]
    public void SetName(string name)
    {
      //  nickname.text = name;
    }
    // Update is called once per frame
    void Update()
    {
        //ī�޶� ����
        gameObject.transform.LookAt(transform.position + cam.rotation * Vector3.forward, cam.rotation * Vector3.up);
    }
}
