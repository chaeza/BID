using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerAttack : MonoBehaviourPun
{
    private Animator myAnimator;
    private PlayerInfo playerInfo;
    private AudioSource sound;
    private bool isAttack = true;
    public delegate void AttackEvent();
    public event AttackEvent attackEvent;
    private void Start()
    {
        sound = GetComponent<AudioSource>();
        myAnimator = GetComponent<Animator>();
        playerInfo = GetComponent<PlayerInfo>();
    }  


    private void Update()
    {
        //if (photonView.IsMine == false) return;
        if (playerInfo.playerAlive == state.Die) return;
        if (playerInfo.playerStun == state.Stun) return;
        if (GameMgr.Instance.playerInput.inputKey == KeyCode.A)
        {
            Attack();
        }
    }
    [PunRPC]
    public void Attack()
    {
        //모션 
        if (isAttack == true)
        {
            if (attackEvent != null) attackEvent();
            isAttack = false;
            //모션 랜덤 설정 
            int motionNum = Random.Range(0, 3);
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
    IEnumerator Attack_Delay(int motionNum)
    {
        Debug.Log("공격");
        playerInfo.StayPlayer(0.7f);
        yield return new WaitForSeconds(0.2f);
        //GameObject eff = PhotonNetwork.Instantiate("BasicAttackEff", transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        //sound.Play();
        //GameMgr.Instance.DestroyTarget(eff, 0.5f);
        //if (motionNum == 0 || motionNum == 1) eff.transform.Rotate(0, 0, -45);
        yield return new WaitForSeconds(playerInfo.basicAttackSpeed-0.2f);
        isAttack = true;
    }
}
