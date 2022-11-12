using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;

public class Projectile_Bash_Hit : MonoBehaviourPun
{
    private Vector3 desiredDir;
    private bool dashAttack = false;
    NavMeshAgent navMeshAgent;
   
    private void OnTriggerEnter(Collider other)
    {
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        navMeshAgent.speed = 40f;
        dashAttack = true;
        if (other.tag == "Player")
        {
            // shield effect
            GameObject a = PhotonNetwork.Instantiate("WarofWall", transform.position, Quaternion.identity);

            dashAttack = false;
            navMeshAgent.speed = 5f;

            GameMgr.Instance.DestroyTarget(a, 6.2f);
            GameMgr.Instance.DestroyTarget(gameObject, 6.2f);
        }

    }

    private void Update()
    {
        if (dashAttack == true)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.updateRotation = true;
            navMeshAgent.updatePosition = true;

            navMeshAgent.SetDestination(desiredDir);
        }
        StartCoroutine(Fire());
    }

    IEnumerator Fire()  //큐브 이동시키기
    {
        for (int i = 0; i < 20; i++)
        {
            this.transform.Translate(0, 0, -0.5f);
            yield return null;
        }
        yield break;
    }
}
