using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static UnityEditor.Progress;

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

    //버튼용 Delegate(대리자)
    delegate void UseButton();

    UseButton useButton;

    // 강화 단계별 확률
    private float[] enhanceChances = new float[] { 0.8f, 0.7f, 0.6f, 0.5f, 0.4f };

    // 강화 단계별 능력치 증가량
    private Dictionary<e_StatType, List<int>> enhanceStats = new Dictionary<e_StatType, List<int>>();

    // 강화 단계 (0 ~ 4: 0이면 1강, 4면 5강)
    private int enhanceLevel = 0;

    Slot slot;

    List<Slot> slotList = new List<Slot>();

    List<ItemData> dataList = new List<ItemData>();

    Data_Item.Param m_Item;

    //아이템 버튼을 누르면 호출하는 함수
    public void ItemButton() => useButton.Invoke();

    //강화탭 인벤토리
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

    //인벤토리의 아이템 아이콘을 갱신하는 메서드
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

    //아이템의 정보를 표시해주는 함수
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

    //버튼을 바꿔주는 함수
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

    //아이템 선택한 것을 바꿔주는 함수
    public void SelectSlot(Slot newSlot)
    {
        if (slot != null)
        {
            slot.SelectSlot(false);
        }

        newSlot.SelectSlot(true);
        slot = newSlot;
    }

    //선생님에게 물어보기
    private void Button_Enhance(ItemData item)
    {
        
    }

    private ItemData GetSelectedItem()
    {
        ItemData selectedItem = null;

        // 아이템 슬롯들을 순회하면서 선택된 아이템을 찾음
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
        // 강화 단계별 능력치 증가량 초기화
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
