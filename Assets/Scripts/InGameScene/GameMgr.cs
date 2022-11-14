using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public partial class GameMgr : Singleton<GameMgr>
{
    private void Awake()
    {
        playerInput = gameObject.AddComponent<PlayerInput>();
        randomSkill = gameObject.AddComponent<RandomSkill>();
        randomItem = gameObject.AddComponent<RandomItem>();
        inventory = gameObject.AddComponent<Inventory>();
        codeExample = FindObjectOfType<CodeExample>();
        followCam = FindObjectOfType<FollowCam>();
        uIMgr = FindObjectOfType<UIMgr>();
        itemSpawner = FindObjectOfType<ItemSpawner>();
        resourceData = Resources.Load<ResourceData>("ResourceData");
        potalSystem = FindObjectOfType<PotalSystem>();
        gameSceneLogic = gameObject.AddComponent<GameSceneLogic>();
    }

    public GameObject PunFindObject(int viewID3)//����̵� �Ѱܹ޾� ������� ������Ʈ�� ã�´�.
    {
        GameObject find = null;
        PhotonView[] viewObject = FindObjectsOfType<PhotonView>();
        for (int i = 0; i < viewObject.Length; i++)
        {
            if (viewObject[i].ViewID == viewID3) find = viewObject[i].gameObject;
        }
        if (find != null) return find;
        else return null;
    }
    public void DestroyTarget(GameObject desObject, float time)
    {
        if (desObject != null) photonView.RPC("PunDestroyObject", RpcTarget.All, desObject.GetPhotonView().ViewID, time);
        else Debug.Log("null");
    }
    [PunRPC]
    public void PunDestroyObject(int viewid, float time)
    {
        Destroy(PunFindObject(viewid), time);
    }
}
