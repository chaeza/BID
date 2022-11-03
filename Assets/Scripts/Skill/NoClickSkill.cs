using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NoClickSkill : MonoBehaviourPun, SkillMethod
{
    // Sound
    AudioSource skillSound;
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
            GameObject a = PhotonNetwork.Instantiate("SwampField", transform.position, Quaternion.identity);

            // ADD HitScript
            //a.AddComponent<>();

            // Assigns an attacker to the effect.
            a.SendMessage("AttackerName", gameObject.GetPhotonView().ViewID, SendMessageOptions.DontRequireReceiver);
            // Position value when skill is created
            a.transform.position = gameObject.transform.position; // + new Vector3(0, 0, 0);

            skillSound = a.GetComponent<AudioSource>();
            StartCoroutine(SoundStart());
            skillSound.Play();

            // skill remove (a, time)
            GameMgr.Instance.DestroyTarget(a, 8f);
            skillCool = true;
            Debug.Log("use skill");
            // UIMgr SkillColl send
            GameMgr.Instance.uIMgr.SkillCooltime(gameObject, 18);
        }

        //SkillSound Start
        IEnumerator SoundStart()
        {
            yield return new WaitForSeconds(7f);
            skillSound.Stop();
        }
    }


}
