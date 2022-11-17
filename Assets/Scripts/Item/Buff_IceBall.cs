using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Buff_IceBall : Skill
{
    [SerializeField] private int itemNum = 0;
    public void GetItem(int itemType, int itemnum)//inventory order
    {
        if (itemNum == 0)
        {
            itemNum = itemnum;
            skillInfo.itemNum = itemNum;
            skillInfo.skillNum = itemType;
            GameMgr.Instance.uIMgr.SetItemIcon(itemType, itemNum);
        }
    }
    private void Awake()
    {
        skillInfo.type = SkillType.Item;
        skillInfo.cooltime = 0;
        skillInfo.skillType = SkillType.Buff;

        skillInfo.hitBoxInfo.attackType = AttackType.Shot;
        skillInfo.hitBoxInfo.interval = 1;
        skillInfo.hitReturn = true;

        skillInfo.hitBoxInfo.damageInfo.attackState = state.Stun;
        skillInfo.hitBoxInfo.damageInfo.attackDamage = 0;
        skillInfo.hitBoxInfo.damageInfo.attackerViewID = gameObject.GetPhotonView().ViewID;
        skillInfo.hitBoxInfo.damageInfo.slowDownRate = 0;
        skillInfo.hitBoxInfo.damageInfo.timer = 2f;
    }
    private void Update()
    {
        if (itemNum == 0 && GameMgr.Instance.playerInput.inputKey == KeyCode.Q) SkillUse();
        else if (itemNum == 1 && GameMgr.Instance.playerInput.inputKey == KeyCode.W) SkillUse();
        else if (itemNum == 2 && GameMgr.Instance.playerInput.inputKey == KeyCode.E) SkillUse();
        else if (itemNum == 3 && GameMgr.Instance.playerInput.inputKey == KeyCode.R) SkillUse();
        else if (GameMgr.Instance.playerInput.inputKey == KeyCode.Mouse0) SkillClick(Input.mousePosition);
    }
    protected override void HitFire(GameObject attacker, GameObject hit)
    {
        GameObject eff = PhotonNetwork.Instantiate("MysticArrow_Boom", hit.transform.position + new Vector3(0, 2, 0), Quaternion.identity);
        GameMgr.Instance.DestroyTarget(eff, 1f);
    }
    protected override void SkillFire()
    {
        if (skillInfo.skillType == SkillType.Immediate) GameMgr.Instance.uIMgr.onSetItemDescription -= ItemRadius;
        if (skillInfo.skillType != SkillType.Buff && skillInfo.skillType != SkillType.Passive) GameMgr.Instance.codeExample.onChangeSkillType -= UnClick;
        
        GameObject iceBall = PhotonNetwork.Instantiate("IceBall", transform.position + new Vector3(0, 2, 0), Quaternion.identity);
        iceBall.AddComponent<HitBox>().skillInfo = skillInfo;
        //
        GameMgr.Instance.uIMgr.UseItem(itemNum);
        Destroy(GetComponent<Buff_IceBall>());

    }
}
