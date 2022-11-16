using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;

public class Buff_OverPhysical : Skill
{
    [SerializeField] private GameObject rangeEff;
    [SerializeField] private GameObject speedEff;
    [SerializeField] private GameObject damageDcreaseEffect;
    [SerializeField] private GameObject nowEffect;

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
    private void Start()
    {
        rangeEff = PhotonNetwork.Instantiate("BS_RangePlusEff", transform.position, Quaternion.identity);
        speedEff = PhotonNetwork.Instantiate("BS_SpeedPlusEff", transform.position, Quaternion.identity);
        damageDcreaseEffect = PhotonNetwork.Instantiate("BS_DamageDcreasePlusEff", transform.position, Quaternion.identity);

        rangeEff.SetActive(false);
        speedEff.SetActive(false);
        damageDcreaseEffect.SetActive(false);
    }

    private void Update()
    {
        if (GameMgr.Instance.GameState == false) return;
        if (GameMgr.Instance.playerInput.inputKey == KeyCode.D) SkillUse();
        if (nowEffect != null) nowEffect.transform.position = gameObject.transform.position + Vector3.up;
    }

    int OPstate = 0;
    int[] lastOPstate = new int[3];
    protected override void SkillFire()
    {
        playerInfo.StayPlayer(1f);
        // init value : setting basicAttackRange == 1,movespeed == 7 , damageDecrease == 0
        if (OPstate == 0)
        {
            rangeEff.SetActive(true);
            speedEff.SetActive(false);
            damageDcreaseEffect.SetActive(false);
            nowEffect = rangeEff;
            playerInfo.PlayInfoChange(ChangeableInfo.basicAttackRange, -lastOPstate[0]);
            playerInfo.PlayInfoChange(ChangeableInfo.moveSpeed, -lastOPstate[1]);
            playerInfo.PlayInfoChange(ChangeableInfo.damageDecrease, -lastOPstate[2]);
            playerInfo.PlayInfoChange(ChangeableInfo.basicAttackRange, 2);
            playerInfo.PlayInfoChange(ChangeableInfo.moveSpeed, 0);
            playerInfo.PlayInfoChange(ChangeableInfo.damageDecrease, 0);
            lastOPstate[0] = 2;
            lastOPstate[1] = 0;
            lastOPstate[2] = 0;
        }
        else if (OPstate == 1)
        {
            rangeEff.SetActive(false);
            speedEff.SetActive(true);
            damageDcreaseEffect.SetActive(false);
            nowEffect = speedEff;
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
            rangeEff.SetActive(false);
            speedEff.SetActive(false);
            damageDcreaseEffect.SetActive(true);
            nowEffect = damageDcreaseEffect;
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

        if (skillInfo.cooltime != 0) GameMgr.Instance.uIMgr.SkillCooltime(skillInfo.cooltime, skillInfo.skillNum, 0);
    }

}