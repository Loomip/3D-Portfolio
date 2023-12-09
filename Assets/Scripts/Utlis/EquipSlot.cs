using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;


public class EquipSlot : Slot
{
    public Image img_Frame;

    public ItemData EquippedItem { get; private set; } // �� ���Կ� ������ ������

    public e_EquipType Type { get; private set; }

    public Player player;

    //���⸦ ���� �ִ���
    public bool IsEquipped { get; private set; } = false;

    public void Set(ItemData data)
    {
        // ������ ������ ����
        EquippedItem = data;

        // ���� ���� ����
        IsEquipped = true;
        
        // ���� ������ ����
        InventoryManager.instance.RemoveItem(data);

        img_Icon.enabled = true;

        Set_Icon(data);

        // �÷��̾� ���� ������Ʈ
        var status = DataManager.instance.GetItemDataStatus(data.id);

        foreach (var s in status)
        {
            player.stat.AddStat(s.Key, s.Value);
        }
    }

    public ItemData Detach()
    {
        // ���� �����Ǵ� ������ ������ �ӽ� ������ ����
        ItemData detachedItem = EquippedItem;

        var status = DataManager.instance.GetItemDataStatus(detachedItem.id);

        foreach (var s in status)
        {
            player.stat.RemoveStat(s.Key, s.Value);
        }
    
        // ������ �������� ������
        img_Icon.sprite = null;
        img_Icon.enabled = false;

        // ������ �������� �κ��丮�� �߰�
        InventoryManager.instance.AddItem(detachedItem);

        EquippedItem = null;
        IsEquipped = false;

        // ���� �����Ǵ� ������ ������ ��ȯ
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
