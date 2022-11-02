using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class GameMgr : Singleton<GameMgr>
{
    public PlayerInput playerInput { get; private set; } = null;
    public FollowCam followCam { get; private set; } = null;

    private void Awake()
    {
        playerInput = gameObject.AddComponent<PlayerInput>();
        followCam = FindObjectOfType<FollowCam>();

    }
}
