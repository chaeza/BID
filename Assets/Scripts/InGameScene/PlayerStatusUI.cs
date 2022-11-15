using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerStatusUI : MonoBehaviourPun
{
    //[SerializeField] TextMeshProUGUI nickname = null;
    [SerializeField] TextMeshProUGUI nickName = null;
    [SerializeField] Slider hpBar = null;
    [SerializeField] Image[] bee = null;

    private PlayerInfo playerInfo = null;
    private Transform cam = null;

    void Start()
    {
        cam = FindObjectOfType<FollowCam>().gameObject.transform;
        playerInfo = gameObject.GetComponentInParent<PlayerInfo>();
        hpBar.value = 100;
        playerInfo.HPTransfer = SetHPvalue;

        SetName(photonView.Controller.NickName);

    }

    public void SetName(string name)
    {
        nickName.text = name;
    }
    // Update is called once per frame
    void Update()
    {

        //카메라 보기
        gameObject.transform.LookAt(transform.position + cam.rotation * Vector3.forward, cam.rotation * Vector3.up);
    }
    private void SetHPvalue(float playerHP)
    {
        hpBar.value = playerHP;
    }

}
