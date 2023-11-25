using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPC_Shop : NPC_Base
{
    [Header("���� UI")]
    public Data_Shop shopData;
    public GameObject shopObject;
    public GameObject shopSlotPrefab;
    public Transform shopContent;
    public TextMeshProUGUI addText;
    public TextMeshProUGUI closeText;

    public bool shopOpen = false;
    private List<GameObject> shopSlots = new List<GameObject>(); // ������ ������ ������ ����Ʈ
    private Data_Shop.Param selectedItem; // ���� ���õ� ������
    private ShopSlot selectedSlot;

    // ������ �����ִ��� Ȯ���ϴ� �޼���
    public override bool IsShopOpen()
    {
        return shopOpen;
    }

    public void SetSelectedItem(Data_Shop.Param item)  // ���õ� �������� �����ϴ� �Լ�
    {
        if (selectedSlot != null)
        {
            selectedSlot.Deselect();
        }

        selectedItem = item;
        selectedSlot = shopSlots.Find(slot => slot.GetComponent<ShopSlot>().shopData == item).GetComponent<ShopSlot>();

        if (selectedSlot != null)
        {
            selectedSlot.Select();
        }
    }

    public override void OnInteract()
    {
        shopOpen = true;
        shopObject.SetActive(shopOpen);

        addText.text = "���";
        closeText.text = "������";

        // ���콺 ������ ���� �� ���� �Ͻ�����
        if (shopOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;

            // ������ ������ ���� ����
            foreach (GameObject slot in shopSlots)
            {
                Destroy(slot);
            }

            shopSlots.Clear();

            if (shopData != null)
            {
                // ���� �������� ��Ʈ�� �����ɴϴ�.
                Data_Shop.Sheet sheet = shopData.sheets[0];

                foreach (Data_Shop.Param shopItem in sheet.list)
                {
                    // ���� ���� �������� �����Ͽ� ������ �����մϴ�.
                    GameObject shopSlotObject = Instantiate(shopSlotPrefab, shopContent);

                    // ���Կ� ���� ������ �����͸� �����մϴ�.
                    ShopSlot shopSlot = shopSlotObject.GetComponent<ShopSlot>();
                    shopSlot.SetShopData(shopItem, this);

                    // ������ ������ ����Ʈ�� �߰�
                    shopSlots.Add(shopSlotObject);
                }
            }
            else
            {
                Debug.LogWarning("���� �����Ͱ� �Ҵ���� �ʾҽ��ϴ�.");
            }
        }
    }

    public void Buy()
    {
        if (selectedItem != null)
        {
            // �������� ������ ���� ��庸�� ������ Ȯ��
            if (selectedItem.AddPrise <= InventoryManager.instance.gold)
            {
                // ���� ��忡�� �������� ������ ����
                InventoryManager.instance.gold -= selectedItem.AddPrise;
                InventoryManager.instance.Refresh_Gold(); // ��� UI ������Ʈ

                // ���õ� ������ �����ͷ� �� ������ ����
                ItemData newItem = new ItemData();
                newItem.id = selectedItem.ID;
                ++newItem.amount;

                // �� �������� �κ��丮�� �߰�
                InventoryManager.instance.AddItem(newItem);
            }
            else
            {
                Debug.Log("Not enough gold.");
            }
        }
        else
        {
            Debug.Log("No item selected.");
        }
    }

    //������ 
    public void ShopClose()
    {
        shopOpen = false;
        shopObject.SetActive(shopOpen);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }
}
