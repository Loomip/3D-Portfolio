using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


//아이템의 고유 ID와 개수를 저장
public class ItemData
{
    public int id; //아이템 고유 ID
    public int amount; //아이템 갯수
}

public class Equip : MonoBehaviour
{
    private Data_Lacalize.Param m_localizeData;

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

    [Header("인벤토리 프리팹")]
    //아이템 슬롯 프리팹
    public GameObject itemslotPrefab;
    //장비 슬롯 프리팹
    public GameObject equipslotPrefab;

    [Header("인벤토리 위치")]
    //아이템 슬롯들이 들어가는 위치
    public RectTransform itemContent;

    //아이템 장비 슬롯들이 들어가는 위치
    public Transform equiplist;

    [Header("툴팁")]
    //아이템 정보 관련된 변수
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemExplanText;
    public Image tooltip_Icon;

    [Header("버튼")]
    //사용/장착 버튼 
    public TextMeshProUGUI btn_Use;
    //버리기 버튼
    public GameObject btn_Discard;

    //버튼용 Delegate(대리자)
    delegate void UseButton();

    UseButton useButton;

    Slot slot;

    List<Slot> slotList = new List<Slot>();

    //현제 인벤토리에 있는 아이템 목록을 저장하는 리스트
    private List<ItemData> items = new List<ItemData>();

    Dictionary<e_EquipType, EquipSlot> equipSlotList = new Dictionary<e_EquipType, EquipSlot>();

