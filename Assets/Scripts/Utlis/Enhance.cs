using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


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

        int weaponIndex = 0; // ���� �����۸��� ī��Ʈ�ϴ� ���ο� �ε��� ����

        for (int i = 0; i < dataList.Count; i++)
        {
            string ItemType = DataManager.instance.GetItemDataParams(dataList[i].id).ItemType;
            e_ItemType e_Item = BaseData.ToEnum<e_ItemType>(ItemType);
            int baseItemId = (dataList[i].id / 1000) * 1000;
            enhanceLevel = dataList[i].id - baseItemId;

            if (e_Item == e_ItemType.Weapon && enhanceLevel < Consts.MAX_ENHANCE_LEVEL)
            {
                slotList[weaponIndex].Set_Icon(dataList[i]);
                weaponIndex++;
            }
        }

        // ���� ���Ե��� �ʱ�ȭ�մϴ�.
        for (int i = weaponIndex; i < InventoryManager.instance.MAXSLOTCOUNT; i++)
        {
            slotList[i].ClearSlot();
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
            tooltip_Icon.sprite = Resources.Load<Sprite>("Itemicons/" + m_Item.SpritName);
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

    private void Button_Enhance(ItemData item)
    {
        // �������� ���� �κ��丮�� �ִ��� Ȯ��
        if (!InventoryManager.instance.ItemExist(item))
        {
            Debug.Log("The item does not exist in the inventory!");
            return;
        }

        // ������ ������ ���� �⺻ ������ ID�� ������
        int baseItemId = (item.id / 1000) * 1000;

        // ������ ID�� ������� ��ȭ ������ ���
        int itemEnhanceLevel = item.id - baseItemId;

        // ��ȭ ������ enhanceChances �迭�� ������ ����� ��ȭ�� �õ����� ����
        if (itemEnhanceLevel >= enhanceChances.Length)
        {
            Debug.Log("Item has reached maximum enhance level!");
            return;
        }

        // ��ȭ Ȯ���� ����
        float enhanceChance = enhanceChances[itemEnhanceLevel];

        // ������ ���ڸ� ���ؼ� ��ȭ Ȯ���� ��
        if (Random.value <= enhanceChance)
        {
            // ��ȭ ���� �� ����
            EnhanceItem(item);

            // ��ȭ ���� �� ���� ��Ȱ��ȭ
            Refresh_Tooltip(null);

            Refresh_Button(null);

            // ������ ����Ʈ ������Ʈ
            RefreshIcon();
        }
        else
        {
            // ��ȭ ���� �� ����
            Debug.Log("Enhance Failed!");
        }
    }

    private void EnhanceItem(ItemData item)
    {
        // ��ȭ ���� ����
        enhanceLevel++;

        // ��ȭ�� �������� ���ο� ���Կ� �߰�
        AddEnhancedItemToNewSlot(item);

        // ���� ������ ����
        InventoryManager.instance.RemoveItem(item);

        // ������ �ʱ�ȭ
        slot.ClearSlot();

        Debug.Log("Enhance Success!");
    }

    private void AddEnhancedItemToNewSlot(ItemData item)
    {
        // ��ȭ�� ������ ID�� ����
        int enhancedItemId = item.id + 1;

        // ��ȭ�� ������ �����͸� �ҷ���
        Data_Item.Param enhancedItemData = DataManager.instance.GetItemDataParams(enhancedItemId);

        // �ҷ��� ������ �����Ͱ� ���ٸ� ��ȭ�� �������� ���� ���̹Ƿ� ����
        if (enhancedItemData == null)
        {
            Debug.Log("No item data for the enhanced item ID: " + enhancedItemId);
            return;
        }

        // ���ο� ������ �����͸� ����
        ItemData newItem = new ItemData
        {
            id = enhancedItemId
        };

        // ���ο� ���Կ� ������ �߰�
        InventoryManager.instance.AddItem(newItem);
    }

    private void Awake()
    {
        InitSlots();
        tooltip_Icon.enabled = true;
        ItemData selectedItem = GetSelectedItem();
        Refresh_Tooltip(selectedItem);
        Refresh_Button(selectedItem);
    }

    private void Update()
    {
        RecordItem();
    }
}
