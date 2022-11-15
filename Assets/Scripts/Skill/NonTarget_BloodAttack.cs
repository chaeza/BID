using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NonTarget_BloodAttack : Skill
{
    public void SetSkillNum(int Num)
    {
        skillInfo.skillNum = Num;
    }
    // Start is called before the first frame update
    private void Awake()
    {
        skillInfo.type = SkillType.Skill;
        skillInfo.radius = 5;//use Immediate,NonTarget
        skillInfo.range = 20;//use Projectile,NonTarget,Cone
        skillInfo.cooltime = 15;
        skillInfo.skillType = SkillType.NonTarget;
        skillInfo.hitReturn = true;

        skillInfo.hitBoxInfo.attackType = AttackType.Shot;
        skillInfo.hitBoxInfo.interval = 0f;

        skillInfo.hitBoxInfo.damageInfo.attackState = state.None;
        skillInfo.hitBoxInfo.damageInfo.attackDamage = 10;
        skillInfo.hitBoxInfo.damageInfo.attackerViewID = gameObject.GetPhotonView().ViewID;
        skillInfo.hitBoxInfo.damageInfo.slowDownRate = 0;
        skillInfo.hitBoxInfo.damageInfo.timer = 0f;
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
        if (skillInfo.cooltime != 0) GameMgr.Instance.uIMgr.SkillCooltime(skillInfo.cooltime, skillInfo.skillNum, 0);
    }
    protected override void HitFire(GameObject attacker, GameObject hit)
    {
        GameObject eff = PhotonNetwork.Instantiate("BloodAbsorption", hit.transform.position, Quaternion.identity);
        //eff.transform.Rotate(0, -90f, 0);
        eff.transform.LookAt(attacker.transform);
        eff.transform.Rotate(new Vector3(0, 180f, 0));
        StartCoroutine(BloodAbsorptionMotion(attacker, eff));
        attacker.GetPhotonView().RPC("ChangeHP", RpcTarget.All, 10f);

    }
    IEnumerator SkillFire_Delay(float time)
    {

        yield return new WaitForSeconds(time);
        GameObject eff = PhotonNetwork.Instantiate("BloodAttack", desiredDir, Quaternion.identity);
        eff.transform.Rotate(-90, 0f, 0);
        eff.AddComponent<HitBox>().skillInfo = skillInfo;
        GameMgr.Instance.DestroyTarget(eff, 1f);

    }

    IEnumerator BloodAbsorptionMotion(GameObject attacker, GameObject eff)
    {
        int i = 0;
        while (true)
        {
            if (eff == null) break;
            eff.transform.Translate(Vector3.back);
            i++;
            //eff.GetComponentInChildren<Transform>().localScale = new Vector3()
            if (i == 18) break;
            yield return new WaitForSeconds(0.05f);
        }
        GameMgr.Instance.DestroyTarget(eff, 0.1f);
        yield return null;
    }
}
