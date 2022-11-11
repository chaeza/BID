using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PotalSystem : MonoBehaviourPun
{
    [SerializeField]
    private Portal centerPortal;
    [SerializeField]
    private Portal[] nonCenterPortal;

    private void Start()
    {
        
        centerPortal.exit = nonCenterPortal;
        centerPortal.portalData.num = 0;
        centerPortal.portalData.isCenter = true;
        centerPortal.portalData.isDestoryed = false;
        centerPortal.portalData.isRandomPotal = false;
        centerPortal.portalData.potalTotalNum = nonCenterPortal.Length;
        ConnectPotal();
    }
    
    private void ConnectPotal()
    {
        for (int i = 0; i < nonCenterPortal.Length; i++)
        {
            nonCenterPortal[i].exit = new Portal[1];
            nonCenterPortal[i].exit[0] = centerPortal;
            nonCenterPortal[i].portalData.num = i + 1;
            nonCenterPortal[i].portalData.isCenter = false;
            nonCenterPortal[i].portalData.isDestoryed = false;
            nonCenterPortal[i].portalData.isRandomPotal = false;
            nonCenterPortal[i].portalData.potalTotalNum = nonCenterPortal.Length;
        }
    }
}
