using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;

public class PlayerMove : MonoBehaviourPun
{
    public NavMeshAgent navMeshAgent { get; private set; } = null;
    public GameObject trailEff;
    private PlayerInfo playerInfo;
    private Animator myAnimator;
    private GhostEffect ghostEffect;
    private float ratioX = 0.96588348396132983594601426304165f;
    private float ratioY = 1.0797068659017167862159843860718f;

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
        if (photonView.IsMine == true) playerInfo.onChangeMoveSpeed += myChangeSpeed;
        MoveStop();
    }
    private void myChangeSpeed()
    {
        navMeshAgent.speed = playerInfo.moveSpeed;
        Debug.Log("델리게이트 무브스피드 변경");
    }

    private void Update()
    {
      //Debug.Log("X = " + Input.mousePosition.x);
      //Debug.Log("Y = " + Input.mousePosition.y);
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
            if (Input.mousePosition.x > 1632 && Input.mousePosition.x < 1873 & Input.mousePosition.y > 12 && Input.mousePosition.y < 254)
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
                navMeshAgent.updatePosition = true;
                navMeshAgent.updateRotation = true;
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
        hitPos.x = mousePos.x - 1632.106f;
        hitPos.y = mousePos.y - 12.89964f;
        mask = 1 << LayerMask.NameToLayer("Ground");

        nullCheck = Physics.Raycast(new Vector3(502.4f - hitPos.x * ratioX, 1000, 472.1f - hitPos.y * ratioY), Vector3.down, out hit, 9999, mask);
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
        navMeshAgent.ResetPath();
        navMeshAgent.updatePosition = false;
        navMeshAgent.updateRotation = false;
        navMeshAgent.velocity = Vector3.zero;
        isMove = false;
        desiredDir = Vector3.zero;
    }
    Coroutine ghostEff;
    [PunRPC]
    private void SetGhostEff(float time)
    {
        trailEff.SetActive(true);
        if (ghostEff != null) StopCoroutine(ghostEff);
        ghostEff= StartCoroutine(ghostEffDelady(time));
    }
    IEnumerator ghostEffDelady(float time)
    {    
        yield return new WaitForSeconds(time);
        Debug.Log("트레일 삭제");
        trailEff.SetActive(false);
    }
}