using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Enhance : MonoBehaviour
{
    [Header("위치")]
    //아이템 슬롯들이 들어가는 위치
    public Transform enhanceContent;

    [Header("프리팹")]
    //아이템 슬롯 프리팹
    public GameObject enhanceSlotPrefab;

    [Header("버튼")]
    //사용/장착 버튼 
    public TextMeshProUGUI btn_Enhance;

    //선택된 아이템의 이미지
    public Image tooltip_Icon;

    // 강화 단계별 확률
    private float[] enhanceChances = new float[] { 0.8f, 0.7f, 0.6f, 0.5f, 0.4f };

    // 강화 단계별 능력치 증가량
    private Dictionary<e_StatType, List<int>> enhanceStats = new Dictionary<e_StatType, List<int>>();

    // 강화 단계 (0 ~ 4: 0이면 1강, 4면 5강)
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

    //인벤토리의 아이템 아이콘을 갱신하는 메서드
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
        // 강화 단계별 능력치 증가량 초기화
        enhanceStats.Add(e_StatType.HP, new List<int> { 10, 20, 30, 40, 50 });
        enhanceStats.Add(e_StatType.Atk, new List<int> { 5, 10, 15, 20, 25 });
    }

    private void OnEnable()
    {
        RecordItem();
    }
}
