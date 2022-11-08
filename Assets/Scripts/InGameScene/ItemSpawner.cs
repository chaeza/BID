using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ItemSpawner : MonoBehaviourPun
{
    //�׶��� �±� ���� �迭
    [Header("������ ��ġ ��ǥ")]
    private GameObject[] itemPos = null;
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

    }

    //�⺻ ������ ����
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
