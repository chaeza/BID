using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Projectile_Bash_mgr : MonoBehaviourPun
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // shield effect
            GameObject a = PhotonNetwork.Instantiate("WarofWall", transform.position, Quaternion.identity);
        }
    }
}
