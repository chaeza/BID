using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Dash : Skill
{

    private void Awake()
    {
        skillInfo.type = SkillType.Skill;
        skillInfo.cooltime = 5;
        skillInfo.skillNum = 0;
        skillInfo.skillType = SkillType.Buff;
        GameMgr.Instance.uIMgr.SetSkillIcon(0, 1);
    }
    private void Update()
    {
        if (GameMgr.Instance.playerInput.inputKey == KeyCode.F) SkillUse();
    }
    protected override void SkillFire()
    {
        photonView.RPC("SetChangeMoveSpeed", RpcTarget.All, 400f, 0.5f);
        photonView.RPC("SetGhostEff", RpcTarget.All, 10, 0.05f);

        if (skillInfo.cooltime != 0) GameMgr.Instance.uIMgr.SkillCooltime(skillInfo.cooltime, skillInfo.skillNum,1);
    }
}
