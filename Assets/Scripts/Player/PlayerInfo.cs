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
    Stun,
    Unbeatable,//¹«Àû
    Slow,
    End
}

public class PlayerInfo : MonoBehaviourPun
{
    [field: SerializeField] public float maxHP { get; private set; } = 100;
    [field: SerializeField] public float curHP { get; private set; } = 100;
    [field: SerializeField] public float moveSpeed { get; private set; } = 10;
    [field: SerializeField] public float basicAttackSpeed { get; private set; } = 1;
    [field: SerializeField] public float basicAttackDamage { get; private set; } = 10;
    [field: SerializeField] public float damageDecrease { get; private set; } = 0;
}
