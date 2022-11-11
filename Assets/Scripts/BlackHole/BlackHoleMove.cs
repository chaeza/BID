using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BlackHoleMove : MonoBehaviourPun
{
    [SerializeField] private List<GameObject> blackHoleList = null;
    private float blackHoleTime = 10f;
    private float time = 0;
    private int MasterRan;
    private int ran = 0;

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
         //   photonView.RPC("BlackHolePos", RpcTarget.MasterClient, MasterRan);
        }
    }

    [PunRPC]
    public void BlackHolePos(int MasterRan)
    {
        photonView.RPC("RandomBlackHole", RpcTarget.All, MasterRan);
    }

    [PunRPC]
    public void RandomBlackHole()
    {
        if (blackHoleList.Count > 0)
        {
            MasterRan = Random.Range(0, blackHoleList.Count);
            ran = MasterRan;
        }
    }


    private void Update()
    {
        time += Time.deltaTime;

        if (time >= blackHoleTime)
        {
            RandomBlackHole();
            blackHoleList[ran].SetActive(true);
            blackHoleList.Remove(blackHoleList[ran]);
            time = 0;
        }
    }
}
