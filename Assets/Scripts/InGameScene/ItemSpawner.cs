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
        /*itemSpwanPos = FindObjectsOfType<SpawnArea_Ver2>();*/
        for(int i = 0; i < itemAreaPos.Count; i++)
        {
            newSpawnArea.Add(itemAreaPos[i]);
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
    public void RemoveItemList(SpawnArea_Ver2 Pos)
    {
        itemAreaPos.Remove(Pos);
    }
    /*[PunRPC]
    void ItemRespawn()
    {

        if ((itemMaxCount - itemCount) < 10)
        {
            return;
        }
        else
        {
            itemAreaPos.Clear();
            itemAreaPos2.Clear();
            itemAreaPos.AddRange(GameObject.FindGameObjectsWithTag("SpawnArea"));
            GameObject obj;
            obj = itemQueue.Dequeue();
            int num = Random.Range(0, itemAreaPos.Count);
            randomItemPos = Random.Range(0, itemSpwanPos.Length + 1);
            obj.transform.position = itemSpwanPos[randomItemPos].getRandomPos();
            obj.gameObject.SetActive(true);
            itemCount++;
        }
    }*/


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
