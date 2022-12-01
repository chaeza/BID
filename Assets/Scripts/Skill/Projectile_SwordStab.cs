using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Projectile_SwordStab : Skill
{
    public void SetSkillNum(int Num)
    {
        skillInfo.skillNum = Num;
    }

    private void Awake()
    {
        skillInfo.type = SkillType.Skill;
        skillInfo.angle = 0;//use Cone
        skillInfo.radius = 0;//use Immediate,NonTarget
        skillInfo.range = 15;//use Projectile,NonTarget,Cone
        skillInfo.length = 8;//use Projectile,
        skillInfo.cooltime = 5;
        skillInfo.skillDirY = 3;
        skillInfo.skillType = SkillType.Projectile;

        skillInfo.hitBoxInfo.attackType = AttackType.Shot;
        skillInfo.hitBoxInfo.interval = 0;

        skillInfo.hitBoxInfo.damageInfo.attackState = state.Stun;
        skillInfo.hitBoxInfo.damageInfo.attackDamage = 10;
        skillInfo.hitBoxInfo.damageInfo.attackerViewID = gameObject.GetPhotonView().ViewID;
        skillInfo.hitBoxInfo.damageInfo.slowDownRate = 0;
        skillInfo.hitBoxInfo.damageInfo.timer = 0.5f;

    }
    private void Update()
    {
        if (GameMgr.Instance.GameState == false) return;
        if (GameMgr.Instance.playerInput.inputKey == KeyCode.D) SkillUse();
        else if (GameMgr.Instance.playerInput.inputKey == KeyCode.Mouse0) SkillClick(Input.mousePosition);
    }
    protected override void SkillFire()
    {
        playerInfo.StayPlayer(0.4f);
        animator.SetTrigger("Prick");
        transform.LookAt(desiredDir);
        StartCoroutine(SkillFire_Delay(0.4f));


        if (skillInfo.cooltime != 0) GameMgr.Instance.uIMgr.SkillCooltime(skillInfo.cooltime, skillInfo.skillNum, 0);
    }
    IEnumerator SkillFire_Delay(float time)
    {
        yield return new WaitForSeconds(time);
        GameObject eff = PhotonNetwork.Instantiate("SwordStab", transform.position + Vector3.up * 3, Quaternion.identity);
        eff.AddComponent<HitBox>().skillInfo = skillInfo;

        eff.transform.LookAt(desiredDir);
        StartCoroutine(Projectile_EnergyShootTimer(eff));
    }
    IEnumerator Projectile_EnergyShootTimer(GameObject eff)
    {
        int i = 0;
        while (true)
        {
            if (eff == null) break;
            eff.transform.Translate(Vector3.forward);
            i++;
            if (i == 15) break;
            yield return new WaitForSeconds(0.02f);
        }
        GameMgr.Instance.DestroyTarget(eff, 0.5f);
        yield return null;
    }

}
