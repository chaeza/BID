using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Projectile_MysticArrow : Skill
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
        skillInfo.range = 30;//use Projectile,NonTarget,Cone
        skillInfo.length = 7;//use Projectile,
        skillInfo.cooltime = 2;
        skillInfo.skillDirY = 3;
        skillInfo.skillType = SkillType.Projectile;
        skillInfo.hitReturn = true;

        skillInfo.hitBoxInfo.attackType = AttackType.Shot;
        skillInfo.hitBoxInfo.interval = 1;

        skillInfo.hitBoxInfo.damageInfo.attackState = state.Slow;
        skillInfo.hitBoxInfo.damageInfo.attackDamage = 10;
        skillInfo.hitBoxInfo.damageInfo.attackerViewID = gameObject.GetPhotonView().ViewID;
        skillInfo.hitBoxInfo.damageInfo.slowDownRate = 30;
        skillInfo.hitBoxInfo.damageInfo.timer = 1;
    }
    private void FixedUpdate()
    {
        if (GameMgr.Instance.GameState == false) return;
        if (GameMgr.Instance.playerInput.inputKey == KeyCode.D) SkillUse();
        else if (GameMgr.Instance.playerInput.inputKey == KeyCode.Mouse0) SkillClick(Input.mousePosition);
    }
    protected override void SkillFire()
    {
        //
        playerInfo.StayPlayer(0.2f);
        animator.SetTrigger("isAttack1");
        StartCoroutine(SkillFire_Delay(0.2f));
        transform.LookAt(desiredDir);
        //
        if (skillInfo.cooltime != 0) GameMgr.Instance.uIMgr.SkillCooltime(skillInfo.cooltime, skillInfo.skillNum,0);
    }
    protected override void HitFire(GameObject attacker, GameObject hit)
    {
        GameObject eff = PhotonNetwork.Instantiate("MysticArrow_Boom", hit.transform.position+new Vector3(0,2,0), Quaternion.identity);
        GameMgr.Instance.DestroyTarget(eff, 1f);
    }
    IEnumerator SkillFire_Delay(float time)
    {
        yield return new WaitForSeconds(time);
        GameObject eff = PhotonNetwork.Instantiate("MysticArrow", transform.position, Quaternion.identity);
        eff.AddComponent<HitBox>().skillInfo = skillInfo;
        eff.transform.LookAt(desiredDir);
        StartCoroutine(DestroyObject(eff));
    }
    IEnumerator DestroyObject(GameObject eff)
    {
        int i = 0;
        while (true)
        {
            if (eff == null) break;
            eff.transform.Translate(Vector3.forward);
            i++;
            if (i == 30) break;
            yield return new WaitForSeconds(0.02f);
        }
        GameMgr.Instance.DestroyTarget(eff, 0.1f);
        yield return null;
    }

    
}
