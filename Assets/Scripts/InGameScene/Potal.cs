using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Potal : MonoBehaviourPun
{
    //Delay 
    [SerializeField]
    private float transferTimer = 0;


    //conneted Num
    private int myPotalNum = 0;
    private int pairPotalNum = 0;

    //Priority Queue for Transfer Player
    List<int> viewIDList = new List<int>();
    List<GameObject> playerList = new List<GameObject>();
    Queue<GameObject> launchPlayerList = new Queue<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "PlayerPrefab(Clone)")
        {
            //Priority Queue Enqueue
            playerList.Add(other.gameObject);
            viewIDList.Add(other.gameObject.GetPhotonView().ViewID);
            launchPlayerList.Enqueue(other.gameObject);

            StartCoroutine(ReadyToTransfer(transferTimer));

        }
        else
            Debug.Log("It is Not Player");
    }

    private void OnTriggerExit(Collider other)
    {
        if (viewIDList.Contains(other.gameObject.GetPhotonView().ViewID))
        {
            viewIDList.Remove(other.gameObject.GetPhotonView().ViewID);
            playerList.Remove(other.gameObject);
            launchPlayerList.Clear();
            Debug.Log(" 지나감");
            for (int i = 0; i < viewIDList.Count; i++)
            {
                launchPlayerList.Enqueue(playerList[i]);
                Debug.Log(i + " 번");
            }
        }
    }

    //When player come into the Potal, He or She have to wait for a time start to transfer player's Position
    IEnumerator ReadyToTransfer(float time)
    {
        GameObject player = new GameObject();
        yield return new WaitForSeconds(time);
        if (launchPlayerList.Count > 0)
        {
            player = launchPlayerList.Dequeue();
            Debug.Log(player.GetPhotonView().ViewID + "????d");
            player.GetComponent<PlayerMove>().navMeshAgent.updatePosition = true;
            player.transform.position = player.transform.position + new Vector3(0, 0, 10);
            //  player.GetComponent<Rigidbody>().position = player.GetComponent<Rigidbody>().position + new Vector3(0, 0, 10);

            viewIDList.Remove(player.gameObject.GetPhotonView().ViewID);
            playerList.Remove(player.gameObject);
        }
        // player.GetComponent<PlayerMove>().MoveStop();

    }
}
