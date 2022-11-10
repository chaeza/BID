using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ItemSpawner : MonoBehaviourPun
{
    //�׶��� �±� ���� �迭
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
    //�ش� �׶��忡 �������� �������� �Ǵ�
    private bool[] IsbeingItem = new bool[500];
    private Vector3 randomPos;
    int ran;
    //������ Ǯ�� ���� ť
    Queue<GameObject> itemQueue = new Queue<GameObject>();

    //������
    private int itemCount =0;

    private void Awake()
    {
        RandomBigAreaItemSpawn();
    }

    [PunRPC]
    [System.Obsolete]
    public void RandomBigAreaItemSpawn()
    {
        //����� ������ �׻� 4���� �������� ���´�
        for (int i = 0; i < 4; i++)
        {
            int posx = Random.Range((int)itemSpecialAreaPos.transform.position.x - 12, (int)itemSpecialAreaPos.transform.position.x + 12);
            int posz = Random.Range((int)itemSpecialAreaPos.transform.position.z - 12, (int)itemSpecialAreaPos.transform.position.z + 12);
            PhotonNetwork.InstantiateSceneObject("ItemBox", new Vector3(posx, itemSpecialAreaPos.transform.position.y, posz), Quaternion.identity);
            itemCount++;
        }

        //�������Ʈ ��ŭ ��������
        for (int i = 0; i < itemBigAreaPos.Length; i++)
        {
            int item = Random.Range(1, 4);
            for(int j = 0; j < item; j++) 
            {
                if(itemCount > itemMaxCount)
                {
                    break;
                }
                int posx = Random.Range(-20, 21) % 2;
                int posz = Random.Range(-20, 21) % 2;
                Vector3 vec = new Vector3(posx, 0f, posz);

                //int posx = Random.Range((int)itemBigAreaPos[i].transform.position.x - 10, (int)itemBigAreaPos[i].transform.position.x + 10);
                //int posz = Random.Range((int)itemBigAreaPos[i].transform.position.z - 10, (int)itemBigAreaPos[i].transform.position.z + 10); 
                PhotonNetwork.InstantiateSceneObject("ItemBox", itemBigAreaPos[i].transform.position + vec, Quaternion.identity);
                itemCount++;
            }
        }
        //�ٸ� ��ġ�� ���� ������
        for(int i = itemCount; i < itemMaxCount; i++)
        {
            if (itemCount > itemMaxCount)
            {
                break;
            }
            ran = Random.Range(0, itemBridgeAreadPos.Length);
            randomPos = itemBridgeAreadPos[ran].transform.position;
            while (IsbeingItem[i] == true)
            {
                ran = Random.Range(0, itemBigAreaPos.Length);
            }
            IsbeingItem[ran] = true;
            PhotonNetwork.InstantiateSceneObject("ItemBox", randomPos, Quaternion.identity);
            itemCount++;
        }
        
    }

    /*//�⺻ ������ ����
    [PunRPC]
    [System.Obsolete]
    public void ItemInit()
    {
       for(int i = itemCount; i < itemMaxCount; i++)
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
*/
    

    
}
