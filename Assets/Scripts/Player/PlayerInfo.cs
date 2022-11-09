using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using TMPro;

public enum state
{
    None,
    Die,
    Stay,
    Stun,
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
    [field: SerializeField] public float moveSpeed { get; private set; } = 10;
    [field: SerializeField] public float basicMoveSpeed { get; private set; } = 10;
    [field: SerializeField] public float basicAttackSpeed { get; private set; } = 1;
    [field: SerializeField] public float basicAttackDamage { get; private set; } = 10;
    [field: SerializeField] public float damageDecrease { get; private set; } = 0;
    [field: Header("PlayerState")]
    [field: SerializeField] public state playerUnbeatable { get; private set; }
    [field: SerializeField] public state playerIsMove { get; private set; }
    [field: SerializeField] public state playerAlive { get; private set; }

    public delegate void ChangeMoveSpeed();
    public event ChangeMoveSpeed changeMoveSpeed;
    // skill range picture
    public GameObject skilla;
    // skill range
    public RectTransform myskillRangerect = null;
    private Coroutine slowCoroutine;
    private Coroutine stunCoroutine;
    private Coroutine unbeatableCoroutine;
    private string sessionID;

    private void Start()
    {
        // if (photonView.IsMine == true)
        gameObject.tag = "Player";
        // GameMgr.Instance.randomSkill.GetRandomSkill(gameObject);
        if (changeMoveSpeed != null) changeMoveSpeed();
    }
    public void StayPlayer(float time)
    {
        StartCoroutine(Stun(state.Stay,time));
    }

    [PunRPC]
    public void MySessionID(string ID)
    {
        sessionID = ID;
    }

    [PunRPC]
    private void RPC_GetDamage(DamageInfo attackInfo)
    {
        if (playerAlive == state.Die) return;
        if (playerUnbeatable == state.Unbeatable) return;
        if (attackInfo.attackState == state.Stun)
        {
            if (stunCoroutine != null) StopCoroutine(stunCoroutine);
            stunCoroutine = StartCoroutine(Stun(state.Stun,attackInfo.timer));
        }
        else if (attackInfo.attackState == state.Slow)
        {
            if (slowCoroutine != null) StopCoroutine(slowCoroutine);
            slowCoroutine = StartCoroutine(Slow(attackInfo.slowDownRate, attackInfo.timer));
        }
        curHP -= attackInfo.attackDamage * ((100 - damageDecrease) / 100);
        if (curHP <= 0)
        {
            curHP = 0;
            photonView.RPC("RPC_Die", RpcTarget.All, attackInfo.attackerViewID);
        }
    }
    [PunRPC]
    private void RPC_Die(int attackerViewID)
    {
        playerAlive = state.Die;
    }

    [PunRPC]
    private void ChangeHP(float hp)
    {
        curHP += hp;
        if (curHP >= maxHP)
            curHP = maxHP;
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
        StartCoroutine(DamageDecrease(value, time));
    }
    #region playerStateChange
    IEnumerator Slow(float slowDownRate, float time)
    {
        moveSpeed = basicMoveSpeed;
        moveSpeed *= slowDownRate / 100;
        if (changeMoveSpeed != null) changeMoveSpeed();
        yield return new WaitForSeconds(time);
        moveSpeed = basicMoveSpeed;
        yield break;
    }
    IEnumerator Stun(state change,float time)
    {
        playerIsMove = change;
        yield return new WaitForSeconds(time);
        playerIsMove = state.None;
        yield break;
    }
    IEnumerator DamageDecrease(float value, float time)
    {
        damageDecrease += value;
        yield return new WaitForSeconds(time);
        damageDecrease -= value;
        yield break;
    }
    IEnumerator Unbeatable(float time)
    {
        playerUnbeatable = state.Unbeatable;
        yield return new WaitForSeconds(time);
        playerUnbeatable = state.None;
        yield break;
    }
    #endregion
}
