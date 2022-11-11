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
public struct HitBoxInfo
{
    public AttackType attackType;
    public float interval;
    public DamageInfo damageInfo;
}
public class HitBox : MonoBehaviourPun
{
    public HitBoxInfo hitBoxInfo;
    private List<GameObject> attackList = new List<GameObject>();
    private float timer;
    private int count;

    private void OnTriggerEnter(Collider other)
    {
        if (hitBoxInfo.attackType != AttackType.Shot) return;
        if (other.gameObject.tag == "Player" && attackList.Contains(other.gameObject) == false)
        {
            other.gameObject.GetPhotonView().RPC("RPC_GetDamage", RpcTarget.All,hitBoxInfo.damageInfo);
            attackList.Add(other.gameObject);
            if(hitBoxInfo.interval!=0)
            {
                count++;
                if (count >= hitBoxInfo.interval) GameMgr.Instance.DestroyTarget(gameObject, 0f);
            }
        }
    }



    private void OnTriggerStay(Collider other)
    {
        if (hitBoxInfo.attackType != AttackType.Continuous) return;
        timer += Time.deltaTime;
        if (timer >= hitBoxInfo.interval && other.gameObject.tag == "Player")
        {
            other.gameObject.GetPhotonView().RPC("RPC_GetDamage", RpcTarget.All,hitBoxInfo.damageInfo);
            timer = 0;
        }
    }
}
