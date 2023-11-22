using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Equip : MonoBehaviour
{
    private Data_Lacalize.Param m_localizeData;

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

    [Header("플레이어 스텟")]
    //체력
    public TextMeshProUGUI healthText;
    //공격력
    public TextMeshProUGUI attackPowerText;
    //스피드
    public TextMeshProUGUI speedText;


    //버튼용 Delegate(대리자)
    delegate void UseButton();

    UseButton useButton;
    Player player;

    //인벤토리 슬롯
    Slot slot;

    //슬롯들이 저장되는 리스트
    List<Slot> slotList = new List<Slot>();

    Dictionary<e_EquipType, EquipSlot> equipSlotList = new Dictionary<e_EquipType, EquipSlot>();

    List<ItemData> dataList = new List<ItemData>();

    //인벤토리의 슬롯을 초기화 하는 메서드
    private void InitSlots()
    {
        for (int i = 0; i < InventoryManager.instance.MAXSLOTCOUNT; i++)
        {
            slot = Instantiate(itemslotPrefab, itemContent).GetComponent<Slot>();
            slot.SLOTINDEX = i;
            slotList.Add(slot);

            slot.onItemClick += Refresh_Tooltip;
            slot.onItemClick += Refresh_Button;
        }
    }

    //장비 슬롯 생성
    private void InitEquipSlots()
    {
        for (int i = 0; i < (int)e_EquipType.Length; ++i)
        {
            EquipSlot slot = Instantiate(equipslotPrefab, equiplist).GetComponent<EquipSlot>();
            slot.InitData();
            equipSlotList.Add((e_EquipType)i, slot);

            slot.onItemClick += Refresh_Button;
        }
    }

    //아이템의 정보를 표시해주는 함수
    private void Refresh_Tooltip(ItemData item)
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
            tooltip_Icon.sprite = Resources.Load<Sprite>("Itemicons/" + m_localizeData.SpritName);
            itemExplanText.text = m_localizeData.Explan;
            itemNameText.text = m_localizeData.TooltipName;
        }
    }

    //인벤토리의 아이템 아이콘을 갱신하는 메서드
    public void RefreshIcon()
    {
        dataList = InventoryManager.instance.GetItemList();
        InventoryManager.instance.CUR_SLOT_COUNT = dataList.Count;

        for (int i = 0; i < InventoryManager.instance.MAXSLOTCOUNT; i++)
        {
            if (i < InventoryManager.instance.CUR_SLOT_COUNT)
            {
                if (dataList[i] != null)
                {
                    slotList[i].Set_Icon(dataList[i]);
                }
                else
                {
                    slotList[i].ClearSlot();
                }
            }
            else
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

    //아이템 버튼을 누르면 호출하는 함수
    public void ItemButton() => useButton.Invoke();

    //버튼을 바꿔주는 함수
    private void Refresh_Button(ItemData item)
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
                        // 기존에 장착되어 있던 모든 무기들을 비활성화
                        foreach (int index in currentWeaponIndices)
                        {
                            if (index >= 0 && index < player.weapons.Length)
                            {
                                player.weapons[index].SetActive(false);
                                player.animator.SetInteger("weaponType", 0);
                            }
                        }
                        return;
                    }
                    btn_Use.text = DataManager.instance.GetWordData("Wear");
                    useButton = () => Button_Equip(item);
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
                    player.stat.AddStat(e_StatType.HP, item.Value);
                    UIManager.instance.Refresh_HP(player);
                    break;
                case e_Spend.Croissant:
                    player.stat.AddStat(e_StatType.HP, item.Value);
                    UIManager.instance.Refresh_HP(player);
                    break;
                case e_Spend.Donut:
                    player.stat.AddStat(e_StatType.HP, item.Value);
                    UIManager.instance.Refresh_HP(player);
                    break;
                case e_Spend.HamburgerM:
                    player.stat.AddStat(e_StatType.HP, item.Value);
                    UIManager.instance.Refresh_HP(player);
                    break;
                case e_Spend.Pizza:
                    player.stat.AddStat(e_StatType.HP, item.Value);
                    UIManager.instance.Refresh_HP(player);
                    break;
            }
        }

        InventoryManager.instance.RemoveItem(itemData);

        // 아이템을 사용한 후에 아이템의 개수를 확인
        if (itemData.amount == 0)
        {
            Refresh_Tooltip(null);
            Refresh_Button(null);
        }
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
        player.isWeaponEquipped = itemType == e_ItemType.Weapon;

        // 선택한 아이템이 무기인 경우
        if (itemType == e_ItemType.Weapon)
        {
            // 기존에 장착되어 있던 모든 무기들을 비활성화
            foreach (int index in currentWeaponIndices)
            {
                if (index >= 0 && index < player.weapons.Length)
                {
                    player.weapons[index].SetActive(false);
                }
            }

            // 무기 타입을 가져옴
            e_Weapon weaponType = GetWeaponType(itemData);

            // 해당 무기 타입에 대한 인덱스 리스트를 가져옴
            List<int> indexList = GetWeaponIndexList(weaponType);

            currentWeaponIndices.Clear();

            foreach (int index in indexList)
            {
                if (index >= 0 && index < player.weapons.Length)
                {
                    player.weapons[index].SetActive(true);

                    // 현재 장착중인 무기의 인덱스 업데이트
                    currentWeaponIndices.Add(index);
                }
            }


            // 애니메이션 설정
            player.animator.SetInteger("weaponType", (int)weaponType);

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
                        RefreshIcon();
                    }
                    else
                    {
                        equipSlot.Detach();
                        RefreshIcon();

                        // 선택한 장비를 장착
                        equipSlot.Set(item);
                        RefreshIcon();
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
        InventoryManager.instance.RemoveItem(slot.GetItem());

        // 아이템을 버린 후에 아이템의 개수를 확인
        // 선택된 태두리가 안꺼짐 (수정사항)
        ItemData remainingItem = slot.GetItem();
        if (remainingItem == null || remainingItem.amount == 0)
        {
            Refresh_Tooltip(null);
            Refresh_Button(null);
            // 선택된 아이템 초기화
            slot = null;
        }

        RefreshIcon();
    }

    private e_Weapon GetWeaponType(Data_Item.Param item)
    {
        // SpritName에 따라 무기 유형을 결정
        switch (item.SpritName)
        {
            case "One_handed_sword":
                return e_Weapon.One_handed_sword;
            case "Two_handed_sword":
                return e_Weapon.Two_handed_sword;
            case "Karate":
                return e_Weapon.Karate;
            case "Glove":
                return e_Weapon.Glove;
            case "Bag":
                return e_Weapon.Bag;
            case "Bead":
                return e_Weapon.Bead;
            case "Musical_instruments":
                return e_Weapon.Musical_instruments;
            case "Dance":
                return e_Weapon.Dance;
            default:
                return e_Weapon.None;
        }
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
            InventoryManager.instance.AddItem(newItem);
            RefreshIcon();
        }
        else
        {
            Debug.Log("No item data available for random selection.");
        }
    }

    public void Refresh_Stat()
    {
        healthText.text = "HP: " + player.CurHealth + " / " + player.MaxHealth;
        attackPowerText.text = "Attack : " + player.Atk;
        speedText.text = "Speed: " + player.Speed;
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();
        InitEquipSlots();
        InitSlots();
        ItemData selectedItem = GetSelectedItem();
        Refresh_Tooltip(selectedItem);
        Refresh_Button(selectedItem);
        RefreshIcon();
    }

    private void Update()
    {
        Refresh_Stat();
        RecordItem();
    }
}
