using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class EquipSlot : Slot
{
    public Image img_Frame;

    public ItemData EquippedItem { get; private set; } // �� ���Կ� ������ ������

    public e_EquipType Type { get; private set; }

    //���⸦ ���� �ִ���
    public bool IsEquipped { get; private set; } = false;

    public void Set(ItemData data)
    {
        // ������ ������ ����
        EquippedItem = data;

        // ���� ���� ����
        IsEquipped = true;

        Set_Icon(data);

        // �÷��̾� ���� ������Ʈ
        var status = DataManager.instance.GetItemDataStatus(data.id);

        foreach (var s in status)
        {
            Player.instance.stat.AddStat(s.Key, s.Value);
        }
    }

    public ItemData Detach()
    {
        // ���� �����Ǵ� ������ ������ �ӽ� ������ ����
        ItemData detachedItem = EquippedItem;

        var status = DataManager.instance.GetItemDataStatus(detachedItem.id);

        foreach (var s in status)
        {
            Player.instance.stat.RemoveStat(s.Key, s.Value);
        }
    
        // ������ �������� ������
        img_Icon.sprite = null;

        EquippedItem = null;
        IsEquipped = false;

        // ���� �����Ǵ� ������ ������ ��ȯ
        return detachedItem;
    }

    private void Start()
    {
        img_Frame.enabled = true;
    }
}
