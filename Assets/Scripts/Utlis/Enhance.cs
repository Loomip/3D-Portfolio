using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Enhance : Singleton<Enhance>
{
    private Data_Item.Param m_itemData = null;

    [Header("위치")]
    //아이템 슬롯들이 들어가는 위치
    public Transform enhanceContent;

    [Header("프리팹")]
    //아이템 슬롯 프리팹
    public GameObject enhanceSlotPrefab;

    [Header("버튼")]
    //사용/장착 버튼 
    public TextMeshProUGUI btn_Enhance;

    //내가 선택한 아이템
    public Slot selected = null;

    //선택된 아이템의 이미지
    public Image tooltip_Icon;




    //강화창 아이템 리스트
    private List<Slot> enhanceItemList = new List<Slot>();

    // 강화 단계별 확률
    private float[] enhanceChances = new float[] { 0.8f, 0.7f, 0.6f, 0.5f, 0.4f };

    // 강화 단계별 능력치 증가량
    private Dictionary<e_StatType, List<int>> enhanceStats = new Dictionary<e_StatType, List<int>>();

    // 강화 단계 (0 ~ 4: 0이면 1강, 4면 5강)
    private int enhanceLevel = 0;



    //실시간 아이템 업데이트
    private void ClearEnhanceItems()
    {
        foreach (var slot in enhanceItemList)
        {
            Destroy(slot.gameObject);
        }
        enhanceItemList.Clear();
    }


    private void Start()
    {
        // 강화 단계별 능력치 증가량 초기화
        enhanceStats.Add(e_StatType.HP, new List<int> { 10, 20, 30, 40, 50 });
        enhanceStats.Add(e_StatType.Atk, new List<int> { 5, 10, 15, 20, 25 });
    }
}
