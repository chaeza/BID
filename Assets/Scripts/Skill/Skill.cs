using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public struct SkillInfo
{
    public DamageInfo skillDamageInfo;
    public SkillType skillType;
    public float range;
    public float radius;
    public float angle;
    public float length;
    public float cooltime;
}
public enum SkillType
{
    Buff,
    Passive,
    Immediate,
    Cone,
    NonTarget,
    Projectile,
}
public class Skill : MonoBehaviourPun
{
    protected SkillInfo skillInfo;
    protected CodeExample codeExample;
    protected bool click;
    protected Vector3 clickPos;
    protected Vector3 desiredDir;

    private RaycastHit hit;
    private bool nullCheck;
    private bool nullCheckHit;
    private int mask;

    private void Awake()
    {
        codeExample = FindObjectOfType<CodeExample>();
    }
    protected void SkillUse()
    {
        if (skillInfo.skillType == SkillType.Immediate)
        {
            codeExample.Interrupt();
            codeExample.Radius(skillInfo.radius);
            SkillFire();
        }
        else if (skillInfo.skillType == SkillType.Projectile)
        {
            if (click == true)
            {
                codeExample.Interrupt();
                click = false;
            }
            else
            {
                Debug.Log("사용");
                codeExample.Line(skillInfo.length, skillInfo.range);
                click = true;
            }
        }
        else if (skillInfo.skillType == SkillType.Cone)
        {
            if (click == true)
            {
                codeExample.Interrupt();
                click = false;
            }
            else
            {
                Debug.Log("사용");
                codeExample.Cone(skillInfo.angle, skillInfo.range);
                click = true;
            }
        }
        else if (skillInfo.skillType == SkillType.NonTarget)
        {
            if (click == true)
            {
                codeExample.Interrupt();
                click = false;
            }
            else
            {
                codeExample.Area(skillInfo.radius, skillInfo.range);
                click = true;
            }
        }
        else if (skillInfo.skillType == SkillType.Buff)
        {
            SkillFire();
        }
        else if (skillInfo.skillType == SkillType.Passive)
        {

        }
    }
    protected void SkillClick(Vector3 mousePos)
    {
        codeExample.Interrupt();
        if (click == true)
        {
            click = false;
            mask = 1 << LayerMask.NameToLayer("Ground");
            nullCheck = Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out hit, 9999, mask);
            nullCheckHit = (nullCheck) ? hit.transform.gameObject.CompareTag("Ground") : false;
            if (nullCheckHit == true)
            {
                clickPos = hit.point;
                clickPos.y = transform.position.y;
                desiredDir = hit.point;
            }
            if (Vector3.Distance(clickPos, transform.position) > skillInfo.range) { Debug.Log("사정거리 밖"); return; }
            SkillFire();
        }
    }
    protected virtual void SkillFire()
    {
        Debug.Log("기존스킬발사");
    }
}