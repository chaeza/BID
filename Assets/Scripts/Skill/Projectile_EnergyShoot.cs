using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Projectile_EnergyShoot : Skill
{
    private void Awake()
    {
        skillInfo.type = SkillType.Skill;
        skillInfo.angle = 0;//use Cone
        skillInfo.radius = 0;//use Immediate,NonTarget
        skillInfo.range = 30;//use Projectile,NonTarget,Cone
        skillInfo.length = 8;//use Projectile,
        skillInfo.cooltime = 3;
        skillInfo.skillNum = 3;
        skillInfo.skillType = SkillType.Projectile;

        skillInfo.hitBoxInfo.attackType = AttackType.Shot;
        skillInfo.hitBoxInfo.interval = 1;

        skillInfo.hitBoxInfo.damageInfo.attackState = state.None;
        skillInfo.hitBoxInfo.damageInfo.attackDamage = 7;
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
        GameObject eff = PhotonNetwork.Instantiate("EnergyBall", transform.position, Quaternion.identity);
        eff.AddComponent<HitBox>().hitBoxInfo = skillInfo.hitBoxInfo;

        eff.transform.LookAt(desiredDir);
        StartCoroutine(Projectile_EnergyShootTimer(eff));


        if (skillInfo.cooltime != 0) GameMgr.Instance.uIMgr.SkillCooltime(skillInfo.cooltime, skillInfo.skillNum);
    }

    IEnumerator Projectile_EnergyShootTimer(GameObject eff)
    {
        int i = 0;
        while(true)
        {
            eff.transform.Translate(Vector3.forward);
            yield return new WaitForSeconds(0.1f);
            i++;
            if (i == 30) break;
        }
        GameMgr.Instance.DestroyTarget(eff,0.1f);
        yield return null;
    }

}