    public List<ItemData> GetItemList()
    {
        CUR_SLOT_COUNT = items.Count;
        return items;
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
            RefreshIcon();
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

            RefreshIcon();
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

    //인벤토리의 슬롯을 초기화 하는 메서드
    void InitSlots()
    {
        for (int i = 0; i < MAXSLOTCOUNT; i++)
        {
            slot = Instantiate(itemslotPrefab, itemContent).GetComponent<Slot>();
            slot.SLOTINDEX = i;
            slotList.Add(slot);

            slot.onItemClick += Refresh_Tooltip;
            slot.onItemClick += Refresh_Button;
        }
    }

    //장비 슬롯 생성
    void InitEquipSlots()
    {
        for (int i = 0; i < (int)e_EquipType.Length; ++i)
        {
            EquipSlot slot = Instantiate(equipslotPrefab, equiplist).GetComponent<EquipSlot>();
            slot.InitData();
            equipSlotList.Add((e_EquipType)i, slot);

            slot.onItemClick += Refresh_Button;
        }
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

    //인벤토리의 아이템 아이콘을 갱신하는 메서드
    public void RefreshIcon()
    {
        for (int i = 0; i < MAXSLOTCOUNT; i++)
        {
            if (i < CUR_SLOT_COUNT && -1 < items[i].id)
            {
                slotList[i].Set_Icon(items[i]);
            }
            else
                slotList[i].ClearSlot();

            slotList[i].SelectSlot(false);
        }
    }

    //아이템의 정보를 표시해주는 함수
    public void Refresh_Tooltip(ItemData item)
    {
        if (item == null)
        {
            tooltip_Icon.color = Color.clear;
            tooltip_Icon.sprite = null;
            itemExplanText.text = string.Empty;
            itemNameText.text = string.Empty;
        }
        else
        {
            m_localizeData = DataManager.instance.GetLocalizeData(item.id);
            tooltip_Icon.color = Color.white;
            tooltip_Icon.sprite = Resources.Load<Sprite>("Itemicons/" + m_localizeData.Name);
            itemExplanText.text = m_localizeData.Explan;
            itemNameText.text = m_localizeData.TooltipName;
        }
    }

    //아이템 버튼을 누르면 호출하는 함수
    public void ItemButton() => useButton.Invoke();

    //버튼을 바꿔주는 함수
    public void Refresh_Button(ItemData item)
    {
        if (item == null)
        {
            btn_Use.transform.parent.gameObject.SetActive(false);
            btn_Discard.SetActive(false);
            return;
        }

        btn_Use.transform.parent.gameObject.SetActive(true);

        btn_Discard.SetActive(true);

        if (DataManager.instance.GetItemDataParams(item.id) != null)
        {
            string ItemType = DataManager.instance.GetItemDataParams(item.id).ItemType;
            e_ItemType itemType = BaseData.ToEnum<e_ItemType>(ItemType);

            switch (itemType)
            {
                case e_ItemType.Spend:
                    btn_Use.text = DataManager.instance.GetWordData("Use");
                    useButton = () => Button_Use(item);
                    break;
                case e_ItemType.Weapon:
                    if (slot is EquipSlot)
                    {
                        EquipSlot equipSlot = slot as EquipSlot;
                        btn_Use.text = DataManager.instance.GetWordData("Unbind");
                        useButton = () => { equipSlot.Detach(); };
                        return;
                    }
                    btn_Use.text = DataManager.instance.GetWordData("Wear");
                    useButton = () => Button_Equip(item);
                    break;
                case e_ItemType.Item:
                    btn_Use.transform.parent.gameObject.SetActive(false);
                    break;
            }
        }
    }


    // 현재 활성화된 무기의 인덱스
    List<int> currentWeaponIndices = new List<int>();

    //사용하기
    private void Button_Use(ItemData itemData)
    {
        var Name = DataManager.instance.GetItemDataParams(itemData.id).Name;
        var status = DataManager.instance.GetItemDataStatus(itemData.id);
        e_Spend spend = BaseData.ToEnum<e_Spend>(Name);
        foreach (var item in status)
        {
            switch (spend)
            {
                case e_Spend.Bread:
                    Player.instance.stat.AddStat(e_StatType.HP, item.Value);
                    UIManager.instance.Refresh_HP(Player.instance);
                    break;
                case e_Spend.Croissant:
                    Player.instance.stat.AddStat(e_StatType.HP, item.Value);
                    UIManager.instance.Refresh_HP(Player.instance);
                    break;
                case e_Spend.Donut:
                    Player.instance.stat.AddStat(e_StatType.HP, item.Value);
                    UIManager.instance.Refresh_HP(Player.instance);
                    break;
                case e_Spend.HamburgerM:
                    Player.instance.stat.AddStat(e_StatType.HP, item.Value);
                    UIManager.instance.Refresh_HP(Player.instance);
                    break;
                case e_Spend.Pizza:
                    Player.instance.stat.AddStat(e_StatType.HP, item.Value);
                    UIManager.instance.Refresh_HP(Player.instance);
                    break;
            }
        }

        RemoveItem(itemData);
    }

    // 장비하기
    private void Button_Equip(ItemData item)
    {
        // 슬롯 종류를 비교
        // 선택한 장비의 타입
        Data_Item.Param itemData = DataManager.instance.GetItemDataParams(item.id);

        if (item == null)
        {
            Debug.LogError("아이템 데이터가 없습니다.");
            return;
        }

        // 아이템의 타입을 가져옴
        e_ItemType itemType = BaseData.ToEnum<e_ItemType>(itemData.ItemType);

        // 무기를 들었을 경우만 공격할수 있도록
        Player.instance.isWeaponEquipped = itemType == e_ItemType.Weapon;

        // 선택한 아이템이 무기인 경우
        if (itemType == e_ItemType.Weapon)
        {
            // 기존에 장착되어 있던 모든 무기들을 비활성화
            foreach (int index in currentWeaponIndices)
            {
                if (index >= 0 && index < Player.instance.weapons.Length)
                {
                    Player.instance.weapons[index].SetActive(false);
                }
            }

            // 무기 타입을 가져옴
            e_Weapon weaponType = GetWeaponType(itemData.Name);

            // 해당 무기 타입에 대한 인덱스 리스트를 가져옴
            List<int> indexList = GetWeaponIndexList(weaponType);

            currentWeaponIndices.Clear();

            foreach (int index in indexList)
            {
                if (index >= 0 && index < Player.instance.weapons.Length)
                {
                    Player.instance.weapons[index].SetActive(true);

                    // 현재 장착중인 무기의 인덱스 업데이트
                    currentWeaponIndices.Add(index);
                }
            }


            // 애니메이션 설정
            Player.instance.animator.SetInteger("weaponType", (int)weaponType);

            // 선택한 아이템의 타입과 일치하는 장비 슬롯을 찾음
            foreach (var equipSlot in equipSlotList.Values)
            {
                if (equipSlot.Type.ToString() == itemData.ItemType)
                {
                    // 이미 장착한 장비가 있는지 확인
                    if (!equipSlot.IsEquipped)
                    {
                        // 선택한 장비를 장착
                        equipSlot.Set(item);

                        // 선택한 아이템의 개수 감소
                        RemoveItem(item);
                    }
                    else
                    {
                        // 이미 장착한 장비를 해제
                        ItemData detachedItem = equipSlot.Detach();

                        // 해제된 아이템을 인벤토리에 추가
                        AddItem(detachedItem);

                        // 선택한 장비를 장착
                        equipSlot.Set(item);

                        // 선택한 아이템의 개수 감소
                        RemoveItem(item);
                    }
                    return;
                }


            }
        }
    }

    //버리기
    public void Button_Discard()
    {
        // 선택된 아이템이 없으면 함수를 종료
        if (slot == null)
        {
            Debug.LogError("버릴 아이템이 선택되지 않았습니다.");
            return;
        }

        // 아이템을 인벤토리에서 제거
        RemoveItem(slot.GetItem());

        // 선택된 아이템 초기화
        slot = null;
    }

    private e_Weapon GetWeaponType(string itemName)
    {
        return (e_Weapon)Enum.Parse(typeof(e_Weapon), itemName);
    }

    private List<int> GetWeaponIndexList(e_Weapon weaponType)
    {
        List<int> indexList = new List<int>();

        // 무기 유형에 따라 해당하는 인덱스를 리스트에 추가
        switch (weaponType)
        {
            case e_Weapon.None:
                break;
            case e_Weapon.One_handed_sword:
                indexList.Add(0);
                break;
            case e_Weapon.Two_handed_sword:
                indexList.Add(1);
                break;
            case e_Weapon.Karate:
                indexList.Add(2);
                indexList.Add(3);
                break;
            case e_Weapon.Glove:
                indexList.Add(4);
                indexList.Add(5);
                break;
            case e_Weapon.Bag:
                indexList.Add(6);
                break;
            case e_Weapon.Bead:
                indexList.Add(7);
                break;
            case e_Weapon.Musical_instruments:
                indexList.Add(8);
                break;
            case e_Weapon.Dance:
                indexList.Add(9);
                indexList.Add(10);
                break;
            case e_Weapon.Length:
                break;
            default:
                break;
        }

        return indexList;
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

    public void Btu()
    {
        // 랜덤한 아이템 데이터 가져오기
        Data_Item.Param randomItemData = DataManager.instance.GetRandomItemDataParams();

        // 가져온 아이템 데이터로 새 아이템 생성
        if (randomItemData != null)
        {
            ItemData newItem = new ItemData();
            newItem.id = randomItemData.ID;
            ++newItem.amount;

            // 새 아이템을 인벤토리에 추가
            AddItem(newItem);
        }
        else
        {
            Debug.Log("No item data available for random selection.");
        }
    }

    private void Start()
    {
        InitSlots();
        InitEquipSlots();
        ItemData selectedItem = GetSelectedItem();
        Refresh_Tooltip(selectedItem);
        Refresh_Button(selectedItem);
    }
}
