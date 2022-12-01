using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Immediate_BloodField : Skill
{
    public void SetSkillNum(int Num)
    {
        skillInfo.skillNum = Num;
    }
    private void Awake()
    {
        skillInfo.type = SkillType.Skill;
        skillInfo.angle = 0;//use Cone
        skillInfo.radius = 11;//use Immediate,NonTarget
        skillInfo.range = 0;//use Projectile,NonTarget,Cone
        skillInfo.length = 0;//use Projectile,
        skillInfo.cooltime = 20;
        skillInfo.skillType = SkillType.Immediate;

        skillInfo.hitBoxInfo.attackType = AttackType.Shot;
        skillInfo.hitBoxInfo.interval = 0;

        skillInfo.hitBoxInfo.damageInfo.attackState = state.Slow;
        skillInfo.hitBoxInfo.damageInfo.attackDamage = 25;
        skillInfo.hitBoxInfo.damageInfo.attackerViewID = gameObject.GetPhotonView().ViewID;
        skillInfo.hitBoxInfo.damageInfo.slowDownRate = 50;
        skillInfo.hitBoxInfo.damageInfo.timer = 3;

    }
    private void Update()
    {
        if (GameMgr.Instance.GameState == false) return;
        if (GameMgr.Instance.playerInput.inputKey == KeyCode.D) SkillUse();
        else if (GameMgr.Instance.playerInput.inputKey == KeyCode.Mouse0) SkillClick(Input.mousePosition);
    }
    protected override void SkillFire()
    {
        //
        playerInfo.StayPlayer(0.5f);
        animator.SetTrigger("Slam");
        StartCoroutine(SkillFire_Delay(0.5f));
        //
        if (skillInfo.cooltime != 0) GameMgr.Instance.uIMgr.SkillCooltime(skillInfo.cooltime, skillInfo.skillNum, 0);
    }
    IEnumerator SkillFire_Delay(float time)
    {

        yield return new WaitForSeconds(time);
        GameObject eff = PhotonNetwork.Instantiate("BloodField", transform.position, Quaternion.identity);
        eff.AddComponent<HitBox>().skillInfo = skillInfo;
        eff.transform.position = gameObject.transform.position + new Vector3(0f, 2f, 0f);
        eff.transform.Rotate(-90f, 0f, 0f);
        GameMgr.Instance.DestroyTarget(eff, 5f);
        yield return new WaitForSeconds(0.5f);
        eff.GetComponent<HitBox>().skillInfo = skillInfo;
        eff.GetComponent<HitBox>().DestroyHitBox(0.5f);
    }
}