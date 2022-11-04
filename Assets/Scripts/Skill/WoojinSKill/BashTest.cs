using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BashTest : MonoBehaviourPun, SkillMethod
{
    // Sound
    private AudioSource skillSound;
    // Skill Cool 
    private bool skillCool = false;


   



    //Skill cooldown check
    public void ResetCooltime()
    {
        skillCool = false;//��ų�� �ٽ� ��� �����ϰ���
        Debug.Log("��ų��");
    }

    public void SkillFire()
    {
        if (skillCool == false)
        {
            // The effect is a photon instance.
            GameObject skill = PhotonNetwork.Instantiate("Bash", transform.position, Quaternion.identity);

            // ADD HitScript
            // a.AddComponent<>();

            // Assigns an attacker to the effect.
           // skill.SendMessage("AttackerName", gameObject.GetPhotonView().ViewID, SendMessageOptions.DontRequireReceiver);
            // Position value when skill is created
            skill.transform.position = gameObject.transform.position; // + new Vector3(0, 0, 0);
            skill.transform.rotation = gameObject.transform.rotation;
            StartCoroutine(Fire(skill));

            skill.transform.Rotate(0, 0, 0);
           
            //// skillSound = skill.GetComponent<AudioSource>();
            //StartCoroutine(SoundStart());
            //skillSound.Play();

            // skill remove (a, time)
            // GameMgr.Instance.DestroyTarget(skill, 8f);
           // skillCool = true;
            Debug.Log("use skill");

            Destroy(skill, 3f);
            // UIMgr SkillCool send
            // GameMgr.Instance.uIMgr.SkillCooltime(gameObject, 18);
        }
    }

    IEnumerator Fire(GameObject skill)//ť�� �̵���Ű��
    {
        for (int i = 0; i < 20; i++)
        {
            skill.transform.Translate(Vector3.forward * 1f);
            yield return null;
        }
        yield break;
    }
}
