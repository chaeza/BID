using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ItemSpawner : MonoBehaviourPun
{
    [Header("아이템 위치 좌표")]
    [SerializeField] private GameObject[] itemBigAreaPos = null;
    [Header("스페셜 위치 좌표")]
    [SerializeField] private GameObject itemSpecialAreaPos = null;
    [Header("다리 위치 좌표")]
    [SerializeField] private GameObject[] itemBridgeAreadPos = null;
    //아이템 프리팹
    [Header("아이템 프리팹")]
    [SerializeField] private GameObject itemPrefab;
    //아이템 개수
    [Header("아이템 개수")]
    [SerializeField] private int itemMaxCount = 0;

    //아이템 풀을 담을 큐
    Queue<GameObject> itemQueue = new Queue<GameObject>();

    //아이템
    private int itemCount = 0;


    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
            RandomBigAreaItemSpawn();
    }
    public void RandomBigAreaItemSpawn()
    {
        //스페셜 지역은 항상 4개의 아이템이 나온다
        for (int i = 0; i < 4; i++)
        {
            Debug.Log("for문");
            int posx = Random.Range(-(int)itemSpecialAreaPos.transform.localScale.x / 2, (int)itemSpecialAreaPos.transform.localScale.x / 2);
            int posz = Random.Range(-(int)itemSpecialAreaPos.transform.localScale.z / 2, (int)itemSpecialAreaPos.transform.localScale.z / 2);

            Vector3 vec = new Vector3(posx, 0f, posz);
            Debug.Log(vec);
            GameObject box = PhotonNetwork.Instantiate("Cube", itemSpecialAreaPos.transform.position + vec, Quaternion.identity);

            itemCount++;
        }

        //큰지역 빈오브젝트 만큼 포문돌림
        for (int i = 0; i < itemBigAreaPos.Length; i++)
        {
            int item = Random.Range(1, 5);
            for (int j = 0; j < item; j++)
            {
                int posx = Random.Range(-(int)itemBigAreaPos[i].transform.localScale.x / 2, (int)itemBigAreaPos[i].transform.localScale.x / 2);
                int posz = Random.Range(-(int)itemBigAreaPos[i].transform.localScale.z / 2, (int)itemBigAreaPos[i].transform.localScale.z / 2);
                Vector3 vec = new Vector3(posx, 0f, posz);

                GameObject box = PhotonNetwork.Instantiate("Cube", itemBigAreaPos[i].transform.position + vec, Quaternion.identity);
                itemCount++;
                if (itemCount > itemMaxCount)
                {
                    break;
                }
            }
        }
        if (itemCount < itemMaxCount)
        {
            int check = 0;
            //다리 위치에 놓을 아이템
            while (true)
            {
               
                int rad = Random.Range(0, 2);
                int posx = Random.Range(-(int)itemBridgeAreadPos[check].transform.localScale.x / 2, (int)itemBridgeAreadPos[check].transform.localScale.x / 2);
                int posz = Random.Range(-(int)itemBridgeAreadPos[check].transform.localScale.z / 2, (int)itemBridgeAreadPos[check].transform.localScale.z / 2);
                Vector3 vec = new Vector3(posx, 0f, posz);
                if (rad == 1)
                {

                    GameObject box = PhotonNetwork.Instantiate("Cube", itemBridgeAreadPos[check].transform.position + vec, Quaternion.identity);
                    itemCount++;

                }
                check++;
                if (check == itemBridgeAreadPos.Length-1)
                {
                    check = 0;
                }
                if (itemCount == itemMaxCount)
                {
                    break;
                }
                Debug.Log(itemCount);
            }

        }
    }
    [PunRPC]
    void ReleasePool(int viewID)
    {
        Release(GameMgr.Instance.PunFindObject(viewID));
    }
    public void Release(GameObject obj)
    {   //큐로 다시 보낸다
        obj.gameObject.SetActive(false);   //플레이어에 닿으면 false시키고 큐에 저장
        itemQueue.Enqueue(obj);

        StartCoroutine(TenSec());    //x초 코루틴 실행
    }

    IEnumerator TenSec()
    {
        yield return new WaitForSeconds(30f);

        if (PhotonNetwork.IsMasterClient)
        {

            photonView.RPC("ItemRespawn", RpcTarget.All);
        }
    }
}
