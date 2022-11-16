using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;

public class Buff_OverPhysical: Skill
{
    public void SetSkillNum(int Num)
    {
        skillInfo.skillNum = Num;
    }
    private void Awake()
    {
        skillInfo.type = SkillType.Skill;
      
        skillInfo.cooltime = 1;
        skillInfo.skillType = SkillType.Buff;
    }
    
    private void Update()
    {
        if (GameMgr.Instance.GameState == false) return;
        if (GameMgr.Instance.playerInput.inputKey == KeyCode.D) SkillUse();
    }

    int OPstate =0;
    int[] lastOPstate = new int[3];
    protected override void SkillFire()
    {
        playerInfo.StayPlayer(1f);

        if (OPstate == 0) 
        {
            playerInfo.PlayInfoChange(ChangeableInfo.basicAttackRange, -lastOPstate[0]);
            playerInfo.PlayInfoChange(ChangeableInfo.moveSpeed, -lastOPstate[1]);
            playerInfo.PlayInfoChange(ChangeableInfo.damageDecrease, -lastOPstate[2]);

            // basicAttackRange == 1,movespeed == 7 , damageDecrease == 0
            playerInfo.PlayInfoChange(ChangeableInfo.basicAttackRange, 2); 
            playerInfo.PlayInfoChange(ChangeableInfo.moveSpeed, 0);
            playerInfo.PlayInfoChange(ChangeableInfo.damageDecrease, 0);

            lastOPstate[0] = 2;
            lastOPstate[1] = 0;
            lastOPstate[2] = 0;
        }
        else if (OPstate == 1)
        {
            playerInfo.PlayInfoChange(ChangeableInfo.basicAttackRange, -lastOPstate[0]);
            playerInfo.PlayInfoChange(ChangeableInfo.moveSpeed, -lastOPstate[1]);
            playerInfo.PlayInfoChange(ChangeableInfo.damageDecrease, -lastOPstate[2]);
            playerInfo.PlayInfoChange(ChangeableInfo.basicAttackRange, 0);
            playerInfo.PlayInfoChange(ChangeableInfo.moveSpeed, 7);
            playerInfo.PlayInfoChange(ChangeableInfo.damageDecrease, 0);
            lastOPstate[0] = 0;
            lastOPstate[1] = 7;
            lastOPstate[2] = 0;
        }
        else if (OPstate == 2)
        {
            playerInfo.PlayInfoChange(ChangeableInfo.basicAttackRange, -lastOPstate[0]);
            playerInfo.PlayInfoChange(ChangeableInfo.moveSpeed, -lastOPstate[1]);
            playerInfo.PlayInfoChange(ChangeableInfo.damageDecrease, -lastOPstate[2]);
            playerInfo.PlayInfoChange(ChangeableInfo.basicAttackRange, 0);
            playerInfo.PlayInfoChange(ChangeableInfo.moveSpeed, 0);
            playerInfo.PlayInfoChange(ChangeableInfo.damageDecrease, 20);
            lastOPstate[0] = 0;
            lastOPstate[1] = 0;
            lastOPstate[2] = 20;
        }
        else Debug.Log("음수입니다.");

        OPstate++;
        if (OPstate >= 2) OPstate = 0;

      /*  //EFF
        GameObject eff = PhotonNetwork.Instantiate("Bash", transform.position, Quaternion.identity);
      
        //Eff position & Player Position Sync
        MyPosInfo myPosInfo;
        myPosInfo.myPos = gameObject.transform;
        myPosInfo.zPos = -3.5f;
        myPosInfo.xPos = 0f;
        myPosInfo.yPos = 0f;
        eff.AddComponent<MyPos>().myPosInfo = myPosInfo;*/

        
        if (skillInfo.cooltime != 0) GameMgr.Instance.uIMgr.SkillCooltime(skillInfo.cooltime, skillInfo.skillNum, 0);
    }

}