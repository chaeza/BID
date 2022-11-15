using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NonTarget_FallTheRain : Skill
{
    public void SetSkillNum(int Num)
    {
        skillInfo.skillNum = Num;
    }
    // Start is called before the first frame update
    private void Awake()
    {
        skillInfo.type = SkillType.Skill;
        skillInfo.radius = 6;//use Immediate,NonTarget
        skillInfo.range = 30;//use Projectile,NonTarget,Cone
        skillInfo.cooltime = 15;
        skillInfo.skillType = SkillType.NonTarget;

        skillInfo.hitBoxInfo.attackType = AttackType.Continuous;
        skillInfo.hitBoxInfo.interval = 0.5f;

        skillInfo.hitBoxInfo.damageInfo.attackState = state.Slow;
        skillInfo.hitBoxInfo.damageInfo.attackDamage = 5;
        skillInfo.hitBoxInfo.damageInfo.attackerViewID = gameObject.GetPhotonView().ViewID;
        skillInfo.hitBoxInfo.damageInfo.slowDownRate = 30;
        skillInfo.hitBoxInfo.damageInfo.timer = 1f;
    }

    private void Update()
    {
        if (GameMgr.Instance.playerInput.inputKey == KeyCode.D) SkillUse();
        else if (GameMgr.Instance.playerInput.inputKey == KeyCode.Mouse0) SkillClick(Input.mousePosition);
    }
    protected override void SkillFire()
    {
        playerInfo.StayPlayer(0.5f);
        animator.SetTrigger("isSkill2");
        transform.LookAt(desiredDir);
        StartCoroutine(SkillFire_Delay(0.5f));
        if (skillInfo.cooltime != 0) GameMgr.Instance.uIMgr.SkillCooltime(skillInfo.cooltime, skillInfo.skillNum,0);
    }
    IEnumerator SkillFire_Delay(float time)
    {

        yield return new WaitForSeconds(time);
        GameObject eff = PhotonNetwork.Instantiate("FallTheRainPrefab", desiredDir, Quaternion.identity);
        eff.transform.Rotate(-90, 0f, 0);
        eff.AddComponent<HitBox>().skillInfo = skillInfo;
        GameMgr.Instance.DestroyTarget(eff, 5f);

    }
}
