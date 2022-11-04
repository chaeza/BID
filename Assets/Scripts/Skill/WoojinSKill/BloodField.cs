using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BloodField : MonoBehaviourPun , SkillMethod
{
    // Sound
    private AudioSource skillSound;
    // Skill Cool 
    private bool skillCool = false;

    //Skill cooldown check
    public void ResetCooltime()
    {
        skillCool = false;//스킬을 다시 사용 가능하게함
        Debug.Log("스킬쿨끝");
    }

    public void SkillFire()
    {
        if (skillCool == false)
        {
            // The effect is a photon instance.
            GameObject skill = PhotonNetwork.Instantiate("BloodField", transform.position, Quaternion.identity);

            // ADD HitScript
            skill.AddComponent<BloodFieldHit>();

            // Assigns an attacker to the effect.
            //skill.SendMessage("AttackerName", gameObject.GetPhotonView().ViewID, SendMessageOptions.DontRequireReceiver);
            // Position value when skill is created
            skill.transform.position = gameObject.transform.position  + new Vector3(0, 3.5f, 0);
            skill.transform.Rotate(-90f, 0f, 0f);

            // skillSound = skill.GetComponent<AudioSource>();
            //  StartCoroutine(SoundStart());
            //skillSound.Play();



            // skill remove (a, time)

            Destroy(skill, 5f);
           // GameMgr.Instance.DestroyTarget(skill, 8f);
           // skillCool = true;
            Debug.Log("use skill");
            // UIMgr SkillCool send
           // GameMgr.Instance.uIMgr.SkillCooltime(gameObject, 18);
        }

        //SkillSound Start
        IEnumerator SoundStart()
        {
            yield return new WaitForSeconds(7f);
            skillSound.Stop();
        }
    }
}
