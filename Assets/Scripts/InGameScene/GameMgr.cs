using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public partial class GameMgr : Singleton<GameMgr>
{
    private void Awake()
    {
        gameSceneLogic = gameObject.AddComponent<GameSceneLogic>();
        playerInput = gameObject.AddComponent<PlayerInput>();
        randomSkill = gameObject.AddComponent<RandomSkill>();
        randomItem = gameObject.AddComponent<RandomItem>();
        inventory = gameObject.AddComponent<Inventory>();
        codeExample = FindObjectOfType<CodeExample>();
        followCam = FindObjectOfType<FollowCam>();
        uIMgr = FindObjectOfType<UIMgr>();
    }

    // Skill Destroy
    public GameObject PunFindObject(int viewID)
    {
        GameObject find = null;
        PhotonView[] viewObject = FindObjectsOfType<PhotonView>();
        for (int i = 0; i < viewObject.Length; i++)
        {
            if (viewObject[i].ViewID == viewID) find = viewObject[i].gameObject;
        }
        return find;
    }
    public void DestroyTarget(GameObject desObject, float time)
    {
        photonView.RPC("PunDestroyObject", RpcTarget.All, desObject.GetPhotonView().ViewID, time);
    }
    [PunRPC]
    public void PunDestroyObject(int viewID, float time)
    {
        Destroy(PunFindObject(viewID), time);
    }
}
