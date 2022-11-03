using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class GameMgr : Singleton<GameMgr>
{
    [Tooltip("Game MGR uIMgr")]
    [field: SerializeField]
    public UIMgr uIMgr { get; set; } = null;
    public PlayerInput playerInput { get;  set; } = null;
    public FollowCam followCam { get;  set; } = null;


    private void Awake()
    {
        playerInput = gameObject.AddComponent<PlayerInput>();
        followCam = FindObjectOfType<FollowCam>();
    }

    // Remove skill from photon
    public void DestroyTarget(GameObject desObject, float time)
    {
        photonView.RPC("PunDestroyObject", RpcTarget.All, desObject.GetPhotonView().ViewID, time);
    }
}
