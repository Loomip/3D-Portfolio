using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    Image img_Frame;
    protected Image img_Icon;
    TextMeshProUGUI txt_Amount;

    private bool isSelect;

    public bool SELECT
    {
        get => isSelect;
    }
    private bool isEmpty;

    //슬롯이 비어 있는지 확인
    public bool EMPTY
    {
        get => isEmpty;
    }
    private int slotIndex;

    //슬롯의 인덱스를 반환하거나 설정하는 속성
    public int SLOTINDEX
    {
        get => slotIndex;
        set => slotIndex = value;
    }

    //아이템 클릭 이벤트
    public event Action<ItemData> onItemClick;

    //현재 슬롯의 아이템 데이터를 저장
    private ItemData currentItem;

    public ItemData GetItem()
    {
        return currentItem;
    }

    //슬롯의 UI컴포넌트를 초기화 하고 슬롯을 비움
    public void InitData()
    {
        img_Frame = transform.GetChild(0).GetComponent<Image>();
        img_Icon = transform.GetChild(1).GetComponent<Image>();
        txt_Amount = GetComponentInChildren<TextMeshProUGUI>();

        ClearSlot();
    }

    private Color iconColor;

    //슬롯의 아이콘을 설정하는 메서드
    public void Set_Icon(ItemData newItem)
    {
        if (DataManager.instance.GetItemData(newItem.id, out Data_Item.Param data))
        {
            if (data != null)
            {
                img_Icon.sprite = Resources.Load<Sprite>("Itemicons/" + data.SpritName);
                ChangeAmount(newItem.amount);
                txt_Amount.enabled = true;
                isEmpty = false;
                iconColor = img_Icon.color;
                iconColor.a = 1f;
                img_Icon.color = iconColor;
                currentItem = newItem;
            }
            else
            {
                Debug.Log("Item data is null for id: " + newItem.id);
            }
        }
        else
        {
            Debug.Log("Failed to get item data for id: " + newItem.id);
        }
    }

    //슬롯을 비우는 메서드
    public void ClearSlot()
    {
        img_Frame.enabled = false;
        isSelect = false;
        txt_Amount.enabled = false;
        isEmpty = true;
        iconColor = img_Icon.color;
        iconColor.a = 0f;
        img_Icon.color = iconColor;
    }

    //슬롯의 아이템 개수를 변경하는 메서드
    public void ChangeAmount(int newAmount)
    {
        txt_Amount.text = newAmount.ToString();
    }


    //슬롯 선택 버튼을 클릭했을 때 호출되는 메서드
    public void SelectButton()
    {
        if (!isEmpty)
        {
            isSelect = !isSelect;
            SelectSlot(isSelect);

            Equip equip = FindObjectOfType<Equip>();
            Enhance enhance = FindObjectOfType<Enhance>();
            if (equip != null)
            {
                equip.SelectSlot(this);
            }
            if(enhance != null)
            {
                enhance.SelectSlot(this);
            }
            onItemClick?.Invoke(currentItem);
        }
    }

    //슬롯의 선택 상태를 설정하는 메서드
    public void SelectSlot(bool isSelect)
    {
        img_Frame.enabled = isSelect;
    }

    private void Start()
    {
        InitData();
    }
}