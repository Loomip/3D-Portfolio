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

    public void SetSelectedItem(Data_Shop.Param item)  // ���õ� �������� �����ϴ� �Լ�
    {
        selectedItem = item;
        Debug.Log("���� " + gameObject.name);
        Debug.Log("���õ� ������: " + (selectedItem != null ? selectedItem.Name : "null"));
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
                    //shopSlot.SetShopData(shopItem, this);

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

    private Data_Item.Param GetGameDataFromShopItem(Data_Shop.Param shopItem)
    {
        // ���� �����ۿ��� ���� ������ ID ��������
        int gameItemId = shopItem.ID;

        // DataManager �Ǵ� �ش��ϴ� Ŭ������ ���� ���� ���� ������ ������ ��������
        Data_Item.Param gameItemData = DataManager.instance.GetItemDataParams(gameItemId);

        return gameItemData;
    }

    public void Buy(Data_Shop.Param item)
    {
        if (UIManager.instance.gold >= item.AddPrise)
        {
            // �÷��̾��� ������ �������� ���ݸ�ŭ ����
            UIManager.instance.gold -= item.AddPrise;

            // ���� �����ۿ� �����ϴ� ���� ������ ������ ��������
            Data_Item.Param itemData = GetGameDataFromShopItem(item);
        }
        else
        {
            Debug.Log("Not enough gold to buy this item.");
        }

    }

    public void BuyButton()
    {
        Debug.Log("���� " + gameObject.name);
        if (selectedItem != null)
        {
            Buy(selectedItem);
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
