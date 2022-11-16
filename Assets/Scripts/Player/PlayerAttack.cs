using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class PlayerAttack : MonoBehaviourPun
{
    private Animator myAnimator;
    private PlayerInfo playerInfo;
    private bool isAttack = true;
    private int motionNum;
    private AudioSource sound;
    protected SkillInfo skillInfo;
    private void Start()
    {
        //sound = GetComponent<AudioSource>();
        myAnimator = GetComponent<Animator>();
        playerInfo = GetComponent<PlayerInfo>();
        
        skillInfo.type = SkillType.Skill;
        skillInfo.skillType = SkillType.Passive;
        skillInfo.hitReturn = false;

        skillInfo.hitBoxInfo.attackType = AttackType.Shot;
        skillInfo.hitBoxInfo.interval = 0;

        skillInfo.hitBoxInfo.damageInfo.attackState = state.None;
        skillInfo.hitBoxInfo.damageInfo.attackDamage = 10;
        skillInfo.hitBoxInfo.damageInfo.attackerViewID = gameObject.GetPhotonView().ViewID;
        skillInfo.hitBoxInfo.damageInfo.slowDownRate = 0;
        skillInfo.hitBoxInfo.damageInfo.timer = 0;
    }

    private void Update()
    {
        if (GameMgr.Instance.GameState == false) return;
        if (photonView.IsMine != true) return;
        if (playerInfo.playerAlive == state.Die || playerInfo.playerStun == state.Stun || playerInfo.playerStay == state.Stay) return;
        if (GameMgr.Instance.playerInput.inputKey == KeyCode.A) Attack();
    }

    public void Attack()
    {
        //모션 
        if (isAttack == true)
        {
            isAttack = false;
            //모션 랜덤 설정 
            motionNum = Random.Range(0, 3);
            switch (motionNum)
            {
                case 0:
                    {
                        myAnimator.SetTrigger("isAttack1");
                        break;
                    }
                case 1:
                    {
                        myAnimator.SetTrigger("isAttack2");
                        break;
                    }
                case 2:
                    {
                        myAnimator.SetTrigger("isAttack3");
                        break;
                    }
            }
            StartCoroutine(Attack_Delay(motionNum));
        }

    }

    IEnumerator Attack_Delay(int num)
    {
        playerInfo.StayPlayer(0.7f);
        yield return new WaitForSeconds(0.2f);
        GameObject eff = PhotonNetwork.Instantiate("BasicAttackEff", transform.position + new Vector3(0, 2.5f, 0), Quaternion.identity);
        eff.gameObject.transform.localScale = Vector3.one*3* playerInfo.basicAttackRange;
        if (num == 0 || num == 1) eff.transform.Rotate(0, 0, -30);
        eff.AddComponent<HitBox>().skillInfo = skillInfo;
        eff.GetComponent<HitBox>().DestroyHitBox(0.2f);
        GameMgr.Instance.DestroyTarget(eff, 1f);
        //
        yield return new WaitForSeconds(playerInfo.basicAttackSpeed-0.4f);
        isAttack = true;
    }


}
