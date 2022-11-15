using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;

public class Projectile_Bash : Skill
{
    public void SetSkillNum(int Num)
    {
        skillInfo.skillNum = Num;
    }

    private NavMeshAgent navMeshAgent;
    private bool dashAttack = false;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        skillInfo.type = SkillType.Skill;
        skillInfo.angle = 0;//use Cone
        skillInfo.radius = 0;//use Immediate,NonTarget
        skillInfo.range = 18;//use Projectile,NonTarget,Cone
        skillInfo.length = 20;//use Projectile,
        skillInfo.cooltime = 20;
        skillInfo.skillType = SkillType.Projectile;
        skillInfo.skillDirY = 0;
        skillInfo.hitReturn = true;
        skillInfo.hitBoxInfo.attackType = AttackType.Shot;
        skillInfo.hitBoxInfo.interval = 1;

        skillInfo.hitBoxInfo.damageInfo.attackState = state.Stun;
        skillInfo.hitBoxInfo.damageInfo.attackDamage = 10;
        skillInfo.hitBoxInfo.damageInfo.attackerViewID = gameObject.GetPhotonView().ViewID;
        skillInfo.hitBoxInfo.damageInfo.slowDownRate = 0;
        skillInfo.hitBoxInfo.damageInfo.timer = 2f;
    }
    private void Update()
    {
        if (GameMgr.Instance.playerInput.inputKey == KeyCode.D) SkillUse();
        else if (GameMgr.Instance.playerInput.inputKey == KeyCode.Mouse0) SkillClick(Input.mousePosition);

        if (dashAttack == true)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.updateRotation = true;
            navMeshAgent.updatePosition = true;

            navMeshAgent.SetDestination(desiredDir);
        }
    }
    Coroutine dash;
    protected override void SkillFire()
    {
        playerInfo.StayPlayer(1f);
        animator.SetTrigger("isBash");
        GameObject eff = PhotonNetwork.Instantiate("Bash", transform.position, Quaternion.identity);
        eff.AddComponent<HitBox>().skillInfo = skillInfo;
        eff.transform.LookAt(desiredDir);
        eff.transform.Rotate(0, 180, 0);
        MyPosInfo myPosInfo;
        myPosInfo.myPos = gameObject.transform;
        myPosInfo.zPos = -3.5f;
        myPosInfo.xPos = 0f;
        myPosInfo.yPos = 0f;
        eff.AddComponent<MyPos>().myPosInfo = myPosInfo;
        dash= StartCoroutine(EndSkill(eff));
        if (skillInfo.cooltime != 0) GameMgr.Instance.uIMgr.SkillCooltime(skillInfo.cooltime, skillInfo.skillNum, 0);
    }
    protected override void HitFire(GameObject attacker, GameObject hit)
    {
        StopCoroutine(dash);
        playerInfo.StayPlayer(0f);
        GameObject a = PhotonNetwork.Instantiate("WarofWall", transform.position, Quaternion.identity);
        GameMgr.Instance.DestroyTarget(a, 5f);
    }
    IEnumerator EndSkill(GameObject eff)
    {
        navMeshAgent.speed = 14f;
        dashAttack = true;
        yield return new WaitForSeconds(1f);
        dashAttack = false;
        navMeshAgent.speed = 7f;
        GameMgr.Instance.DestroyTarget(eff,0f);
    }

}