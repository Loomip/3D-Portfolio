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

    //버튼용 Delegate(대리자)
    delegate void UseButton();

    UseButton useButton;

    // 강화 단계별 확률
    private float[] enhanceChances = new float[] { 0.8f, 0.7f, 0.6f, 0.5f, 0.4f };

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

        int weaponIndex = 0; // 무기 아이템만을 카운트하는 새로운 인덱스 변수

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

        // 남은 슬롯들을 초기화합니다.
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
            tooltip_Icon.sprite = Resources.Load<Sprite>("Itemicons/" + m_Item.SpritName);
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

    private void Button_Enhance(ItemData item)
    {
        // 아이템이 아직 인벤토리에 있는지 확인
        if (!InventoryManager.instance.ItemExist(item))
        {
            Debug.Log("The item does not exist in the inventory!");
            return;
        }

        // 아이템 종류에 따라 기본 아이템 ID를 가져옴
        int baseItemId = (item.id / 1000) * 1000;

        // 아이템 ID를 기반으로 강화 레벨을 계산
        int itemEnhanceLevel = item.id - baseItemId;

        // 강화 레벨이 enhanceChances 배열의 범위를 벗어나면 강화를 시도하지 않음
        if (itemEnhanceLevel >= enhanceChances.Length)
        {
            Debug.Log("Item has reached maximum enhance level!");
            return;
        }

        // 강화 확률을 구함
        float enhanceChance = enhanceChances[itemEnhanceLevel];

        // 랜덤한 숫자를 구해서 강화 확률과 비교
        if (Random.value <= enhanceChance)
        {
            // 강화 성공 시 로직
            EnhanceItem(item);

            // 강화 성공 후 툴팁 비활성화
            Refresh_Tooltip(null);

            Refresh_Button(null);

            // 아이템 리스트 업데이트
            RefreshIcon();
        }
        else
        {
            // 강화 실패 시 로직
            Debug.Log("Enhance Failed!");
        }
    }

    private void EnhanceItem(ItemData item)
    {
        // 강화 레벨 증가
        enhanceLevel++;

        // 강화된 아이템을 새로운 슬롯에 추가
        AddEnhancedItemToNewSlot(item);

        // 원본 아이템 제거
        InventoryManager.instance.RemoveItem(item);

        // 아이템 초기화
        slot.ClearSlot();

        Debug.Log("Enhance Success!");
    }

    private void AddEnhancedItemToNewSlot(ItemData item)
    {
        // 강화된 아이템 ID를 구함
        int enhancedItemId = item.id + 1;

        // 강화된 아이템 데이터를 불러옴
        Data_Item.Param enhancedItemData = DataManager.instance.GetItemDataParams(enhancedItemId);

        // 불러온 아이템 데이터가 없다면 강화된 아이템이 없는 것이므로 종료
        if (enhancedItemData == null)
        {
            Debug.Log("No item data for the enhanced item ID: " + enhancedItemId);
            return;
        }

        // 새로운 아이템 데이터를 생성
        ItemData newItem = new ItemData
        {
            id = enhancedItemId
        };

        // 새로운 슬롯에 아이템 추가
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
