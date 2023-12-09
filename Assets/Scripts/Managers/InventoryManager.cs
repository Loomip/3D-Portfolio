using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

//아이템의 고유 ID와 개수를 저장
public class ItemData
{
    public int id; //아이템 고유 ID
    public int amount; //아이템 갯수
    public int enhanceLevel; // 아이템의 강화 레벨
}

public class InventoryManager : SingletonDontDestroy<InventoryManager>
{
    //골드
    [SerializeField] TextMeshProUGUI goldText;

    // 현재 골드
    public int gold;

    //골드 UI를 리프레쉬 해주는 함수
    public void Refresh_Gold()
    {
        if(goldText != null)
        goldText.text = string.Format("{0: #,##0} 골드", gold);
    }


    public void StartGold()
    {
        gold = Consts.START_GOLD;
    }


    //============================================================================================================
    [Header("인벤토리 최대 개수")]
    private int maxSlotCount = Consts.MAX_SLOT_COUNT;
    public int MAXSLOTCOUNT
    {
        get => maxSlotCount;
    }

    [Header("인벤토리 현재 개수")]
    private int curSlotCount;
    public int CUR_SLOT_COUNT
    {
        get => curSlotCount;
        set => curSlotCount = value;
    }

    //현재 인벤토리에 있는 아이템 목록을 저장하는 리스트
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

    //인벤토리에 새 아이템을 추가하는 메서드
    public void AddItem(ItemData newItem)
    {
        int index = FindItemIndex(newItem);

        if (DataManager.instance.GetItemData(newItem.id, out Data_Item.Param item))
        {
            if (-1 < index) //인벤토리에 있던 아이템
            {
                items[index].amount += 1;
            }
            else // 기존 인벤토리에 없던 아이템
            {
                newItem.id = item.ID;
                newItem.amount = 1;
                items.Add(newItem);
                curSlotCount++;
            }
        }
    }

    // 인벤토리에서 아이템을 제거
    public void RemoveItem(ItemData newItem)
    {
        int index = FindItemIndex(newItem);

        if (index >= 0) // 인벤토리에 아이템이 존재할 경우
        {
            // 아이템의 개수 감소
            items[index].amount -= 1;

            // 아이템의 개수가 0이 되면 인벤토리에서 아이템 제거
            if (items[index].amount <= 0)
            {
                items.RemoveAt(index);
                curSlotCount--;
            }
        }
        else
        {
            Debug.LogError("인벤토리에 없는 아이템을 제거하려고 시도했습니다.");
        }
    }

    //주어진 아이템이 인벤토리에 있는 위치를 찾는 메서드
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
