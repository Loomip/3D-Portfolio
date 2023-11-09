using System.Collections;
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

    // ��ȭ �ܰ躰 Ȯ��
    private float[] enhanceChances = new float[] { 0.8f, 0.7f, 0.6f, 0.5f, 0.4f };

    // ��ȭ �ܰ躰 �ɷ�ġ ������
    private Dictionary<e_StatType, List<int>> enhanceStats = new Dictionary<e_StatType, List<int>>();

    // ��ȭ �ܰ� (0 ~ 4: 0�̸� 1��, 4�� 5��)
    private int enhanceLevel = 0;

    Slot slot;

    List<Slot> slotList = new List<Slot>();

    List<ItemData> dataList = new List<ItemData>();

    public void InitSlots()
    {
        dataList = InventoryManager.Inst.GetItemList();

        for (int i = 0; i < InventoryManager.Inst.MAXSLOTCOUNT; i++)
        {
            slot = Instantiate(enhanceSlotPrefab, enhanceContent).GetComponent<Slot>();
            slot.SLOTINDEX = i;
            slotList.Add(slot);
        }
    }

    //�κ��丮�� ������ �������� �����ϴ� �޼���
    public void RefreshIcon()
    {
        dataList = InventoryManager.Inst.GetItemList();
        InventoryManager.Inst.CUR_SLOT_COUNT = dataList.Count;

        for (int i = 0; i < InventoryManager.Inst.MAXSLOTCOUNT; i++)
        {
            if (i < InventoryManager.Inst.CUR_SLOT_COUNT && -1 < dataList[i].id)
            {
                slotList[i].Set_Icon(dataList[i]);
            }
            else
                slotList[i].ClearSlot();

            slotList[i].SelectSlot(false);
        }
    }

    public void RecordItem()
    {
        for (int i = 0; i < InventoryManager.Inst.GetItemList().Count; ++i)
        {
            if (i < dataList.Count)
            {
                RefreshIcon();
            }
        }
    }

    public void Initialize()
    {
        InitSlots();
    }

    private void Awake()
    {
        tooltip_Icon.enabled = true;
        // ��ȭ �ܰ躰 �ɷ�ġ ������ �ʱ�ȭ
        enhanceStats.Add(e_StatType.HP, new List<int> { 10, 20, 30, 40, 50 });
        enhanceStats.Add(e_StatType.Atk, new List<int> { 5, 10, 15, 20, 25 });
    }

    private void OnEnable()
    {
        RecordItem();
    }
}
