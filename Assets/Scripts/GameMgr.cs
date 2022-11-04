using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public partial class GameMgr : Singleton<GameMgr>
{
    [Tooltip("Game MGR uIMgr")]
    [field: SerializeField]
    public UIMgr uIMgr { get; private set; } = null;

    private void Awake()
    {
        playerInput = gameObject.AddComponent<PlayerInput>();
        followCam = FindObjectOfType<FollowCam>();

    }

    // Skill Destroy
    public void DestroyTarget(GameObject desObject, float time)
    {
        photonView.RPC("PunDestroyObject", RpcTarget.All, desObject.GetPhotonView().ViewID, time);
    }
}
