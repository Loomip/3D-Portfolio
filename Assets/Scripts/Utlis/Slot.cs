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

    //������ ��� �ִ��� Ȯ��
    public bool EMPTY
    {
        get => isEmpty;
    }
    private int slotIndex;

    //������ �ε����� ��ȯ�ϰų� �����ϴ� �Ӽ�
    public int SLOTINDEX
    {
        get => slotIndex;
        set => slotIndex = value;
    }

    //������ Ŭ�� �̺�Ʈ
    public event Action<ItemData> onItemClick;

    //���� ������ ������ �����͸� ����
    private ItemData currentItem;

    public ItemData GetItem()
    {
        return currentItem;
    }

    //������ UI������Ʈ�� �ʱ�ȭ �ϰ� ������ ���
    public void InitData()
    {
        img_Frame = transform.GetChild(0).GetComponent<Image>();
        img_Icon = transform.GetChild(1).GetComponent<Image>();
        txt_Amount = GetComponentInChildren<TextMeshProUGUI>();

        ClearSlot();
    }

    private Color iconColor;

    //������ �������� �����ϴ� �޼���
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

    //������ ���� �޼���
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

    //������ ������ ������ �����ϴ� �޼���
    public void ChangeAmount(int newAmount)
    {
        txt_Amount.text = newAmount.ToString();
    }


    //���� ���� ��ư�� Ŭ������ �� ȣ��Ǵ� �޼���
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

    //������ ���� ���¸� �����ϴ� �޼���
    public void SelectSlot(bool isSelect)
    {
        img_Frame.enabled = isSelect;
    }

    private void Start()
    {
        InitData();
    }
}