using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Enhance : MonoBehaviour
{
    [Header("��ġ")]
    //������ ���Ե��� ���� ��ġ
    public Transform enhanceContent;

    [Header("������")]
    //������ ���� ������
    public GameObject enhanceSlotPrefab;

    [Header("��ư")]
    //���/���� ��ư 
    public TextMeshProUGUI btn_Enhance;

    //���õ� �������� �̹���
    public Image tooltip_Icon;

    //��ư�� Delegate(�븮��)
    delegate void UseButton();

    UseButton useButton;

    // ��ȭ �ܰ躰 Ȯ��
    private float[] enhanceChances = new float[] { 0.8f, 0.7f, 0.6f, 0.5f, 0.4f };

    // ��ȭ �ܰ躰 �ɷ�ġ ������
    private Dictionary<e_StatType, List<int>> enhanceStats = new Dictionary<e_StatType, List<int>>();

    // ��ȭ �ܰ� (0 ~ 4: 0�̸� 1��, 4�� 5��)
    private int enhanceLevel = 0;

    Slot slot;

    List<Slot> slotList = new List<Slot>();

    List<ItemData> dataList = new List<ItemData>();

    Data_Item.Param m_Item;

    //������ ��ư�� ������ ȣ���ϴ� �Լ�
    public void ItemButton() => useButton.Invoke();

    //��ȭ�� �κ��丮
    private void InitSlots()
    {
        dataList = InventoryManager.instance.GetItemList();

        for (int i = 0; i < InventoryManager.instance.MAXSLOTCOUNT; i++)
        {
            slot = Instantiate(enhanceSlotPrefab, enhanceContent).GetComponent<Slot>();
            slot.SLOTINDEX = i;
            slot.onItemClick += Refresh_Tooltip;
            slot.onItemClick += Refresh_Button;
            slotList.Add(slot);
        }
    }

    //�κ��丮�� ������ �������� �����ϴ� �޼���
    private void RefreshIcon()
    {
        dataList = InventoryManager.instance.GetItemList();
        InventoryManager.instance.CUR_SLOT_COUNT = dataList.Count;

        for (int i = 0; i < InventoryManager.instance.MAXSLOTCOUNT; i++)
        {
            if (i < InventoryManager.instance.CUR_SLOT_COUNT && -1 < dataList[i].id)
            {
                slotList[i].Set_Icon(dataList[i]);
            }
            else
                slotList[i].ClearSlot();

            slotList[i].SelectSlot(false);
        }
    }

    private void RecordItem()
    {
        for (int i = 0; i < InventoryManager.instance.GetItemList().Count; ++i)
        {
            if (i < dataList.Count)
            {
                RefreshIcon();
            }
        }
    }

    //�������� ������ ǥ�����ִ� �Լ�
    private void Refresh_Tooltip(ItemData item)
    {
        if (item == null)
        {
            tooltip_Icon.color = Color.clear;
            tooltip_Icon.sprite = null;
        }
        else
        {
            m_Item = DataManager.instance.GetItemDataParams(item.id);
            tooltip_Icon.color = Color.white;
            tooltip_Icon.sprite = Resources.Load<Sprite>("Itemicons/" + m_Item.Name);
        }
    }

    //��ư�� �ٲ��ִ� �Լ�
    private void Refresh_Button(ItemData item)
    {
        if (item == null)
        {
            btn_Enhance.transform.parent.gameObject.SetActive(false);
            return;
        }

        btn_Enhance.transform.parent.gameObject.SetActive(true);

        if (DataManager.instance.GetItemDataParams(item.id) != null)
        {
            string ItemType = DataManager.instance.GetItemDataParams(item.id).ItemType;
            e_ItemType itemType = BaseData.ToEnum<e_ItemType>(ItemType);

            switch (itemType)
            {
                case e_ItemType.Spend:
                    btn_Enhance.transform.parent.gameObject.SetActive(false);
                    break;
                case e_ItemType.Weapon:
                    btn_Enhance.text = DataManager.instance.GetWordData("Enhance");
                    useButton = () => Button_Enhance(item);
                    break;
            }
        }
    }

    //������ ������ ���� �ٲ��ִ� �Լ�
    public void SelectSlot(Slot newSlot)
    {
        if (slot != null)
        {
            slot.SelectSlot(false);
        }

        newSlot.SelectSlot(true);
        slot = newSlot;
    }

    //�����Կ��� �����
    private void Button_Enhance(ItemData item)
    {
        
    }

    private ItemData GetSelectedItem()
    {
        ItemData selectedItem = null;

        // ������ ���Ե��� ��ȸ�ϸ鼭 ���õ� �������� ã��
        foreach (var slot in slotList)
        {
            if (slot.SELECT)
            {
                selectedItem = slot.GetItem();
                break;
            }
        }

        return selectedItem;
    }


    private void Awake()
    {
        InitSlots();
        tooltip_Icon.enabled = true;
        // ��ȭ �ܰ躰 �ɷ�ġ ������ �ʱ�ȭ
        enhanceStats.Add(e_StatType.HP, new List<int> { 10, 20, 30, 40, 50 });
        enhanceStats.Add(e_StatType.Atk, new List<int> { 5, 10, 15, 20, 25 });
        ItemData selectedItem = GetSelectedItem();
        Refresh_Tooltip(selectedItem);
        Refresh_Button(selectedItem);
    }

    private void OnEnable()
    {
        RecordItem();
    }
}
