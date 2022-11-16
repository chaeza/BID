using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomItem : MonoBehaviour
{
    private int itemType = 7;//�� ������ ����
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
                player.AddComponent<Buff_DamageDecrease>().GetItem(itemRan, GameMgr.Instance.inventory.AddInventory(itemRan));
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
            else if (itemRan == 3 && GameMgr.Instance.inventory.ContainInventory(3) == false)
            {
                player.AddComponent<Buff_BasicAttackUp>().GetItem(itemRan, GameMgr.Instance.inventory.AddInventory(itemRan));
                break;
            }
            else if (itemRan == 4 && GameMgr.Instance.inventory.ContainInventory(4) == false)
            {
                player.AddComponent<Buff_Unbeatable>().GetItem(itemRan, GameMgr.Instance.inventory.AddInventory(itemRan));
                break;
            }
            else if (itemRan == 5 && GameMgr.Instance.inventory.ContainInventory(5) == false)
            {
                player.AddComponent<Buff_BasicMoveSpeedUp>().GetItem(itemRan, GameMgr.Instance.inventory.AddInventory(itemRan));
                break;
            }
            else if (itemRan == 6 && GameMgr.Instance.inventory.ContainInventory(6) == false)
            {
                player.AddComponent<Buff_IceBall>().GetItem(itemRan, GameMgr.Instance.inventory.AddInventory(itemRan));
                break;
            }
            else itemRan = Random.Range(0, itemType);//�ߺ��� �ٽ� ����
        }
    }
}
