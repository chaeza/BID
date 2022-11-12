using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;

public class Projectile_Bash : Skill
{
    [SerializeField] private GameObject Eff;
    private Animator anim;
    private NavMeshAgent navMeshAgent;
    private bool dashAttack = false;
    

    //PlayerInfo playerInfo;
    //private void Awake()
    //{
    //    playerInfo = GetComponent<PlayerInfo>();
    //}
    private void Awake()
    {
        anim = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        skillInfo.type = SkillType.Skill;
        skillInfo.angle = 0;//use Cone
        skillInfo.radius = 0;//use Immediate,NonTarget
        skillInfo.range = 20;//use Projectile,NonTarget,Cone
        skillInfo.length = 20;//use Projectile,
        skillInfo.cooltime = 2;
        skillInfo.skillNum = 6;
        skillInfo.skillType = SkillType.Projectile;
        skillInfo.skillDirY = 0;
        skillInfo.hitBoxInfo.attackType = AttackType.Shot;
        skillInfo.hitBoxInfo.interval = 1;

        skillInfo.hitBoxInfo.damageInfo.attackState = state.None;
        skillInfo.hitBoxInfo.damageInfo.attackDamage = 10;
        skillInfo.hitBoxInfo.damageInfo.attackerViewID = gameObject.GetPhotonView().ViewID;
        skillInfo.hitBoxInfo.damageInfo.slowDownRate = 0;
        skillInfo.hitBoxInfo.damageInfo.timer = 0;
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
    protected override void SkillFire()
    {
        anim.SetTrigger("isBash");
        GameObject eff = PhotonNetwork.Instantiate("Bash", transform.position, Quaternion.identity);
        eff.AddComponent<HitBox>().hitBoxInfo = skillInfo.hitBoxInfo;
        eff.transform.LookAt(desiredDir);
        eff.transform.Rotate(0, 180, 0);
        MyPosInfo myPosInfo;
        myPosInfo.myPos = gameObject.transform;
        myPosInfo.zPos = -3.5f;
        myPosInfo.xPos = 0f;
        myPosInfo.yPos = 0f;
        eff.AddComponent<MyPos>().myPosInfo = myPosInfo;
        StartCoroutine(EndSkill(eff));
        if (skillInfo.cooltime != 0) GameMgr.Instance.uIMgr.SkillCooltime(skillInfo.cooltime, skillInfo.skillNum, 0);
    }

    IEnumerator EndSkill(GameObject eff)
    {
        navMeshAgent.speed = 40f;
        dashAttack = true;
        yield return new WaitForSeconds(0.5f);
        dashAttack = false;
        navMeshAgent.speed = 10f;
        GameMgr.Instance.DestroyTarget(eff,0f);
    }

}