using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Immediate_FallTheRain : Skill
{
    public int skillNum;
    // Start is called before the first frame update
    private void Awake()
    {
        skillInfo.type = SkillType.Skill;
        skillInfo.radius = 6;//use Immediate,NonTarget
        skillInfo.range = 30;//use Projectile,NonTarget,Cone
        skillInfo.cooltime = 15;
        skillInfo.skillNum = skillNum;
        skillInfo.skillType = SkillType.NonTarget;

        skillInfo.hitBoxInfo.attackType = AttackType.Continuous;
        skillInfo.hitBoxInfo.interval = 0.5f;

        skillInfo.hitBoxInfo.damageInfo.attackState = state.Slow;
        skillInfo.hitBoxInfo.damageInfo.attackDamage = 15;
        skillInfo.hitBoxInfo.damageInfo.attackerViewID = gameObject.GetPhotonView().ViewID;
        skillInfo.hitBoxInfo.damageInfo.slowDownRate = 10;
        skillInfo.hitBoxInfo.damageInfo.timer = 1f;
    }

    private void Update()
    {
        if (GameMgr.Instance.playerInput.inputKey == KeyCode.D) SkillUse();
        else if (GameMgr.Instance.playerInput.inputKey == KeyCode.Mouse0) SkillClick(Input.mousePosition);
    }
    protected override void SkillFire()
    {
        GameObject eff = PhotonNetwork.Instantiate("FallTheRainPrefab", desiredDir, Quaternion.identity);
        eff.transform.Rotate(0, 90f, 0);
        eff.AddComponent<HitBox>().hitBoxInfo = skillInfo.hitBoxInfo;

        if (skillInfo.cooltime != 0) GameMgr.Instance.uIMgr.SkillCooltime(skillInfo.cooltime, skillInfo.skillNum,0);
    }
}
