using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

//�������� ���� ID�� ������ ����
public class ItemData
{
    public int id; //������ ���� ID
    public int amount; //������ ����
    public int enhanceLevel; // �������� ��ȭ ����
}

public class InventoryManager : SingletonDontDestroy<InventoryManager>
{
    //���
    [SerializeField] TextMeshProUGUI goldText;

    // ���� ���
    public int gold;

    //��� UI�� �������� ���ִ� �Լ�
    public void Refresh_Gold()
    {
        if(goldText != null)
        goldText.text = string.Format("{0: #,##0} ���", gold);
    }


    public void StartGold()
    {
        gold = Consts.START_GOLD;
    }


    //============================================================================================================
    [Header("�κ��丮 �ִ� ����")]
    private int maxSlotCount = Consts.MAX_SLOT_COUNT;
    public int MAXSLOTCOUNT
    {
        get => maxSlotCount;
    }

    [Header("�κ��丮 ���� ����")]
    private int curSlotCount;
    public int CUR_SLOT_COUNT
    {
        get => curSlotCount;
        set => curSlotCount = value;
    }

    //���� �κ��丮�� �ִ� ������ ����� �����ϴ� ����Ʈ
    private List<ItemData> items = new List<ItemData>();

    public List<ItemData> GetItemList()
    {
        CUR_SLOT_COUNT = items.Count;
        return items;
    }

    public bool ItemExist(ItemData item)
    {
        return GetItemList().Contains(item);
    }

    //�κ��丮�� �� �������� �߰��ϴ� �޼���
    public void AddItem(ItemData newItem)
    {
        int index = FindItemIndex(newItem);

        if (DataManager.instance.GetItemData(newItem.id, out Data_Item.Param item))
        {
            if (-1 < index) //�κ��丮�� �ִ� ������
            {
                items[index].amount += 1;
            }
            else // ���� �κ��丮�� ���� ������
            {
                newItem.id = item.ID;
                newItem.amount = 1;
                items.Add(newItem);
                curSlotCount++;
            }
        }
    }

    // �κ��丮���� �������� ����
    public void RemoveItem(ItemData newItem)
    {
        int index = FindItemIndex(newItem);

        if (index >= 0) // �κ��丮�� �������� ������ ���
        {
            // �������� ���� ����
            items[index].amount -= 1;

            // �������� ������ 0�� �Ǹ� �κ��丮���� ������ ����
            if (items[index].amount <= 0)
            {
                items.RemoveAt(index);
                curSlotCount--;
            }
        }
        else
        {
            Debug.LogError("�κ��丮�� ���� �������� �����Ϸ��� �õ��߽��ϴ�.");
        }
    }

    //�־��� �������� �κ��丮�� �ִ� ��ġ�� ã�� �޼���
    private int FindItemIndex(ItemData newItem)
    {
        int result = -1;
        for (int i = items.Count - 1; i >= 0; i--)
        {
            if (items[i].id == newItem.id)
            {
                result = i;
                break;
            }
        }
        return result;
    }


    public List<ItemData> GetEquipmentItems()
    {
        return items.Where(item => DataManager.instance.GetItemDataParams(item.id).ItemType == "Weapon").ToList();
    }

    protected override void DoAwake()
    {
        Refresh_Gold();
    }
}
