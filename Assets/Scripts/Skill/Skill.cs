using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public struct SkillInfo
{
    public HitBoxInfo hitBoxInfo;
    public SkillType skillType;
    public SkillType type;
    public bool hitReturn;
    public float skillDirY;
    public float range;
    public float radius;
    public float angle;
    public float length;
    public int skillNum;
    public int cooltime;
    public int itemNum;
}
public enum SkillType
{
    Skill,
    Item,
    Buff,
    Passive,
    Immediate,
    Cone,
    NonTarget,
    Projectile,
}
public class Skill : MonoBehaviourPun
{
    protected CodeExample codeExample;
    protected SkillInfo skillInfo;
    protected Vector3 clickPos;
    protected Vector3 desiredDir;
    protected bool click;
    protected PlayerInfo playerInfo;
    protected Animator animator;


    private bool isCanSkill = true;
    private bool setRadius;
    private RaycastHit hit;
    private bool nullCheck;
    private bool nullCheckHit;
    private int mask;

    private void Start()
    {
        codeExample = FindObjectOfType<CodeExample>();
        playerInfo = GetComponent<PlayerInfo>();
        if (photonView.IsMine == true)
        {
            if (skillInfo.skillType != SkillType.Buff && skillInfo.skillType != SkillType.Passive) GameMgr.Instance.codeExample.onChangeSkillType += UnClick;
            if (skillInfo.type == SkillType.Skill)
            {
                GameMgr.Instance.uIMgr.onResetCoolTime += ResetCoolTime;
                if (skillInfo.skillType == SkillType.Immediate) GameMgr.Instance.uIMgr.onSetSkillDescription += SkillRadius;
            }
            else if (skillInfo.type == SkillType.Item)
            {
                if (skillInfo.skillType == SkillType.Immediate) GameMgr.Instance.uIMgr.onSetItemDescription += ItemRadius;
            }
        }
    }
    protected void HitReturn(HitReturnInfo hitReturnInfo)
    {
        if (hitReturnInfo.type == SkillType.Skill)
            if (skillInfo.type != hitReturnInfo.type && skillInfo.skillNum != hitReturnInfo.num) return;
        else if (hitReturnInfo.type == SkillType.Item)
            if (skillInfo.type != hitReturnInfo.type && skillInfo.itemNum != hitReturnInfo.num) return;
        HitFire(hitReturnInfo.attacker, hitReturnInfo.hit);

    }
    protected void UnClick()
    {
        Debug.Log("UnClick");
        click = false;
    }
    protected void ItemRadius(int skillNum)
    {
        Debug.Log(skillNum);
        if (skillInfo.skillType == SkillType.Immediate && skillNum == skillInfo.itemNum)
        {
            codeExample.Radius(skillInfo.radius);
        }
        else if (skillNum == 5) codeExample.Interrupt();
    }
    protected void SkillRadius()
    {
        if (setRadius == false)
        {
            codeExample.Radius(skillInfo.radius);
            setRadius = true;
        }
        else
        {
            setRadius = false;
            codeExample.Interrupt();
        }
    }
    protected void SkillUse()
    {
        if (isCanSkill == false) return;
        if (playerInfo.playerAlive == state.Die || playerInfo.playerStun == state.Stun || playerInfo.playerStay == state.Stay) return;
        if (playerInfo.playerSilence == state.Silence) return;
        codeExample.Interrupt();
        if (skillInfo.skillType == SkillType.Immediate)
        {
            if (isCanSkill == true)
            {
                isCanSkill = false;
                SkillFire();
            }
        }
        else if (skillInfo.skillType == SkillType.Projectile)
        {
            if (click == true) click = false;
            else
            {
                codeExample.Line(skillInfo.length, skillInfo.range);
                click = true;
            }
        }
        else if (skillInfo.skillType == SkillType.Cone)
        {
            if (click == true) click = false;
            else
            {
                codeExample.Cone(skillInfo.angle, skillInfo.range);
                click = true;
            }
        }
        else if (skillInfo.skillType == SkillType.NonTarget)
        {
            if (click == true) click = false;
            else
            {
                codeExample.Area(skillInfo.radius, skillInfo.range);
                click = true;
            }
        }
        else if (skillInfo.skillType == SkillType.Buff)
        {
            if (isCanSkill == true)
            {
                isCanSkill = false;
                SkillFire();
            }
        }
        else if (skillInfo.skillType == SkillType.Passive)
        {

        }
    }
    protected void SkillClick(Vector3 mousePos)
    {
        codeExample.Interrupt();
        if (playerInfo.playerAlive == state.Die || playerInfo.playerStun == state.Stun || playerInfo.playerStay == state.Stay) return;
        if (playerInfo.playerSilence == state.Silence) return;
        if (click == true)
        {
            click = false;
            mask = 1 << LayerMask.NameToLayer("Ground");
            nullCheck = Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out hit, 9999, mask);
            nullCheckHit = (nullCheck) ? hit.transform.gameObject.CompareTag("Ground") : false;
            if (nullCheckHit == true)
            {
                clickPos = hit.point;
                clickPos.y = hit.point.y+skillInfo.skillDirY;
                desiredDir = clickPos;
                clickPos.y = transform.position.y;

            }
            if (Vector3.Distance(clickPos, transform.position) > skillInfo.range) { Debug.Log("사정거리 밖"); return; }
            if (isCanSkill == true)
            {
                isCanSkill = false;
                SkillFire();
            }
        }
    }
    protected virtual void SkillFire()
    {
        Debug.Log("기존스킬발사");
    }
    protected virtual void HitFire(GameObject attacker,GameObject hit)
    {
        Debug.Log("기존스킬발사");
    }
    protected void ResetCoolTime(int skillNum)
    {
        if (skillNum == skillInfo.skillNum)
        {
            Debug.Log(skillInfo.skillNum);
            isCanSkill = true;
            Debug.Log("리셋");
        }
    }
}