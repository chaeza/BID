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
public enum ChangeableInfo
{
    maxHP,
    curHP,
    moveSpeed,
    basicMoveSpeed,
    basicAttackSpeed,
    basicAttackDamage,
    damageDecrease,
    basicAttackRange,
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
    [field: SerializeField] public float damageDecrease { get; private set; } = 0;
    [field: SerializeField] public float basicAttackRange { get; private set; } = 1;
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
    private int miniMapViewID;
    private Animator myAnimator;
    private Coroutine slowCoroutine;
    private Coroutine stunCoroutine;
    private Coroutine silenceCoroutine;
    private Coroutine unbeatableCoroutine;


    [SerializeField]
    private string sessionID;
    [SerializeField] private int myPlayerNum;


    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GameMgr.Instance.gameSceneLogic.gameObject.GetPhotonView().RPC("RPC_All_SessionID", RpcTarget.All, sessionID);
        }

        if (photonView.IsMine == true)
        {
            gameObject.tag = "MainPlayer";
            GameMgr.Instance.randomSkill.GetRandomSkill(gameObject);
            gameObject.AddComponent<Dash>();
            myAnimator = GetComponent<Animator>();
        }
        if (onChangeMoveSpeed != null) onChangeMoveSpeed();
    }

    public void StayPlayer(float time)
    {
        StartCoroutine(Stay(time));
    }
    public void PlayInfoChange(ChangeableInfo info, float infoValue)
    {
        if (info == ChangeableInfo.basicAttackDamage)
        {
            basicAttackDamage += infoValue;
            GameMgr.Instance.uIMgr.PlayInfoChange(0, basicAttackDamage);
        }
        else if (info == ChangeableInfo.basicAttackRange)
        {
            basicAttackRange += infoValue;
            GameMgr.Instance.uIMgr.PlayInfoChange(1, basicAttackRange);
        }
        else if (info == ChangeableInfo.basicAttackSpeed)
        {
            basicAttackSpeed += infoValue;
            GameMgr.Instance.uIMgr.PlayInfoChange(2, basicAttackSpeed);
        }
        else if (info == ChangeableInfo.moveSpeed)
        {
            moveSpeed += infoValue;
            ChangeMoveSpeed(moveSpeed);
        }
        else if (info == ChangeableInfo.damageDecrease)
        {
            damageDecrease += infoValue;
            GameMgr.Instance.uIMgr.PlayInfoChange(4, damageDecrease);
        }
        else if (info == ChangeableInfo.basicMoveSpeed)
        {
            basicMoveSpeed += infoValue;
            ChangeMoveSpeed(basicMoveSpeed);
        }
    }


    [PunRPC]
    public void MySessionID(string ID, int playerNum)
    {
        sessionID = ID;
        myPlayerNum = playerNum;
        GameMgr.Instance.uIMgr.TabUpDate(myPlayerNum, state.None);
    }

    [PunRPC]
    private void RPC_GetDamage(state attackState, float attackDamage, float slowDownRate, float timer, int attackerViewID)
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
            if (photonView.IsMine == true)
            {
                if (slowCoroutine != null) StopCoroutine(slowCoroutine);
                slowCoroutine = StartCoroutine(Slow(slowDownRate, timer));
            }
        }
        else if (attackState == state.Silence)
        {
            if (photonView.IsMine)
            {
                if (silenceCoroutine != null) StopCoroutine(silenceCoroutine);
                silenceCoroutine = StartCoroutine(Silence(timer));
            }
        }
        if (damageDecrease < 100)
            curHP -= attackDamage * ((100 - damageDecrease) / 100);

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
        curHP = 0;
        HPTransfer(curHP);
        if (photonView.IsMine) myAnimator.SetTrigger("isDie");
        GameMgr.Instance.gameSceneLogic.AliveNumCheck();
        GameMgr.Instance.uIMgr.TabUpDate(myPlayerNum, state.Die);
        if (GameMgr.Instance.PunFindObject(miniMapViewID) != null)
            GameMgr.Instance.PunFindObject(miniMapViewID).SetActive(false);
        if (gameObject.GetPhotonView().ViewID == attackerViewID2)
        {
            GameMgr.Instance.uIMgr.TabUpDate(myPlayerNum, state.Die);
            return;
        }
        else
        {
            // if (photonView.IsMine == true)
            //     GameMgr.Instance.gameSceneLogic.gameObject.GetPhotonView().RPC("KillCount", RpcTarget.All, GameMgr.Instance.PunFindObject(attackerViewID2).GetComponent<PlayerInfo>().myPlayerNum);

            GameMgr.Instance.uIMgr.KillUpDate(GameMgr.Instance.PunFindObject(attackerViewID2).GetComponent<PlayerInfo>().myPlayerNum);
            GameMgr.Instance.uIMgr.TabUpDate(myPlayerNum, state.Die);
        }
    }
    [PunRPC]
    public void MyMiniMapRender(int viewID)
    {
        miniMapViewID = viewID;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ItemBlackHole")
        {
            gameObject.GetPhotonView().RPC("RPC_Die", RpcTarget.All, gameObject.GetPhotonView().ViewID);
        }
    }

    public void SetChangeMoveSpeed(float value, float time)
    {
        if (slowCoroutine != null) StopCoroutine(slowCoroutine);
        slowCoroutine = StartCoroutine(Slow(value, time));
    }
    public void ChangeMoveSpeed(float value)
    {
        moveSpeed = value;
        GameMgr.Instance.uIMgr.PlayInfoChange(3, moveSpeed);
        if (onChangeMoveSpeed != null) onChangeMoveSpeed();
    }

    [PunRPC]
    private void ChangeHP(float hp)
    {
        curHP += hp;
        if (curHP >= maxHP)
            curHP = maxHP;
        HPTransfer(curHP);
    }
    [PunRPC]
    private void SetUnbeatable(float time)
    {
        if (unbeatableCoroutine != null) StopCoroutine(unbeatableCoroutine);
        unbeatableCoroutine = StartCoroutine(Unbeatable(time));

    }
    public void SetDamageDecrease(float value, float time)
    {
        StartCoroutine(DamageDecrease(value, time));
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
        if (playerStay == state.Stay) playerStay = state.None;
        yield break;
    }
    IEnumerator Silence(float time)
    {
        GameMgr.Instance.uIMgr.SetSilence(true);
        playerSilence = state.Silence;
        yield return new WaitForSeconds(time);
        if (playerSilence == state.Silence)
        {
            playerSilence = state.None;
            GameMgr.Instance.uIMgr.SetSilence(false);

        }
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
    IEnumerator DamageDecrease(float value, float time)
    {
        PlayInfoChange(ChangeableInfo.damageDecrease, value);
        yield return new WaitForSeconds(time);
        PlayInfoChange(ChangeableInfo.damageDecrease, -value);
        yield break;
    }
    IEnumerator Unbeatable(float time)
    {
        if (moveSpeed < basicMoveSpeed) ChangeMoveSpeed(basicMoveSpeed);
        playerSilence = state.None;
        GameMgr.Instance.uIMgr.SetSilence(false);
        playerStun = state.None;
        stunEff.SetActive(false);
        playerUnbeatable = state.Unbeatable;
        yield return new WaitForSeconds(time);
        playerUnbeatable = state.None;
        yield break;
    }
    #endregion
}
