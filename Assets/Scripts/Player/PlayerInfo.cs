using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using TMPro;
using RPG_Indicator;

public delegate void HPTransfer(float Hp);
public enum state
{
    None,
    Die,
    Stay,
    Stun,
    Silence,
    Unbeatable,//¹«Àû
    Slow,
    End
}
public struct DamageInfo
{
    public state attackState;
    public float attackDamage;
    public float slowDownRate;
    public float timer;
    public int attackerViewID;
}
public class PlayerInfo : MonoBehaviourPun
{
    [field: Header("PlayerInfo")]
    [field: SerializeField] public float maxHP { get; private set; } = 100;
    [field: SerializeField] public float curHP { get; private set; } = 100;
    [field: SerializeField] public float moveSpeed { get; private set; } = 7;
    [field: SerializeField] public float basicMoveSpeed { get; private set; } = 7;
    [field: SerializeField] public float basicAttackSpeed { get; private set; } = 1;
    [field: SerializeField] public float basicAttackDamage { get; private set; } = 10f;
    [field: SerializeField] public float damageDecrpease { get; private set; } = 0;
    [field: Header("PlayerState")]
    [field: SerializeField] public state playerUnbeatable { get; private set; }
    [field: SerializeField] public state playerStun { get; private set; }
    [field: SerializeField] public state playerSlow { get; private set; }
    [field: SerializeField] public state playerStay { get; private set; }
    [field: SerializeField] public state playerAlive { get; private set; }
    [field: SerializeField] public state playerSilence { get; private set; }
    [SerializeField] GameObject stunEff;
    public delegate void OnChangeMoveSpeed();
    public delegate void OnGetDamage();
    public event OnChangeMoveSpeed onChangeMoveSpeed;
    public event OnGetDamage onGetDamage;
    public HPTransfer HPTransfer;
    private Animator myAnimator;
    private Coroutine slowCoroutine;
    private Coroutine stunCoroutine;
    private Coroutine unbeatableCoroutine;
    private string sessionID;


    private void Start()
    {
        if (photonView.IsMine == true)
        {
            gameObject.tag = "MainPlayer";
            GameMgr.Instance.randomSkill.GetRandomSkill(gameObject);
            myAnimator = GetComponent<Animator>();
        }
        if (onChangeMoveSpeed != null) onChangeMoveSpeed();
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.I)) GameMgr.Instance.gameSceneLogic.AliveNumCheck();
    }
    public void StayPlayer(float time)
    {
        StartCoroutine(Stay(time));
    }
    public void SetBasicAttackDamage(float value)
    {
        basicAttackDamage += value;
    }


    [PunRPC]
    public void MySessionID(string ID)
    {
        sessionID = ID;
    }

    [PunRPC]
    private void RPC_GetDamage(state attackState,float attackDamage, float slowDownRate, float timer,int attackerViewID)
    {
        if (playerAlive == state.Die) return;
        if (playerUnbeatable == state.Unbeatable) return;
        if (attackState == state.Stun)
        {
            if (stunCoroutine != null) StopCoroutine(stunCoroutine);
            stunCoroutine = StartCoroutine(RPC_GetDamage_Stun(timer));
        }
        else if (attackState == state.Slow)
        {
            if (slowCoroutine != null) StopCoroutine(slowCoroutine);
            slowCoroutine = StartCoroutine(Slow(slowDownRate, timer));
        }
        curHP -= attackDamage * ((100 - damageDecrpease) / 100);
        if (onGetDamage != null) onGetDamage();
        if (curHP <= 0)
        {
            curHP = 0;
            gameObject.GetPhotonView().RPC("RPC_Die", RpcTarget.All, attackerViewID);
        }
        HPTransfer(curHP);
    }
    [PunRPC]
    private void RPC_Die(int attackerViewID2)
    {
        playerAlive = state.Die;
        if(photonView.IsMine) myAnimator.SetTrigger("isDie");
       // GameMgr.Instance.gameSceneLogic.AliveNumCheck();
    }

    [PunRPC]
    private void ChangeHP(float hp)
    {
        curHP += hp;
        if (curHP >= maxHP)
            curHP = maxHP;
    }
    [PunRPC]
    private void SetChangeMoveSpeed(float value,float time)
    {
        if (slowCoroutine != null) StopCoroutine(slowCoroutine);
        slowCoroutine=StartCoroutine(Slow(value, time));
    }
    public void ChangeMoveSpeed(float value)
    {
        moveSpeed = value;
        if (onChangeMoveSpeed != null) onChangeMoveSpeed();
    }
    [PunRPC]
    private void SetUnbeatable(float time)
    {
        if (unbeatableCoroutine != null) StopCoroutine(unbeatableCoroutine);
        unbeatableCoroutine = StartCoroutine(Unbeatable(time));

    }
    [PunRPC]
    private void SetDamageDecrpease(float value, float time)
    {
        StartCoroutine(DamageDecrpease(value, time));
    }
    #region playerStateChange
    IEnumerator Slow(float slowDownRate, float time)
    {
        float Sp = basicMoveSpeed;
        Sp *= slowDownRate / 100;
        ChangeMoveSpeed(Sp);
        yield return new WaitForSeconds(time);
        ChangeMoveSpeed(basicMoveSpeed);
        yield break;
    }
    IEnumerator Stay(float time)
    {
        playerStay = state.Stay;
        yield return new WaitForSeconds(time);
        if(playerStay==state.Stay) playerStay = state.None;
        yield break;
    }
    IEnumerator RPC_GetDamage_Stun(float time)
    {
        stunEff.SetActive(true);
        playerStun = state.Stun;
        yield return new WaitForSeconds(time);
        if (playerStun == state.Stun) playerStun = state.None;
        stunEff.SetActive(false);
        yield break;
    }
    IEnumerator DamageDecrpease(float value, float time)
    {
        damageDecrpease += value;
        yield return new WaitForSeconds(time);
        damageDecrpease -= value;
        yield break;
    }
    IEnumerator Unbeatable(float time)
    {
        if(moveSpeed<basicMoveSpeed) ChangeMoveSpeed(basicMoveSpeed);
        playerSilence = state.None;
        playerStun = state.None;
        stunEff.SetActive(false);
        playerUnbeatable = state.Unbeatable;
        yield return new WaitForSeconds(time);
        playerUnbeatable = state.None;
        yield break;
    }
    #endregion
}
