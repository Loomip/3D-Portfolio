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
