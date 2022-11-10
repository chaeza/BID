using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ItemSpawner : MonoBehaviourPun
{
    [Header("������ ��ġ ��ǥ")]
    [SerializeField] private GameObject[] itemBigAreaPos = null;
    [Header("����� ��ġ ��ǥ")]
    [SerializeField] private GameObject itemSpecialAreaPos = null;
    [Header("�ٸ� ��ġ ��ǥ")]
    [SerializeField] private GameObject[] itemBridgeAreadPos = null;
    //������ ������
    [Header("������ ������")]
    [SerializeField] private GameObject itemPrefab;
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
    public void RandomBigAreaItemSpawn()
    {
        //����� ������ �׻� 4���� �������� ���´�
        for (int i = 0; i < 4; i++)
        {
            Debug.Log("for��");
            int posx = Random.Range(-(int)itemSpecialAreaPos.transform.localScale.x / 2, (int)itemSpecialAreaPos.transform.localScale.x / 2);
            int posz = Random.Range(-(int)itemSpecialAreaPos.transform.localScale.z / 2, (int)itemSpecialAreaPos.transform.localScale.z / 2);

            Vector3 vec = new Vector3(posx, 0f, posz);
            Debug.Log(vec);
            GameObject box = PhotonNetwork.Instantiate("Cube", itemSpecialAreaPos.transform.position + vec, Quaternion.identity);

            itemCount++;
        }

        //ū���� �������Ʈ ��ŭ ��������
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
            //�ٸ� ��ġ�� ���� ������
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
    {   //ť�� �ٽ� ������
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
