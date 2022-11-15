using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;

public class PlayerMove : MonoBehaviourPun
{
    public NavMeshAgent navMeshAgent { get; private set; } = null;
    private PlayerInfo playerInfo;
    private Animator myAnimator;
    private GhostEffect ghostEffect;
    private float ratioX = 1.3206611570247933884297520661157f;
    private float ratioY = 1.2966942148760330578512396694215f;

    private RaycastHit hit;
    private Vector3 clickPos = Vector3.one;
    private Vector3 hitPos = Vector3.zero;
    private Vector3 desiredDir;
    private bool isMove = false;
    private bool isClick = false;
    private bool nullCheck;
    private bool nullCheckHit;
    private int mask;
    private int count;

    private void Start()
    {
        ghostEffect = GetComponent<GhostEffect>();
        playerInfo = GetComponent<PlayerInfo>();
        myAnimator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = playerInfo.moveSpeed;
        if(photonView.IsMine==true) playerInfo.onChangeMoveSpeed += myChangeSpeed;
        MoveStop();
    }
    private void myChangeSpeed()
    {
        navMeshAgent.speed = playerInfo.moveSpeed;
        Debug.Log("델리게이트 무브스피드 변경");
    }

    private void Update()
    {
        if (GameMgr.Instance.GameState == false) return;
        if (photonView.IsMine == false) return;
        if (playerInfo.playerAlive == state.Die || playerInfo.playerStun == state.Stun || playerInfo.playerStay == state.Stay)
        {
            if (count == 0)
            {
                count++;
                MoveStop();
            }
            return;
        }
        else if (count != 0) count = 0;
        if (GameMgr.Instance.playerInput.inputKey2 == KeyCode.Mouse1)
        {
            if (Input.mousePosition.x > 1623 && Input.mousePosition.x < 1867 & Input.mousePosition.y > 15 && Input.mousePosition.y < 260)
            {
                clickPos = Input.mousePosition;
                MoveMiniMap(clickPos);
                isClick = true;
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
       /* Ray ray = Camera.main.ScreenPointToRay(mousePos);
        Debug.DrawRay(ray.origin, ray.direction * 200f, Color.red, 999f);*/
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
        hitPos.x = mousePos.x - 1623.024f;
        hitPos.y = mousePos.y - 15.24192f;
        mask = 1 << LayerMask.NameToLayer("Ground");

        nullCheck = Physics.Raycast(new Vector3(547.5f - hitPos.x * ratioX, 1000, 508.59f - hitPos.y * ratioY), Vector3.down, out hit, 9999, mask);
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
        desiredDir = Vector3.zero;
    }
    [PunRPC]
    private void SetGhostEff(int num,float time)
    {
        StartCoroutine(ghostEffDelady(num, time));
    }
    IEnumerator ghostEffDelady(int num,float time)
    {
        for(int i = 0; i<num; i++)
        {
            yield return new WaitForSeconds(time);
            ghostEffect.CreateGhostEffectObject(Color.black, 1f, 0.7f, 0.5f, 0.85f, 0.5f);
            yield return null;
        }
        yield break;
    }
}