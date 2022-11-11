using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MiniMapRender : MonoBehaviourPun
{
    // player position
    [SerializeField] private GameObject target;
    // position correction
    private Vector2 tempVec;
    private float ratioX = 1.3206611570247933884297520661157f;
    private float ratioY = 1.2966942148760330578512396694215f;
    
    public void SetTarget(GameObject player)
    {
        target = player;
    }
    
    [PunRPC]
    public void SetParentMiniMap()
    {
        transform.parent = GameObject.Find("MiniMapView").transform;
    }
    void Update()
    {
        if (photonView.IsMine != true) return;
        tempVec.x = 546.6f - target.transform.position.x;
        tempVec.y = 507.6f - target.transform.position.z;
        if (target == null)
            return;
        else
        {
            transform.localPosition = new Vector2(-121 + (tempVec.x / ratioX), -121 + (tempVec.y / ratioY));
        }
    }
}
