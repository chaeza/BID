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
    private float ratioX = 0.95579179156193233307773371987649f;
    private float ratioY = 1.0684259301023901980965346433155f;
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
