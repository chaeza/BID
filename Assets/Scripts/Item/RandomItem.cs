using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomItem : MonoBehaviour
{
    private int itemType = 3;//�� ������ ����
    private int itemRan = 0;//�������� ���� ������ ��ȣ
    public void GetRandomitem(GameObject player)// ���������� ����
    {
        if (GameMgr.Instance.inventory.InvetoryCount(0) != true && GameMgr.Instance.inventory.InvetoryCount(1) != true && GameMgr.Instance.inventory.InvetoryCount(2) != true && GameMgr.Instance.inventory.InvetoryCount(3) != true)
        {
            Debug.Log("�κ��丮�� ���� á���ϴ�.");
            return;
        }
        itemRan = Random.Range(0, itemType);
        while (true)
        {
            if (itemRan == 0 && GameMgr.Instance.inventory.ContainInventory(0) == false)
            {
                player.AddComponent<TestItem>().GetItem(itemRan, GameMgr.Instance.inventory.AddInventory(itemRan));
                break;
            }
            else if (itemRan == 1 && GameMgr.Instance.inventory.ContainInventory(1) == false)
            {
                player.AddComponent<Buff_HpRecovery>().GetItem(itemRan, GameMgr.Instance.inventory.AddInventory(itemRan));
                break;
            }
            else if (itemRan == 2 && GameMgr.Instance.inventory.ContainInventory(2) == false)
            {
                player.AddComponent<Buff_Dash>().GetItem(itemRan, GameMgr.Instance.inventory.AddInventory(itemRan));
                break;
            }
            else itemRan = Random.Range(0, itemType);//�ߺ��� �ٽ� ����
        }
    }
}
