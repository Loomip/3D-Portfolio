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

    public void SetSelectedItem(Data_Shop.Param item)  // 선택된 아이템을 설정하는 함수
    {
        selectedItem = item;
        Debug.Log("선택 " + gameObject.name);
        Debug.Log("선택된 아이템: " + (selectedItem != null ? selectedItem.Name : "null"));
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
                    //shopSlot.SetShopData(shopItem, this);

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

    private Data_Item.Param GetGameDataFromShopItem(Data_Shop.Param shopItem)
    {
        // 상점 아이템에서 게임 아이템 ID 가져오기
        int gameItemId = shopItem.ID;

        // DataManager 또는 해당하는 클래스를 통해 실제 게임 아이템 데이터 가져오기
        Data_Item.Param gameItemData = DataManager.instance.GetItemDataParams(gameItemId);

        return gameItemData;
    }

    public void Buy(Data_Shop.Param item)
    {
        if (UIManager.instance.gold >= item.AddPrise)
        {
            // 플레이어의 돈에서 아이템의 가격만큼 빼기
            UIManager.instance.gold -= item.AddPrise;

            // 상점 아이템에 대응하는 게임 아이템 데이터 가져오기
            Data_Item.Param itemData = GetGameDataFromShopItem(item);
        }
        else
        {
            Debug.Log("Not enough gold to buy this item.");
        }

    }

    public void BuyButton()
    {
        Debug.Log("구매 " + gameObject.name);
        if (selectedItem != null)
        {
            Buy(selectedItem);
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
