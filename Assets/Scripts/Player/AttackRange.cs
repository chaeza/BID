using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AttackRange : MonoBehaviourPun
{
    [PunRPC]
    public void AttackRangeScale(Vector3 Pos)
    {
        gameObject.transform.localScale = Pos;
    }
}
