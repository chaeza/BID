using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public struct PotalData
{
    public bool isCenter;
    public bool isRandomPotal;
    public int potalTotalNum;
    public int num;
}

public class Potal : MonoBehaviourPun
{
    public Potal[] exit;
    public PotalData potalData;




    //Delay 
    [SerializeField]
    private float transferTimer = 0;


    //Priority Queue for Transfer Player
    List<int> viewIDList = new List<int>();
    List<GameObject> playerList = new List<GameObject>();
    Queue<GameObject> launchPlayerList = new Queue<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player(Clone)")
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
            // Debug.Log(player.GetPhotonView().ViewID + "????d");
            player.GetComponent<PlayerMove>().navMeshAgent.updatePosition = true;

            if (potalData.isCenter == false) player.transform.position = exit[0].gameObject.transform.position;

            else if (potalData.isCenter == true) player.transform.position = exit[Random.Range(0, potalData.potalTotalNum)].gameObject.transform.position;

            viewIDList.Remove(player.gameObject.GetPhotonView().ViewID);
            playerList.Remove(player.gameObject);
        }
    }
}