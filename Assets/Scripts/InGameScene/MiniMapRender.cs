using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class MiniMapRender : MonoBehaviourPun
{
    // player position
    [SerializeField] private GameObject target;
    [SerializeField] private Sprite enemyImage;
    // position correction
    private Vector2 tempVec;
    private float ratioX = 0.96588348396132983594601426304165f;
    private float ratioY = 1.0797068659017167862159843860718f;
    private Image renderImage;
    
    private void Awake()
    {
        renderImage = GetComponent<Image>();
        if (photonView.IsMine != true) renderImage.sprite = enemyImage;
    }
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
        if (GameMgr.Instance.GameState == false) return;
        if (photonView.IsMine == false) return;
        
        tempVec.x = 502.4f - target.transform.position.x;
        tempVec.y = 472.1f - target.transform.position.z;
        if (target == null)
            return;
        else
        {
            transform.localPosition = new Vector2(-121 + (tempVec.x / ratioX), -121 + (tempVec.y / ratioY));
        }
    }
}
