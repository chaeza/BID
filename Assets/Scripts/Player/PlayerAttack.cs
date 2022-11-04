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

    private void Start()
    {
        sound = GetComponent<AudioSource>();
        myAnimator = GetComponent<Animator>();
        playerInfo = GetComponent<PlayerInfo>();
    }


    private void Update()
    {
        if (GameMgr.Instance.playerInput.inputKey == KeyCode.D)
            SendMessage("SkillFire", SendMessageOptions.DontRequireReceiver);
    }
}
