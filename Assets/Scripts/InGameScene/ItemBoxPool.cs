using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ItemBoxPool : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "MainPlayer")
        {
            Debug.Log("¥Í¿Ω");
            GameMgr.Instance.randomItem.GetRandomitem(other.gameObject);
            Debug.Log(other.gameObject);
            GameMgr.Instance.itemSpawner.photonView.RPC("ReleasePool", RpcTarget.All, gameObject.GetPhotonView().ViewID);
        }
        else
        {
            Debug.Log("æ»∏‘¿Ω");
        }
    }
}
