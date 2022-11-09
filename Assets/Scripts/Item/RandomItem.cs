using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomItem : MonoBehaviour
{
    private int itemNum = 1;//�� ������ ����
    private int itemRan = 0;//�������� ���� ������ ��ȣ
    public void GetRandomitem(GameObject player)// ���������� ����
    {
        if (GameMgr.Instance.inventory.InvetoryCount(1) != true && GameMgr.Instance.inventory.InvetoryCount(2) != true && GameMgr.Instance.inventory.InvetoryCount(3) != true && GameMgr.Instance.inventory.InvetoryCount(4) != true)
        {
            Debug.Log("�κ��丮�� ���� á���ϴ�.");
            return;
        }
        itemRan = Random.Range(0, itemNum);
        while (true)
        {
            if (itemRan == 0 && GameMgr.Instance.inventory.ContainInventory(0) == false)
            {
                player.AddComponent<TestItem>().GetItem(GameMgr.Instance.inventory.AddInventory(itemRan));
                break;
            }
            else itemRan = Random.Range(0, itemNum);//�ߺ��� �ٽ� ����
        }
    }
}
