using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TestSkill : Skill
{
    [SerializeField] private GameObject Eff;

    //PlayerInfo playerInfo;
    //private void Awake()
    //{
    //    playerInfo = GetComponent<PlayerInfo>();
    //}
    private void Awake()
    {
        skillInfo.type = SkillType.Skill;
        skillInfo.angle = 20;//use Cone
        skillInfo.radius = 6;//use Immediate,NonTarget
        skillInfo.range = 30;//use Projectile,NonTarget,Cone
        skillInfo.length = 6;//use Projectile,
        skillInfo.cooltime = 5;
        skillInfo.skillNum = 1;
        skillInfo.skillType = SkillType.Immediate;

        skillInfo.hitBoxInfo.attackType = AttackType.Shot;
        skillInfo.hitBoxInfo.interval = 1;

        skillInfo.hitBoxInfo.damageInfo.attackState = state.None;
        skillInfo.hitBoxInfo.damageInfo.attackDamage = 10;
        skillInfo.hitBoxInfo.damageInfo.attackerViewID = gameObject.GetPhotonView().ViewID;
        skillInfo.hitBoxInfo.damageInfo.slowDownRate = 0;
        skillInfo.hitBoxInfo.damageInfo.timer = 0;

    }
    private void Update()
    {
        if (GameMgr.Instance.playerInput.inputKey == KeyCode.D) SkillUse();
        else if (GameMgr.Instance.playerInput.inputKey == KeyCode.Mouse0) SkillClick(Input.mousePosition);
    }
    protected override void SkillFire()
    {
        //
        GameObject eff = PhotonNetwork.Instantiate("Bash", transform.position, Quaternion.identity);
        eff.AddComponent<HitBox>().hitBoxInfo = skillInfo.hitBoxInfo;


        //
        if (skillInfo.cooltime != 0) GameMgr.Instance.uIMgr.SkillCooltime(skillInfo.cooltime, skillInfo.skillNum);
    }
}
