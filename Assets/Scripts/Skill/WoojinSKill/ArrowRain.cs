using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ArrowRain : MonoBehaviourPun, SkillMethod
{
    private RectTransform myskillRangerect = null;
    private GameObject skilla;
    private Vector3 canSkill;

    private bool skillCool = false;
    private bool skillClick = false;
    private int skillRange = 15;

    private void Start()
    {
        myskillRangerect = GetComponent<PlayerInfo>().myskillRangerect;
        skilla = GetComponent<PlayerInfo>().skilla;
    }
    private void Update()
    {
        if (skillClick == true)
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 target;
            target.x = mousePos.x;
            target.y = mousePos.y;
            target.z = 0;

          //  skilla.transform.position = target;

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out hit, 30f);

            canSkill = hit.point;
            canSkill.y = transform.position.y;
        }
    }
    public void ResetCooltime()
    {
        // re-enable the skill
        skillCool = false;
        Debug.Log("스킬쿨끝");
    }
    public void SkillFire()
    {
        if (skillCool == false)
        {
            if (skillClick == false)
            {
               // skilla.SetActive(true);
              //  myskillRangerect.gameObject.SetActive(true);
              //  myskillRangerect.sizeDelta = new Vector2(skillRange, skillRange);
                skillClick = true;
            }

            else
            {
                skillClick = false;
              //  myskillRangerect.gameObject.SetActive(false);
              //  skilla.SetActive(false);
            }
        }
    }

    public void SkillClick(Vector3 Pos)
    {
        if (skillClick == true)
        {
            skillClick = false;
            myskillRangerect.gameObject.SetActive(false);
            skilla.SetActive(false);

            if (Vector3.Distance(canSkill, transform.position) > skillRange / 2) return;

            RaycastHit hit;
            Vector3 desiredDir = Vector3.zero;
            Ray ray = Camera.main.ScreenPointToRay(Pos);
            int mask = 1 << LayerMask.NameToLayer("Terrain");
            Physics.Raycast(Camera.main.ScreenPointToRay(Pos), out hit, 30f, mask);

            //if the skill is available
            if (hit.collider.tag == "Ground" || hit.collider.tag == "UnGround")
            {
                desiredDir = hit.point;
                desiredDir.y = transform.position.y;
            }
            if (skillCool == false)
            {
                //  GetComponent<Animator>().SetTrigger("isSkill2");
                transform.LookAt(desiredDir);
                //player waiting time
                //GetComponent<PlayerInfo>().Stay(0.1f);
                // skill activation time
                StartCoroutine(Stay(desiredDir, 0.2f));
                // Turn on the cooldown so that it cannot be used again
                skillCool = true;
                Debug.Log("스킬사용");
                // Send cooldown x seconds to UI manager
                // GameMgr.Instance.uIMgr.SkillCooltime(gameObject, 2);
            }
        }
    }
    IEnumerator Stay(Vector3 desiredDir, float time)
    {
        yield return new WaitForSeconds(time);
        // Instance the effect as a photon.
        GameObject skill = PhotonNetwork.Instantiate("ArrowRain", transform.position, Quaternion.identity);
        skill.AddComponent<ArrowRainHit>();//이펙트에 히트 스크립트를 넣습니다.
        // Assign an attacker to the effect.
        skill.SendMessage("AttackerName", gameObject.GetPhotonView().ViewID, SendMessageOptions.DontRequireReceiver);
        skill.transform.LookAt(desiredDir);
        skill.transform.Rotate(0, -80, 0);
        skill.GetComponent<BoxCollider>().enabled = true;

        //   GameMgr.Instance.DestroyTarget(skill, 2f);

        yield break;
    }
}
