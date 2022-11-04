using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ItemSpawner : MonoBehaviourPun
{
    //그라운드 태그 모을 배열
    [Header("아이템 위치 좌표")]
    private GameObject[] itemPos = null;
    //아이템 프리팹
    [Header("아이템 프리팹")]
    [SerializeField] private GameObject itemPrefab;
    //아이템 개수
    [Header("아이템 개수")]
    [SerializeField] public int itemCount = 0;
    //해당 그라운드에 아이템이 놓였는지 판단
    private bool[] IsbeingItem = new bool[500];
    private Vector3 randomPos;
    int ran;
    //아이템 풀을 담을 큐
    Queue<GameObject> itemQueue = new Queue<GameObject>();

    private void Awake()
    {

    }

    //기본 아이템 셋팅
    [PunRPC]
    [System.Obsolete]
    public void ItemInit()
    {
       for(int i =0; i < itemCount; i++)
        {
            ran = Random.Range(0, itemPos.Length);
            randomPos = itemPos[ran].transform.position;
            while (IsbeingItem[i] == true)
            {
                ran = Random.Range(0, itemPos.Length);
            }
            IsbeingItem[ran] = true;
            GameObject item = PhotonNetwork.InstantiateSceneObject("ItemBox", randomPos, Quaternion.identity);
        }
    }


    
}
