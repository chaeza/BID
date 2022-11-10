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
    private void Start()
    {
        skillInfo.angle = 20;//use Cone
        skillInfo.radius = 6;//use Immediate,NonTarget
        skillInfo.range = 30;//use Projectile,NonTarget,Cone
        skillInfo.length = 6;//use Projectile,
        skillInfo.cooltime = 5;
        skillInfo.skillType = SkillType.NonTarget;

        skillInfo.skillDamageInfo.attackType = AttackType.Shot;
        skillInfo.skillDamageInfo.interval = 0;
        skillInfo.skillDamageInfo.attackDamage = 0;
        skillInfo.skillDamageInfo.attackerViewID = gameObject.GetPhotonView().ViewID;
        skillInfo.skillDamageInfo.attackState = state.None;
        skillInfo.skillDamageInfo.slowDownRate = 0;
        skillInfo.skillDamageInfo.timer = 0;
    }
    private void Update()
    {
        if (GameMgr.Instance.playerInput.inputKey == KeyCode.D) SkillUse();
        else if (GameMgr.Instance.playerInput.inputKey == KeyCode.Mouse0) SkillClick(Input.mousePosition);
    }
    protected override void SkillFire()
    {
        //GameObject eff = PhotonNetwork.Instantiate("HitBox", transform.position, Quaternion.identity);
        //eff.AddComponent<HitBox>().damageInfo = skillInfo.skillDamageInfo;
       // GameMgr.Instance.uIMgr.SkillCooltime(skillInfo.cooltime);
    }
}
