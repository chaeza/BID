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
public struct HitReturnInfo
{
    public SkillType type;
    public int num;
    public GameObject attacker;
    public GameObject hit;
}
public class HitBox : MonoBehaviourPun
{
    public SkillInfo skillInfo;
    public HitReturnInfo hitReturnInfo;
    private List<GameObject> attackList = new List<GameObject>();
    private float timer;
    private int count;

    public void DestroyHitBox(float time)
    {
        Destroy(GetComponent<HitBox>(),time);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (skillInfo.hitBoxInfo.attackType != AttackType.Shot) return;
        if (other.gameObject.tag == "Player" && attackList.Contains(other.gameObject) == false)
        {
            other.gameObject.GetPhotonView().RPC("RPC_GetDamage", RpcTarget.AllBufferedViaServer,
                skillInfo.hitBoxInfo.damageInfo.attackState,
                skillInfo.hitBoxInfo.damageInfo.attackDamage,
                skillInfo.hitBoxInfo.damageInfo.slowDownRate,
                skillInfo.hitBoxInfo.damageInfo.timer,
                skillInfo.hitBoxInfo.damageInfo.attackerViewID);
            attackList.Add(other.gameObject);
            if (skillInfo.hitReturn == true)
            {

                hitReturnInfo.attacker = GameMgr.Instance.PunFindObject(skillInfo.hitBoxInfo.damageInfo.attackerViewID);
                hitReturnInfo.hit = other.gameObject;
                hitReturnInfo.type = skillInfo.type;
                if (skillInfo.type == SkillType.Skill) hitReturnInfo.num = skillInfo.skillNum;
                else if (skillInfo.type == SkillType.Item) hitReturnInfo.num = skillInfo.itemNum;

                hitReturnInfo.attacker.SendMessage("HitReturn", hitReturnInfo, SendMessageOptions.DontRequireReceiver);
            }
            if (skillInfo.hitBoxInfo.interval != 0)
            {
                count++;
                if (count >= skillInfo.hitBoxInfo.interval) GameMgr.Instance.DestroyTarget(gameObject, 0f);
            }
        }
    }



    private void OnTriggerStay(Collider other)
    {
        if (skillInfo.hitBoxInfo.attackType != AttackType.Continuous) return;
        timer += Time.deltaTime;
        if (timer >= skillInfo.hitBoxInfo.interval && other.gameObject.tag == "Player")
        {
            other.gameObject.GetPhotonView().RPC("RPC_GetDamage", RpcTarget.AllBufferedViaServer,
                skillInfo.hitBoxInfo.damageInfo.attackState,
                skillInfo.hitBoxInfo.damageInfo.attackDamage,
                skillInfo.hitBoxInfo.damageInfo.slowDownRate,
                skillInfo.hitBoxInfo.damageInfo.timer,
                skillInfo.hitBoxInfo.damageInfo.attackerViewID);
            timer = 0;
        }
    }
}
