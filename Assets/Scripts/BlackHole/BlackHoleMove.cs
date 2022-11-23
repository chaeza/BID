using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BlackHoleMove : MonoBehaviourPun
{
    [SerializeField] private GameObject[] closeGate = null;

    [SerializeField] private List<GameObject> blackHoleList = null;
    //private float blackHoleTime = 100f;
    private float blackHoleTime = 60f;
    private float masterTime = 0f;
    private int MasterRan;

    [PunRPC]
    public void BlackHolePos(int MasterRan)
    {
        Debug.Log(MasterRan);
        blackHoleList[MasterRan].SetActive(true);
        StartCoroutine(CloseGate(MasterRan));
        blackHoleList.Remove(blackHoleList[MasterRan]);
    }
    private void Update()
    {
        if (GameMgr.Instance.GameState == false) return;
        if (PhotonNetwork.IsMasterClient)
        {
            masterTime += Time.deltaTime;
        }

        if (masterTime >= blackHoleTime&& blackHoleList.Count>0)
        {
            MasterRan = Random.Range(0, blackHoleList.Count);
            photonView.RPC("BlackHolePos", RpcTarget.All, MasterRan);
            masterTime = 0;
            return;
        }
    }

    IEnumerator CloseGate(int MasterRan)
    {
        yield return new WaitForSeconds(11f);
        closeGate[MasterRan].gameObject.SetActive(true);
    }


}
