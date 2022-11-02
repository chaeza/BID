using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;



public class PlayerMove : MonoBehaviourPun
{
    private PlayerInfo playerInfo;
    private Animator myAnimator;
    private NavMeshAgent navMeshAgent;

    private Vector3 clickPos = Vector3.one;
    private Vector3 desiredDir;
    private bool isMove = false;

    private void Start()
    {
        playerInfo = GetComponent<PlayerInfo>();
        myAnimator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = playerInfo.moveSpeed;
    }
    public void ChageSpeed(float speed)
    {
        navMeshAgent.speed = speed;
    }

    private void Update()
    {
        if (photonView.IsMine == false) return;
        if (GameMgr.Instance.playerInput.inputKey2 == KeyCode.Mouse1)
        {
            clickPos = Input.mousePosition;
            clickPos.z = 18f;
        }

        if (GameMgr.Instance.playerInput.inputKey == KeyCode.S)
        {
            MoveStop();
        }

        if (isMove == true)
        {
            if (Vector3.Distance(desiredDir, transform.position) > 0.1f)
            {
                myAnimator.SetBool("isMove", true);
                navMeshAgent.isStopped = false;
                navMeshAgent.updateRotation = true;
                navMeshAgent.updatePosition = true;

                navMeshAgent.SetDestination(desiredDir);
            }
            else
                MoveStop();
        }



    }
    public void Move(Vector3 mousePos)
    {
        // 움직임 애니메이션
        // 사운드
        // 실제 움직임 (포지션 변경)
        RaycastHit hit;
        int mask = 1 << LayerMask.NameToLayer("Terrain");
        bool nullCheck = Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out hit, 9999, mask);

        bool nullCheckHit = (nullCheck) ? hit.transform.gameObject.CompareTag("Ground") : false;
        bool nullCheckHit2 = (nullCheck) ? hit.transform.gameObject.CompareTag("UnGround") : false;


        if (nullCheckHit == true || nullCheckHit2 == true)
        {
            desiredDir = hit.point;
            desiredDir.y = transform.position.y;
            isMove = true;
        }
    }

    public void MoveStop()
    {
        myAnimator.SetBool("isMove", false);
        navMeshAgent.isStopped = true;
        navMeshAgent.velocity = Vector3.zero;
        navMeshAgent.updateRotation = false;
        navMeshAgent.updatePosition = false;
        isMove = false;
    }
}