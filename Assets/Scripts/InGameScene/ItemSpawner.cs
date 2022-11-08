using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ItemSpawner : MonoBehaviourPun
{
    //그라운드 태그 모을 배열
    [Header("아이템 위치 좌표")]
    [SerializeField] private GameObject[] itemBigAreaPos = null;
    [Header("스페셜 위치 좌표")]
    [SerializeField] private GameObject itemSpecialAreaPos = null;
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
        RandomBigAreaItemSpawn();
    }

    public void RandomBigAreaItemSpawn()
    {
        //빈오브젝트 만큼 포문돌림
        for(int i = 0; i < itemBigAreaPos.Length; i++)
        {
            int itemCount = Random.Range(1, 4);
            for(int j = 0; j < itemCount; j++) 
            {
                int posx = Random.Range(-20, 21) % 2;
                int posz = Random.Range(-20, 21) % 2;
                Vector3 vec = new Vector3(posx, 0f, posz);

                //int posx = Random.Range((int)itemBigAreaPos[i].transform.position.x - 10, (int)itemBigAreaPos[i].transform.position.x + 10);
                //int posz = Random.Range((int)itemBigAreaPos[i].transform.position.z - 10, (int)itemBigAreaPos[i].transform.position.z + 10); 
                Instantiate(itemPrefab, itemBigAreaPos[i].transform.position + vec, Quaternion.identity);

            }
        }

        for(int i = 0; i < 4; i++)
        {
            int posx = Random.Range((int)itemSpecialAreaPos.transform.position.x - 12, (int)itemSpecialAreaPos.transform.position.x + 12);
            int posz = Random.Range((int)itemSpecialAreaPos.transform.position.z - 12, (int)itemSpecialAreaPos.transform.position.z + 12);
            Instantiate(itemPrefab, new Vector3(posx, itemSpecialAreaPos.transform.position.y, posz), Quaternion.identity);
        }
    }

    //기본 아이템 셋팅
    [PunRPC]
    [System.Obsolete]
    public void ItemInit()
    {
       for(int i =0; i < itemCount; i++)
        {
            ran = Random.Range(0, itemBigAreaPos.Length);
            randomPos = itemBigAreaPos[ran].transform.position;
            while (IsbeingItem[i] == true)
            {
                ran = Random.Range(0, itemBigAreaPos.Length);
            }
            IsbeingItem[ran] = true;
            GameObject item = PhotonNetwork.InstantiateSceneObject("ItemBox", randomPos, Quaternion.identity);
        }
    }

    

    
}
