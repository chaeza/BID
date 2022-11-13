using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Immediate_BloodField : Skill
{
    public int skillNum;

    //PlayerInfo playerInfo;
    //private void Awake()
    //{
    //    playerInfo = GetComponent<PlayerInfo>();
    //}
    private void Awake()
    {
        skillInfo.type = SkillType.Skill;
        skillInfo.angle = 0;//use Cone
        skillInfo.radius = 11;//use Immediate,NonTarget
        skillInfo.range = 0;//use Projectile,NonTarget,Cone
        skillInfo.length = 0;//use Projectile,
        skillInfo.cooltime = 20;
        skillInfo.skillNum = skillNum;
        skillInfo.skillType = SkillType.Immediate;

        skillInfo.hitBoxInfo.attackType = AttackType.Shot;
        skillInfo.hitBoxInfo.interval = 0;

        skillInfo.hitBoxInfo.damageInfo.attackState = state.Slow;
        skillInfo.hitBoxInfo.damageInfo.attackDamage = 10;
        skillInfo.hitBoxInfo.damageInfo.attackerViewID = gameObject.GetPhotonView().ViewID;
        skillInfo.hitBoxInfo.damageInfo.slowDownRate = 50;
        skillInfo.hitBoxInfo.damageInfo.timer = 2;

    }
    private void Update()
    {
        if (GameMgr.Instance.playerInput.inputKey == KeyCode.D) SkillUse();
        else if (GameMgr.Instance.playerInput.inputKey == KeyCode.Mouse0) SkillClick(Input.mousePosition);
    }
    protected override void SkillFire()
    {
        //
        GameObject eff = PhotonNetwork.Instantiate("BloodField", transform.position, Quaternion.identity);
        eff.AddComponent<HitBox>().hitBoxInfo = skillInfo.hitBoxInfo;

        eff.transform.position = gameObject.transform.position + new Vector3(0f, 2f, 0f);
        eff.transform.Rotate(-90f, 0f, 0f);

        //
        if (skillInfo.cooltime != 0) GameMgr.Instance.uIMgr.SkillCooltime(skillInfo.cooltime, skillInfo.skillNum,0);
    }
}