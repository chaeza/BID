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
    private float ratioX = 1.3103305785123966942148760330579f;
    private float ratioY = 1.2954545454545454545454545454545f;

    //test indicator
    private CodeExample codeExample;

    private RaycastHit hit;
    //private Vector2 mousePos = Vector2.zero;
    private Vector3 clickPos = Vector3.one;
    private Vector3 desiredDir;
    private bool isMove = false;
    private bool isClick = false;
    private bool nullCheck;
    private bool nullCheckHit;
    private int mask;

    private void Start()
    {
        playerInfo = GetComponent<PlayerInfo>();
        myAnimator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        codeExample = GameObject.FindObjectOfType<CodeExample>();
        navMeshAgent.speed = playerInfo.moveSpeed;
        MoveStop();
    }
    public void ChageSpeed(float speed)
    {
        navMeshAgent.speed = speed;
    }

    private void Update()
    {
        //test indicator
        if (Input.GetKeyDown(KeyCode.Z))
        {
            codeExample.Cone();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            codeExample.Line();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            codeExample.Area();
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            codeExample.Radius();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            codeExample.Cast();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            codeExample.Interrupt();
        }

        //if (photonView.IsMine == false) return;
        if (GameMgr.Instance.playerInput.inputKey2 == KeyCode.Mouse1)
        {
            if (Input.mousePosition.x > 1643 && Input.mousePosition.x < 1883 & Input.mousePosition.y > 11 && Input.mousePosition.y < 252)
            {
                clickPos = Input.mousePosition;
                MoveMiniMap(clickPos);
            }
            else
            {
                clickPos = Input.mousePosition;
                Move(clickPos);
                isClick = true;
            }
        }

        if (GameMgr.Instance.playerInput.inputKey == KeyCode.S)
        {
            MoveStop();
        }

        if (isMove == true && isClick == true)
        {
            if (Vector3.Distance(desiredDir, transform.position) > 0.5f)
            {
                myAnimator.SetBool("isMove", true);
                navMeshAgent.isStopped = false;
                navMeshAgent.updateRotation = true;
                navMeshAgent.updatePosition = true;
                isClick = false;
                navMeshAgent.SetDestination(desiredDir);
            }
        }
        if (Mathf.Abs(desiredDir.z - transform.position.z) < 0.5f) desiredDir.y = transform.position.y;
        else if (Mathf.Abs(desiredDir.x - transform.position.x) < 0.5f) desiredDir.y = transform.position.y;
        if (Vector3.Distance(desiredDir, transform.position) < 0.5f) MoveStop();



    }
    public void Move(Vector3 mousePos)
    {
        //Ray ray = Camera.main.ScreenPointToRay(mousePos);
        // Debug.DrawRay(ray.origin, ray.direction * 200f, Color.red, 999f);
        mask = 1 << LayerMask.NameToLayer("Ground");
        nullCheck = Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out hit, 9999, mask);
        nullCheckHit = (nullCheck) ? hit.transform.gameObject.CompareTag("Ground") : false;
        if (nullCheckHit == true)
        {
            desiredDir = hit.point;
            isMove = true;
        }
    }

    public void MoveMiniMap(Vector3 mousePos)
    {
        mousePos.x = Input.mousePosition.x - 1642.384f;
        mousePos.y = Input.mousePosition.y - 11.25826f;
        mask = 1 << LayerMask.NameToLayer("Ground");

        nullCheck = Physics.Raycast(new Vector3(546.6f - mousePos.x * ratioX, 1000, 502.3f - mousePos.y * ratioY), Vector3.down, out hit, 9999, mask);
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