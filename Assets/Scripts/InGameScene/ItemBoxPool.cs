using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ItemBoxPool : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "MainPlayer")
        {
            Debug.Log("¥Í¿Ω");
            GameMgr.Instance.randomItem.GetRandomitem(other.gameObject);
            GameMgr.Instance.itemSpawner.photonView.RPC("ReleasePool", RpcTarget.All, gameObject.GetPhotonView().ViewID);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
       if (other.gameObject.tag == "ItemBlackHole")
        {
            Debug.Log("∂•ø°∂≥±¿");
            GameMgr.Instance.itemSpawner.photonView.RPC("ReleasePool", RpcTarget.All, gameObject.GetPhotonView().ViewID);
        }
    }
}
