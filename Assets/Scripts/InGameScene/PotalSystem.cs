using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PotalSystem : MonoBehaviourPun
{
    [SerializeField]
    private Potal centerPotal;
    [SerializeField]
    private Potal[] nonCenterPotal;

    private void Start()
    {
        centerPotal.exit = nonCenterPotal;
        centerPotal.potalData.num = 0;
        centerPotal.potalData.isCenter = true;
        centerPotal.potalData.isRandomPotal = false;
        centerPotal.potalData.potalTotalNum = nonCenterPotal.Length;
        ConnectPotal();
    }
    
    private void ConnectPotal()
    {
        for (int i = 0; i < nonCenterPotal.Length; i++)
        {
            nonCenterPotal[i].exit = new Potal[1];
            nonCenterPotal[i].exit[0] = centerPotal;
            nonCenterPotal[i].potalData.num = i + 1;
            nonCenterPotal[i].potalData.isCenter = false;
            nonCenterPotal[i].potalData.isRandomPotal = false;
            nonCenterPotal[i].potalData.potalTotalNum = nonCenterPotal.Length;
        }
    }
}
