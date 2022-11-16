using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Buff_BasicAttackUp : Skill
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
        skillInfo.skillType = SkillType.Buff;
    }
    private void Update()
    {
        if (GameMgr.Instance.GameState == false) return;
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

        //
        Debug.Log("아이템사용");
        playerInfo.PlayInfoChange(ChangeableInfo.basicAttackDamage, 2f);
        GameObject eff = PhotonNetwork.Instantiate("BasicAttackDamageEff", transform.position, Quaternion.identity);
        //
        GameMgr.Instance.uIMgr.UseItem(itemNum);
        Destroy(GetComponent<Buff_BasicAttackUp>());
    }
}
