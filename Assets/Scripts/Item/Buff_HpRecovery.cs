using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Buff_HpRecovery : Skill
{
    [SerializeField] private int itemNum = 0;
    PlayerInfo playerInfo;
    public void GetItem(int itemType,int itemnum)//inventory order
    {
        if (itemNum == 0)
        {
            itemNum = itemnum;
            skillInfo.itemNum = itemNum;
            skillInfo.skillNum = itemType;
            GameMgr.Instance.uIMgr.SetItemIcon(itemType,itemNum);
        }
    }
    private void Awake()
    {
        playerInfo = GetComponent<PlayerInfo>();
        skillInfo.type = SkillType.Item;
        skillInfo.angle = 20;//use Cone
        skillInfo.radius = 10;//use Immediate,NonTarget
        skillInfo.range = 30;//use Projectile,NonTarget,Cone
        skillInfo.length = 6;//use Projectile,
        skillInfo.cooltime = 0;
        skillInfo.skillType = SkillType.Immediate;

        skillInfo.hitBoxInfo.attackType = AttackType.Shot;
        skillInfo.hitBoxInfo.interval = 0;

        skillInfo.hitBoxInfo.damageInfo.attackState = state.None;
        skillInfo.hitBoxInfo.damageInfo.attackDamage = 0;
        skillInfo.hitBoxInfo.damageInfo.attackerViewID = gameObject.GetPhotonView().ViewID;
        skillInfo.hitBoxInfo.damageInfo.slowDownRate = 0;
        skillInfo.hitBoxInfo.damageInfo.timer = 0;
    }
    private void Update()
    {
        if (itemNum == 0 && GameMgr.Instance.playerInput.inputKey == KeyCode.Q) SkillUse();
        else if (itemNum == 1 && GameMgr.Instance.playerInput.inputKey == KeyCode.W) SkillUse();
        else if (itemNum == 2 && GameMgr.Instance.playerInput.inputKey == KeyCode.E) SkillUse();
        else if (itemNum == 3 && GameMgr.Instance.playerInput.inputKey == KeyCode.R) SkillUse();
        else if (GameMgr.Instance.playerInput.inputKey == KeyCode.Mouse0) SkillClick(Input.mousePosition);
    }
    protected override void SkillFire()
    {
        if (skillInfo.skillType == SkillType.Immediate) GameMgr.Instance.uIMgr.onSetItemDescription -= ItemRadius;
        if (skillInfo.skillType != SkillType.Buff && skillInfo.skillType != SkillType.Passive) GameMgr.Instance.codeExample.onChangeSkillType -= UnClick;

        GameObject eff = PhotonNetwork.Instantiate("HpRecovery", transform.position, Quaternion.identity);
        MyPosInfo myPosInfo;
        myPosInfo.myPos = gameObject.transform;
        myPosInfo.yPos = 1;
        eff.AddComponent<MyPos>().myPosInfo = myPosInfo;
        photonView.RPC("ChangeHP", RpcTarget.All, 30);

        GameMgr.Instance.uIMgr.UseItem(itemNum);
        Destroy(GetComponent<Buff_HpRecovery>());
    }
}
