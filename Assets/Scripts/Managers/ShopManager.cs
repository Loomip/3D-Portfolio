using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public Data_Shop shopData;
    public GameObject shopObject;
    public GameObject shopSlotPrefab;
    public Transform content;
    public TextMeshProUGUI addText;
    public TextMeshProUGUI closeText;

    public bool shopOpen = false;
    private List<GameObject> shopSlots = new List<GameObject>(); // 생성된 슬롯을 저장할 리스트
    private Data_Shop.Param selectedItem; // 현재 선택된 아이템
}
