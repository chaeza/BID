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
        followCam = FindObjectOfType<FollowCam>();

    }
}
