using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public delegate void Del_DestroyTarget(GameObject obj, float time);
public delegate GameObject Del_PunFindObject(int viewID);
public partial class GameMgr : Singleton<GameMgr>
{
    public Del_DestroyTarget del_DestroyTarget;
    public Del_PunFindObject del_PunFindObject;
    public bool GameState = false;


    private void Awake()
    {
        GameSceneSettingInitializing();
        DontDestroyOnLoad(gameObject);
    }
    public void RoomInfoSetting()
    {
        //기존 포스트맨 역할
    }

    public void GameSceneSetting(GameObject gameScenManager)
    {
        gameSceneLogic = FindObjectOfType<GameSceneLogic>();
        playerInput = gameObject.AddComponent<PlayerInput>();
        randomSkill = gameObject.AddComponent<RandomSkill>();
        randomItem = gameObject.AddComponent<RandomItem>();
        inventory = gameObject.AddComponent<Inventory>();
        codeExample = FindObjectOfType<CodeExample>();
        followCam = FindObjectOfType<FollowCam>();
        uIMgr = FindObjectOfType<UIMgr>();
        itemSpawner = FindObjectOfType<ItemSpawner>();
        potalSystem = FindObjectOfType<PotalSystem>();
    }

    public void GameSceneSettingInitializing()
    {
        gameSceneLogic = null;
        playerInput = null;
        randomSkill = null;
        randomItem = null;
        inventory = null;
        codeExample = null;
        followCam = null;
        uIMgr = null;
        itemSpawner = null;
        potalSystem = null;

        if (gameObject.GetComponent<PhotonView>() != null)
            Destroy(gameObject.GetComponent<PhotonView>());
    }


    public void DestroyTarget(GameObject desObject, float time)
    {
        if (desObject != null) del_DestroyTarget(desObject, time);//gameObject.GetPhotonView().RPC("PunDestroyObject", RpcTarget.All, desObject.GetPhotonView().ViewID, time);
        else Debug.Log("null");
    }

    public GameObject PunFindObject(int viewID)
    {
        return del_PunFindObject(viewID);
    }

}
