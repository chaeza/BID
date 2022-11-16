using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ItemSpawner : MonoBehaviourPun
{
    [Header("������ ��ġ ��ǥ")]
    [SerializeField] private List<SpawnArea_Ver2> itemAreaPos = null;
    [SerializeField] private List<SpawnArea_Ver2> itemAreaPos2 = null;

    [Header("����� ��ġ ��ǥ")]
    [SerializeField] private SpawnArea_Ver2 itemSpecialAreaPos = null;

    //SpawnArea_Ver2[] itemSpwanPos;
    //������ ����
    [Header("������ ����")]
    [SerializeField] private int itemMaxCount = 0;

    //������ Ǯ�� ���� ť
    Queue<GameObject> itemQueue = new Queue<GameObject>();

    private List<SpawnArea_Ver2> newSpawnArea = null;
    //������
    private int itemCount = 0;
    private int randomItemPos;

    private void Start()
    {
        for (int i = 0; i < itemAreaPos.Count; i++)
        {
            newSpawnArea.Add(itemAreaPos[i]);
        }
        for(int i =0; i < itemAreaPos2.Count; i++)
        {
            newSpawnArea.Add(itemAreaPos2[i]);
        }

        if (PhotonNetwork.IsMasterClient)
            RandomItemSpawn(itemMaxCount);
    }
    public void RandomItemSpawn(int value)
    {
        for(int i=0; i < 4; i++)
        {
            GameObject box = PhotonNetwork.Instantiate("ItemBox", itemSpecialAreaPos.getRandomPos(), Quaternion.identity);
        }
        for (int i = 4; i < value; i++)
        {
            int ran = Random.Range(0, 4);
            if(ran ==0 || ran == 1 || ran ==2)
            {
                randomItemPos = Random.Range(0, itemAreaPos.Count);
                GameObject box = PhotonNetwork.Instantiate("ItemBox", itemAreaPos[randomItemPos].getRandomPos(), Quaternion.identity);
            }
            else
            {
                randomItemPos = Random.Range(0, itemAreaPos2.Count);
                GameObject box = PhotonNetwork.Instantiate("ItemBox", itemAreaPos2[randomItemPos].getRandomPos(), Quaternion.identity);
            }
        }
    }
    public void RemoveItemList(GameObject Pos)
    {
        newSpawnArea.Remove(Pos.GetComponent<SpawnArea_Ver2>());
    }
    [PunRPC]
    void ItemRespawn()
    {

        if ((itemMaxCount - itemCount) < 10)
        {
            return;
        }
        else
        {
            newSpawnArea.Clear();
            newSpawnArea.AddRange(FindObjectsOfType<SpawnArea_Ver2>());
            GameObject obj;
            obj = itemQueue.Dequeue();
            int num = Random.Range(0, itemAreaPos.Count);
            randomItemPos = Random.Range(0, newSpawnArea.Count + 1);
            obj.transform.position = newSpawnArea[randomItemPos].getRandomPos();
            obj.gameObject.SetActive(true);
            itemCount++;
        }
    }


    [PunRPC]
    void ReleasePool(int viewID)
    {
        Debug.Log("ReleasePool");
        if (GameMgr.Instance.PunFindObject(viewID) != null) Release(GameMgr.Instance.PunFindObject(viewID));
    }
    public void Release(GameObject obj)
    {   //ť�� �ٽ� ������
        Debug.Log("Release");
        obj.gameObject.SetActive(false);   //�÷��̾ ������ false��Ű�� ť�� ����
        itemQueue.Enqueue(obj);
        itemMaxCount--;
        itemCount--;
        StartCoroutine(TenSec());    //x�� �ڷ�ƾ ����
    }

    IEnumerator TenSec()
    {
        yield return new WaitForSeconds(45f);

        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("ItemRespawn", RpcTarget.All);
        }
    }
}
