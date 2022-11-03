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

    private RaycastHit hit;
    private Vector3 clickPos = Vector3.one;
    private Vector3 desiredDir;
    private bool isMove = false;
    private bool nullCheck;
    private bool nullCheckHit;
    private int mask;

    private void Start()
    {
        playerInfo = GetComponent<PlayerInfo>();
        myAnimator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = playerInfo.moveSpeed;
        MoveStop();
    }
    public void ChageSpeed(float speed)
    {
        navMeshAgent.speed = speed;
    }

    private void Update()
    {
        //if (photonView.IsMine == false) return;
        if (GameMgr.Instance.playerInput.inputKey2 == KeyCode.Mouse1)
        {
            clickPos = Input.mousePosition;
            Move(clickPos);
        }

        if (GameMgr.Instance.playerInput.inputKey == KeyCode.S)
        {
            MoveStop();
        }

        if (isMove == true)
        {
            if (Mathf.Abs(desiredDir.z - transform.position.z) < 0.5f) desiredDir.y = transform.position.y;
            else if (Mathf.Abs(desiredDir.x - transform.position.x) < 0.5f) desiredDir.y = transform.position.y;
            if (Vector3.Distance(desiredDir, transform.position) > 0.5f)
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
        Debug.Log("MOve");
        mask = 1 << LayerMask.NameToLayer("Ground");


        //Ray ray = Camera.main.ScreenPointToRay(mousePos);
        nullCheck = Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out hit, 9999, mask);

        //Debug.DrawRay(ray.origin, ray.direction * 200f, Color.red, 999f);


        nullCheckHit = (nullCheck) ? hit.transform.gameObject.CompareTag("Ground") : false;
        if (nullCheckHit == true)
        {
            desiredDir = hit.point;
            isMove = true;
        }
    }

    public void MoveStop()
    {
        myAnimator.SetBool("isMove", false);
        Debug.Log("stop");
        navMeshAgent.isStopped = true;
        navMeshAgent.velocity = Vector3.zero;
        navMeshAgent.updateRotation = false;
        navMeshAgent.updatePosition = false;
        isMove = false;
    }
}