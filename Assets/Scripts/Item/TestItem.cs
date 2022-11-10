using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TestItem : Skill
{
    private int itemNum = 0;
    public void GetItem(int itemnum) { if(itemNum == 0) itemNum = itemnum; }

    private void Start()
    {
        skillInfo.angle = 20;//use Cone
        skillInfo.radius = 6;//use Immediate,NonTarget
        skillInfo.range = 30;//use Projectile,NonTarget,Cone
        skillInfo.length = 6;//use Projectile,
        skillInfo.cooltime = 0;
        skillInfo.skillType = SkillType.Projectile;

        skillInfo.skillDamageInfo.attackType = AttackType.Shot;
        skillInfo.skillDamageInfo.interval = 0;
        skillInfo.skillDamageInfo.attackDamage = 0;
        skillInfo.skillDamageInfo.attackerViewID = gameObject.GetPhotonView().ViewID;
        skillInfo.skillDamageInfo.attackState = state.None;
        skillInfo.skillDamageInfo.slowDownRate = 0;
        skillInfo.skillDamageInfo.timer = 0;
    }
    private void Update()
    {
        if (itemNum == 1 && GameMgr.Instance.playerInput.inputKey == KeyCode.Q) SkillUse();
        else if (itemNum == 2 && GameMgr.Instance.playerInput.inputKey == KeyCode.W) SkillUse();
        else if (itemNum == 3 && GameMgr.Instance.playerInput.inputKey == KeyCode.E) SkillUse();
        else if (itemNum == 4 && GameMgr.Instance.playerInput.inputKey == KeyCode.R) SkillUse();
        else if (GameMgr.Instance.playerInput.inputKey == KeyCode.Mouse0) SkillClick(Input.mousePosition);
    }
    protected override void SkillFire()
    {
        Debug.Log("아이템사용");
    }
}
