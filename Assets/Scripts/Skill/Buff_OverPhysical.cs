using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;

public class Buff_OverPhysical : Skill
{
    private GameObject nowEff;
    MyPosInfo myPosInfo;
    public void SetSkillNum(int Num)
    {
        skillInfo.skillNum = Num;
    }
    private void Awake()
    {        
        myPosInfo.myPos = gameObject.transform;
        myPosInfo.yPos = 3;
        myPosInfo.xPos = 0;
        myPosInfo.zPos = 0;

        skillInfo.type = SkillType.Skill;

        skillInfo.cooltime = 1;
        skillInfo.skillType = SkillType.Buff;
    }
 
    private void Update()
    {
        if (GameMgr.Instance.GameState == false) return;
        if (GameMgr.Instance.playerInput.inputKey == KeyCode.D) SkillUse();
    }

    int OPstate = 0;
    int[] lastOPstate = new int[3];
    protected override void SkillFire()
    {
        playerInfo.StayPlayer(1f);
        animator.SetTrigger("isUpperAttack");
        // init value : setting basicAttackRange == 1,movespeed == 7 , damageDecrease == 0
        if (OPstate == 0)
        {
            if (nowEff != null) GameMgr.Instance.del_DestroyTarget(nowEff, 0.1f);
            nowEff = PhotonNetwork.Instantiate("BS_RangePlusEff",transform.position,Quaternion.identity);
            playerInfo.PlayInfoChange(ChangeableInfo.basicAttackRange, -lastOPstate[0]);
            playerInfo.PlayInfoChange(ChangeableInfo.basicMoveSpeed, -lastOPstate[1]);
            playerInfo.PlayInfoChange(ChangeableInfo.damageDecrease, -lastOPstate[2]);
            playerInfo.PlayInfoChange(ChangeableInfo.basicAttackRange, 2);
            playerInfo.PlayInfoChange(ChangeableInfo.basicMoveSpeed, 0);
            playerInfo.PlayInfoChange(ChangeableInfo.damageDecrease, 0);
            lastOPstate[0] = 2;
            lastOPstate[1] = 0;
            lastOPstate[2] = 0;
            nowEff.AddComponent<MyPos>().myPosInfo = myPosInfo;
        }
        else if (OPstate == 1)
        {
            if (nowEff != null) GameMgr.Instance.del_DestroyTarget(nowEff, 0.1f);
            nowEff = PhotonNetwork.Instantiate("BS_SpeedPlusEff", transform.position, Quaternion.identity);

            playerInfo.PlayInfoChange(ChangeableInfo.basicAttackRange, -lastOPstate[0]);
            playerInfo.PlayInfoChange(ChangeableInfo.basicMoveSpeed, -lastOPstate[1]);
            playerInfo.PlayInfoChange(ChangeableInfo.damageDecrease, -lastOPstate[2]);
            playerInfo.PlayInfoChange(ChangeableInfo.basicAttackRange, 0);
            playerInfo.PlayInfoChange(ChangeableInfo.basicMoveSpeed, 7);
            playerInfo.PlayInfoChange(ChangeableInfo.damageDecrease, 0);
            lastOPstate[0] = 0;
            lastOPstate[1] = 7;
            lastOPstate[2] = 0;
            nowEff.AddComponent<MyPos>().myPosInfo = myPosInfo;
        }
        else if (OPstate == 2)
        {
            if (nowEff != null) GameMgr.Instance.del_DestroyTarget(nowEff, 0.1f);
            nowEff = PhotonNetwork.Instantiate("BS_DamageDcreasePlusEff", transform.position, Quaternion.identity);
            playerInfo.PlayInfoChange(ChangeableInfo.basicAttackRange, -lastOPstate[0]);
            playerInfo.PlayInfoChange(ChangeableInfo.basicMoveSpeed, -lastOPstate[1]);
            playerInfo.PlayInfoChange(ChangeableInfo.damageDecrease, -lastOPstate[2]);
            playerInfo.PlayInfoChange(ChangeableInfo.basicAttackRange, 0);
            playerInfo.PlayInfoChange(ChangeableInfo.basicMoveSpeed, 0);
            playerInfo.PlayInfoChange(ChangeableInfo.damageDecrease, 20);
            lastOPstate[0] = 0;
            lastOPstate[1] = 0;
            lastOPstate[2] = 20;
            nowEff.AddComponent<MyPos>().myPosInfo = myPosInfo;
        }
        else Debug.Log("음수입니다.");

        OPstate++;
        if (OPstate > 2) OPstate = 0;

        if (skillInfo.cooltime != 0) GameMgr.Instance.uIMgr.SkillCooltime(skillInfo.cooltime, skillInfo.skillNum, 0);
    }

}