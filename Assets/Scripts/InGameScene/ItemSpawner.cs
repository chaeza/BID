using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ItemSpawner : MonoBehaviourPun
{
    [Header("������ ��ġ ��ǥ")]
    [SerializeField] private List<GameObject> itemAreaPos = null;
    [Header("����� ��ġ ��ǥ")]
    [SerializeField] private GameObject itemSpecialAreaPos = null;

/*    //������ ������
    [Header("������ ������")]
    [SerializeField] private GameObject itemPrefab;*/
    //������ ����
    [Header("������ ����")]
    [SerializeField] private int itemMaxCount = 0;

    //������ Ǯ�� ���� ť
    Queue<GameObject> itemQueue = new Queue<GameObject>();

    //������
    private int itemCount = 0;


    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
            RandomBigAreaItemSpawn();
    }
    public void RemoveItemList(GameObject Pos)
    {
        itemAreaPos.Remove(Pos);
    }
    public Vector3 RandomPos(GameObject spot)
    {
        int posX = Random.Range(-(int)spot.transform.localScale.x / 2, (int)spot.transform.localScale.x / 2);
        int posY = Random.Range(-(int)spot.transform.localScale.y / 2, (int)spot.transform.localScale.y / 2);
        Debug.Log(posX);
        Debug.Log(posY);

        return new Vector3(posX,0, posY);
    }
    public void RandomBigAreaItemSpawn()
    {
        //����� ������ �׻� 4���� �������� ���´�
        for (int i = 0; i < 4; i++)
        {
            GameObject box = PhotonNetwork.Instantiate("Cube", itemSpecialAreaPos.transform.position + RandomPos(itemSpecialAreaPos), Quaternion.identity);
            itemCount++;
        }



        //ū���� �������Ʈ ��ŭ ��������
        for (int i = 0; i < itemAreaPos.Count; i++)
        {
            int item = Random.Range(1, 5);
            for (int j = 0; j < item; j++)
            {
                GameObject box = PhotonNetwork.Instantiate("Cube", itemAreaPos[i].transform.position + RandomPos(itemAreaPos[i]), Quaternion.identity);
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
            //�ٸ� ��ġ�� ���� ������
            while (true)
            {             
                int ran = Random.Range(0, 2);
 
                if (ran == 1)
                {
                    GameObject box = PhotonNetwork.Instantiate("Cube", itemAreaPos[check].transform.position + RandomPos(itemAreaPos[check]), Quaternion.identity);
                    itemCount++;
                }
                check++;
                if (check == itemAreaPos.Count-1)
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
    void ItemRespawn()
    {
        itemAreaPos.Clear();

        itemAreaPos.AddRange(GameObject.FindGameObjectsWithTag("SpawnArea"));

        GameObject obj;

        obj = itemQueue.Dequeue();
        int num = Random.Range(0, itemAreaPos.Count);
        obj.transform.position = itemAreaPos[num].transform.position + RandomPos(itemAreaPos[num]);
        obj.gameObject.SetActive(true);
    }


    [PunRPC]
    void ReleasePool(int viewID)
    {
        Debug.Log("ReleasePool");
        if(GameMgr.Instance.PunFindObject(viewID)!=null) Release(GameMgr.Instance.PunFindObject(viewID));
    }
    public void Release(GameObject obj)
    {   //ť�� �ٽ� ������
        Debug.Log("Release");
        obj.gameObject.SetActive(false);   //�÷��̾ ������ false��Ű�� ť�� ����
        itemQueue.Enqueue(obj);

        StartCoroutine(TenSec());    //x�� �ڷ�ƾ ����
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
