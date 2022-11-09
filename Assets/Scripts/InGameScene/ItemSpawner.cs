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
    //������ ������
    [Header("������ ������")]
    [SerializeField] private GameObject itemPrefab;
    //������ ����
    [Header("������ ����")]
    [SerializeField] public int itemCount = 0;
    //�ش� �׶��忡 �������� �������� �Ǵ�
    private bool[] IsbeingItem = new bool[500];
    private Vector3 randomPos;
    int ran;
    //������ Ǯ�� ���� ť
    Queue<GameObject> itemQueue = new Queue<GameObject>();

    private void Awake()
    {
        RandomBigAreaItemSpawn();
    }

    public void RandomBigAreaItemSpawn()
    {
        //�������Ʈ ��ŭ ��������
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

    //�⺻ ������ ����
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
