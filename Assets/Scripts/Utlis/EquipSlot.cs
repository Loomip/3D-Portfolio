using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;


public class EquipSlot : Slot
{
    public Image img_Frame;

    public ItemData EquippedItem { get; private set; } // 이 슬롯에 장착된 아이템

    public e_EquipType Type { get; private set; }

    public Player player;

    //무기를 끼고 있는지
    public bool IsEquipped { get; private set; } = false;

    public void Set(ItemData data)
    {
        // 아이템 데이터 설정
        EquippedItem = data;

        // 장착 상태 변경
        IsEquipped = true;
        
        // 원본 아이템 제거
        InventoryManager.instance.RemoveItem(data);

        img_Icon.enabled = true;

        Set_Icon(data);

        // 플레이어 상태 업데이트
        var status = DataManager.instance.GetItemDataStatus(data.id);

        foreach (var s in status)
        {
            player.stat.AddStat(s.Key, s.Value);
        }
    }

    public ItemData Detach()
    {
        // 장착 해제되는 아이템 정보를 임시 변수에 저장
        ItemData detachedItem = EquippedItem;

        var status = DataManager.instance.GetItemDataStatus(detachedItem.id);

        foreach (var s in status)
        {
            player.stat.RemoveStat(s.Key, s.Value);
        }
    
        // 장착된 아이템을 제거함
        img_Icon.sprite = null;
        img_Icon.enabled = false;

        // 해제된 아이템을 인벤토리에 추가
        InventoryManager.instance.AddItem(detachedItem);

        EquippedItem = null;
        IsEquipped = false;

        // 장착 해제되는 아이템 정보를 반환
        return detachedItem;
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }
    private void Update()
    {
        img_Frame.enabled = true;
    }
}
