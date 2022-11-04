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
    private GhostEffect ghostEffect;


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
        ghostEffect = GetComponent<GhostEffect>();  
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
    bool ghostCheck = false;
    IEnumerator ghostEffDelady(float time)
    {
        if (ghostCheck == true) yield break;
        else ghostCheck = true;
        while (isMove ==true)
        {
            yield return new WaitForSeconds(time);
            ghostEffect.CreateGhostEffectObject(Color.white, 0f, 0.1f, 0.7f, 0.85f, 0.5f);
        }
        ghostCheck = false;
    }

    public void Move(Vector3 mousePos)
    {
        mask = 1 << LayerMask.NameToLayer("Ground");
        
        nullCheck = Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out hit, 9999, mask);

        nullCheckHit = (nullCheck) ? hit.transform.gameObject.CompareTag("Ground") : false;
        if (nullCheckHit == true)
        {
            desiredDir = hit.point;
            isMove = true;
           // StartCoroutine(ghostEffDelady(0.2f));
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