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
        skillInfo.hitBoxInfo.damageInfo.attackDamage = 15;
        skillInfo.hitBoxInfo.damageInfo.attackerViewID = gameObject.GetPhotonView().ViewID;
        skillInfo.hitBoxInfo.damageInfo.slowDownRate = 0;
        skillInfo.hitBoxInfo.damageInfo.timer = 0f;
    }

    private void Update()
    {
        if (GameMgr.Instance.GameState == false) return;
        if (GameMgr.Instance.playerInput.inputKey == KeyCode.D) SkillUse();
        else if (GameMgr.Instance.playerInput.inputKey == KeyCode.Mouse0) SkillClick(Input.mousePosition);
    }
    protected override void SkillFire()
    {
        playerInfo.StayPlayer(0.5f);
        animator.SetTrigger("BloodAttack");
        transform.LookAt(desiredDir);
        StartCoroutine(SkillFire_Delay(0.5f));
        if (skillInfo.cooltime != 0) GameMgr.Instance.uIMgr.SkillCooltime(skillInfo.cooltime, skillInfo.skillNum, 0);
    }
    protected override void HitFire(GameObject attacker, GameObject hit)
    {
        GameObject eff1 = PhotonNetwork.Instantiate("BloodAbsorption", hit.transform.position, Quaternion.identity);
        GameObject eff2 = PhotonNetwork.Instantiate("blood_01", hit.transform.position + new Vector3(0, 2, 0), Quaternion.identity);
        GameMgr.Instance.DestroyTarget(eff2, 1f);
        eff1.transform.LookAt(attacker.transform);
        eff1.transform.Rotate(new Vector3(0, 180f, 0));
        StartCoroutine(BloodAbsorptionMotion(attacker, eff1));
        attacker.GetPhotonView().RPC("ChangeHP", RpcTarget.All, 15f);

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
        while (true)
        {
            if (eff == null) break;
            eff.transform.Translate(Vector3.back);
            Vector3 apos = attacker.transform.position;
            float distance = Vector3.Distance(eff.transform.position, apos);
            //eff.GetComponentInChildren<Transform>().localScale = new Vector3()
            if (distance <= 2f) break;
            yield return new WaitForSeconds(0.05f);
        }
        GameMgr.Instance.DestroyTarget(eff, 0.1f);
        yield return null;
    }
}
