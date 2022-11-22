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
    [SerializeField] private float screenRatioX;
    // position correction
    private Vector2 tempVec;
    private float ratioX = 0.96794920964835228280278942788686f;
    private float ratioY = 1.0809775369401246252376416055294f;
    private Image renderImage;
    
    private void Awake()
    {
        screenRatioX = Screen.width / 1920f;
        renderImage = GetComponent<Image>();
        if (photonView.IsMine != true) renderImage.sprite = enemyImage;
    }
    private void Start()
    {
        renderImage.transform.localScale = renderImage.transform.localScale * screenRatioX;
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
        Debug.Log("Scale :" + renderImage.transform.localScale);
        if (GameMgr.Instance.GameState == false) return;
        if (photonView.IsMine == false) return;
        
        tempVec.x = 503.5f - target.transform.position.x;
        tempVec.y = 472.67f - target.transform.position.z;
        if (target == null)
            return;
        else
        {
            transform.localPosition = new Vector2(-121 + (tempVec.x / ratioX), -121 + (tempVec.y / ratioY));
        }
    }
}
