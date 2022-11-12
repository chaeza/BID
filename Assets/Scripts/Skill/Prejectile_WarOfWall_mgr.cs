using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Prejectile_WarOfWall_mgr : MonoBehaviourPun
{
    private void Start()
    {
        GameMgr.Instance.DestroyTarget(gameObject, 10f);
    }
}
