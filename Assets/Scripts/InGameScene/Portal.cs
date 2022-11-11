using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public delegate void CameraReSetting();

public struct PortalData
{
    public bool isCenter;
    public bool isDestoryed;
    public bool isRandomPotal;

    public int num;
    public int potalTotalNum;
}

public partial class Portal : MonoBehaviourPun
{
    public Portal[] exit;
    public PortalData portalData;
}

//portal move part
public partial class Portal : MonoBehaviourPun
{
    //Delay 
    [SerializeField]
    private float transferTimer = 0;
    
    public CameraReSetting cameraReSetting;

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
        yield return new WaitForSeconds(time);
        GameObject player;
        if (launchPlayerList.Count > 0)
        {
            player = launchPlayerList.Dequeue();
            // Debug.Log(player.GetPhotonView().ViewID + "????d");
            player.GetComponent<PlayerMove>().navMeshAgent.enabled = false;
            if (portalData.isCenter == false)
            {
                player.GetComponent<PlayerMove>().navMeshAgent.transform.position = exit[0].gameObject.transform.position + Vector3.forward * 2;
                cameraReSetting();
            }
            else if (portalData.isCenter == true)
            {
                int ran = 0;
                while (true)
                {
                    ran = Random.Range(0, portalData.potalTotalNum);
                    if (exit[ran].portalData.isDestoryed == false)
                    {
                        player.GetComponent<PlayerMove>().navMeshAgent.transform.position = exit[ran].gameObject.transform.position + Vector3.forward * 2;
                        cameraReSetting();
                        break;
                    }
                }
            }
            player.GetComponent<PlayerMove>().navMeshAgent.enabled = true;
            player.GetComponent<PlayerMove>().MoveStop();
            viewIDList.Remove(player.gameObject.GetPhotonView().ViewID);
            playerList.Remove(player.gameObject);
        }
    }
}