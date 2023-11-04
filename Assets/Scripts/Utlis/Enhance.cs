using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Enhance : Singleton<Enhance>
{
    private Data_Item.Param m_itemData = null;

    [Header("��ġ")]
    //������ ���Ե��� ���� ��ġ
    public Transform enhanceContent;

    [Header("������")]
    //������ ���� ������
    public GameObject enhanceSlotPrefab;

    [Header("��ư")]
    //���/���� ��ư 
    public TextMeshProUGUI btn_Enhance;

    //���� ������ ������
    public Slot selected = null;

    //���õ� �������� �̹���
    public Image tooltip_Icon;




    //��ȭâ ������ ����Ʈ
    private List<Slot> enhanceItemList = new List<Slot>();

    // ��ȭ �ܰ躰 Ȯ��
    private float[] enhanceChances = new float[] { 0.8f, 0.7f, 0.6f, 0.5f, 0.4f };

    // ��ȭ �ܰ躰 �ɷ�ġ ������
    private Dictionary<e_StatType, List<int>> enhanceStats = new Dictionary<e_StatType, List<int>>();

    // ��ȭ �ܰ� (0 ~ 4: 0�̸� 1��, 4�� 5��)
    private int enhanceLevel = 0;



    //�ǽð� ������ ������Ʈ
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
        // ��ȭ �ܰ躰 �ɷ�ġ ������ �ʱ�ȭ
        enhanceStats.Add(e_StatType.HP, new List<int> { 10, 20, 30, 40, 50 });
        enhanceStats.Add(e_StatType.Atk, new List<int> { 5, 10, 15, 20, 25 });
    }
}
