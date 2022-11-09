using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public partial class GameMgr : Singleton<GameMgr>
{
    private void Awake()
    {
        playerInput = gameObject.AddComponent<PlayerInput>();
        randomSkill = gameObject.AddComponent<RandomSkill>();
        followCam = FindObjectOfType<FollowCam>();
        uIMgr = FindObjectOfType<UIMgr>();
    }

    // Skill Destroy
    public void DestroyTarget(GameObject desObject, float time)
    {
        photonView.RPC("PunDestroyObject", RpcTarget.All, desObject.GetPhotonView().ViewID, time);
    }
}
