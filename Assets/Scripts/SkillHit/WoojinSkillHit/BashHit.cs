using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class BashHit : MonoBehaviourPun
{
    // Added list of attacked enemies.
    List<GameObject> attackList = new List<GameObject>();
    // Attacker declaration
    int attacker;
    // Receives the attack view ID as a sand message.
    void AttackerName(int Name)
    {
        attacker = Name;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && attackList.Contains(other.gameObject) == false)//리스트에 안들어있는 적만 맞음.
        {
            // shield effect
            GameObject a = PhotonNetwork.Instantiate("WarofWall", transform.position, Quaternion.identity);
            other.gameObject.GetPhotonView().RPC("RPC_hit", RpcTarget.All, 10f, attacker, state.Stun, 3f);
            // Adds the attacked enemy to the list to avoid getting hit again after escaping the collider.
            attackList.Add(other.gameObject);


            GameMgr.Instance.DestroyTarget(a, 6.2f);
            GameMgr.Instance.DestroyTarget(gameObject, 6.2f);
        }

    }
}
