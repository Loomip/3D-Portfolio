using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPC_Shop : NPC_Base
{
    [Header("상점 UI")]
    public Data_Shop shopData;
    public GameObject shopObject;
    public GameObject shopSlotPrefab;
    public Transform shopContent;
    public TextMeshProUGUI addText;
    public TextMeshProUGUI closeText;

    public bool shopOpen = false;
    private List<GameObject> shopSlots = new List<GameObject>(); // 생성된 슬롯을 저장할 리스트
    private Data_Shop.Param selectedItem; // 현재 선택된 아이템
    private ShopSlot selectedSlot;

    // 상점이 열려있는지 확인하는 메서드
    public override bool IsShopOpen()
    {
        return shopOpen;
    }

    public void SetSelectedItem(Data_Shop.Param item)  // 선택된 아이템을 설정하는 함수
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

        addText.text = "사기";
        closeText.text = "나가기";

        // 마우스 포인터 설정 및 게임 일시정지
        if (shopOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;

            // 이전에 생성된 슬롯 제거
            foreach (GameObject slot in shopSlots)
            {
                Destroy(slot);
            }

            shopSlots.Clear();

            if (shopData != null)
            {
                // 상점 데이터의 시트를 가져옵니다.
                Data_Shop.Sheet sheet = shopData.sheets[0];

                foreach (Data_Shop.Param shopItem in sheet.list)
                {
                    // 상점 슬롯 프리팹을 복제하여 슬롯을 생성합니다.
                    GameObject shopSlotObject = Instantiate(shopSlotPrefab, shopContent);

                    // 슬롯에 상점 아이템 데이터를 설정합니다.
                    ShopSlot shopSlot = shopSlotObject.GetComponent<ShopSlot>();
                    shopSlot.SetShopData(shopItem, this);

                    // 생성된 슬롯을 리스트에 추가
                    shopSlots.Add(shopSlotObject);
                }
            }
            else
            {
                Debug.LogWarning("상점 데이터가 할당되지 않았습니다.");
            }
        }
    }

    public void Buy()
    {
        if (selectedItem != null)
        {
            // 아이템의 가격이 현재 골드보다 많은지 확인
            if (selectedItem.AddPrise <= InventoryManager.instance.gold)
            {
                // 현재 골드에서 아이템의 가격을 차감
                InventoryManager.instance.gold -= selectedItem.AddPrise;
                InventoryManager.instance.Refresh_Gold(); // 골드 UI 업데이트

                // 선택된 아이템 데이터로 새 아이템 생성
                ItemData newItem = new ItemData();
                newItem.id = selectedItem.ID;
                ++newItem.amount;

                // 새 아이템을 인벤토리에 추가
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

    //나가기 
    public void ShopClose()
    {
        shopOpen = false;
        shopObject.SetActive(shopOpen);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }
}
