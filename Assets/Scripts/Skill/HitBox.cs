using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public enum AttackType
{
    Shot,
    Continuous,
}
public class HitBox : MonoBehaviourPun
{
    public DamageInfo damageInfo;
    private List<GameObject> attackList = new List<GameObject>();
    private float timer;

    private void OnTriggerEnter(Collider other)
    {
        if (damageInfo.attackType != AttackType.Shot) return;
        if (other.gameObject.tag == "Player" && attackList.Contains(other.gameObject) == false)
        {
            other.gameObject.GetPhotonView().RPC("RPC_GetDamage", RpcTarget.All,damageInfo);
            attackList.Add(other.gameObject);
        }
    }



    private void OnTriggerStay(Collider other)
    {
        if (damageInfo.attackType != AttackType.Continuous) return;
        timer += Time.deltaTime;
        if (timer >= damageInfo.interval && other.gameObject.tag == "Player")
        {
            other.gameObject.GetPhotonView().RPC("RPC_GetDamage", RpcTarget.All, damageInfo);
            timer = 0;
        }
    }
}
