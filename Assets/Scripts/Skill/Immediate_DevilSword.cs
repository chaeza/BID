using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Immediate_DevilSword : Skill
{
    public void SetSkillNum(int Num)
    {
        skillInfo.skillNum = Num;
    }
    private void Awake()
    {
        skillInfo.type = SkillType.Skill;
        skillInfo.angle = 0;//use Cone
        skillInfo.radius = 13;//use Immediate,NonTarget
        skillInfo.range = 0;//use Projectile,NonTarget,Cone
        skillInfo.length = 0;//use Projectile,
        skillInfo.cooltime = 20;
        skillInfo.skillType = SkillType.Immediate;

        skillInfo.hitBoxInfo.attackType = AttackType.Shot;
        skillInfo.hitBoxInfo.interval = 0;

        skillInfo.hitBoxInfo.damageInfo.attackState = state.None;
        skillInfo.hitBoxInfo.damageInfo.attackDamage = 30;
        skillInfo.hitBoxInfo.damageInfo.attackerViewID = gameObject.GetPhotonView().ViewID;
        skillInfo.hitBoxInfo.damageInfo.slowDownRate = 0;
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
        playerInfo.StayPlayer(0.8f);
        animator.SetTrigger("Slam");
        StartCoroutine(SkillFire_Delay(0.8f));
        //
        if (skillInfo.cooltime != 0) GameMgr.Instance.uIMgr.SkillCooltime(skillInfo.cooltime, skillInfo.skillNum, 0);
    }
    protected override void HitFire(GameObject attacker, GameObject hit)
    {
        GameObject eff = PhotonNetwork.Instantiate("MysticArrow_Boom", hit.transform.position + new Vector3(0, 2, 0), Quaternion.identity);
        GameMgr.Instance.DestroyTarget(eff, 1f);
    }
    IEnumerator SkillFire_Delay(float time)
    {

        yield return new WaitForSeconds(time);
        GameObject eff = PhotonNetwork.Instantiate("DevilSword", transform.position, Quaternion.identity);
        eff.AddComponent<HitBox>().skillInfo = skillInfo;
        eff.transform.position = gameObject.transform.position + new Vector3(0f, 2f, 0f);
        eff.transform.Rotate(-180f, 0f, 0f);
        GameMgr.Instance.DestroyTarget(eff, 5f);
        eff.GetComponent<HitBox>().DestroyHitBox(0.5f);
    }
}
