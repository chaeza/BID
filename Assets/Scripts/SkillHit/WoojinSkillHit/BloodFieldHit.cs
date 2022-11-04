using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class BloodFieldHit : MonoBehaviourPun
{
    //Add the list to put the attacked enemies in.
    List<GameObject> attackList = new List<GameObject>();

    //declare the attacker
    private float timer;
    private int Attacker;
  
    // Receive the attack view ID as a send message.
    void AttackerName(int Name)
    {
        Attacker = Name;
    }

    private void OnTriggerStay(Collider other)
    {
        timer += Time.deltaTime;
        if (timer > 0.5f && other.tag == "Player")
        {
            // Damage the hit enemy and send who hit it.
            other.gameObject.GetPhotonView().RPC("RPC_hit", RpcTarget.All, 50f, Attacker, state.Slow, 1f);
            other.gameObject.GetPhotonView().RPC("RPC_hit", RpcTarget.All, 2.5f, Attacker, state.None, 0f);

            timer = 0f;
        }
    }
}
