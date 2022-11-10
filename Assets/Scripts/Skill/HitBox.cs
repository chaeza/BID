using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Threading;

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
    private int count;

    private void OnTriggerEnter(Collider other)
    {
        if (damageInfo.attackType != AttackType.Shot) return;
        if (other.gameObject.tag == "Player" && attackList.Contains(other.gameObject) == false)
        {
            other.gameObject.GetPhotonView().RPC("RPC_GetDamage", RpcTarget.All,damageInfo);
            attackList.Add(other.gameObject);
            if(damageInfo.interval!=0)
            {
                count++;
                if (count >= damageInfo.interval) GameMgr.Instance.DestroyTarget(gameObject, 0f);
            }
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
