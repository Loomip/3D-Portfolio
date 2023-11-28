using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{
    public GameObject questSlotPrefab; // QuestSlot ������
    public Transform content; // QuestSlot�� �߰��� ��ġ

    // ����Ʈ ������ �߰�
    public void AddQuestSlot()
    {
        GameObject slot = Instantiate(questSlotPrefab, content);
        QuestSlot questSlot = slot.GetComponent<QuestSlot>();
        questSlot.SetQuestSlot();
    }

    // ����Ʈ ���� ���¸� ������Ʈ
    public void UpdateQuestSlot(NPC_Quest quest)
    {
        foreach (Transform child in content)
        {
            QuestSlot questSlot = child.GetComponent<QuestSlot>();
            if (questSlot != null)
            {
                int currentClearValue = QuestManager.instance.GetQuestClearValue(quest.questId);
                questSlot.UpdateProgressText(currentClearValue, DataManager.instance.GetQuestData(quest.questId).TargetCount);
            }
        }
    }

    // ��� ����Ʈ�� �ҷ��� UI�� ǥ��
    public void LoadQuests()
    {
        // ������ �߰��� ��� ����Ʈ ���� ����
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        Dictionary<int, Data_Quest.Param> quests = QuestManager.instance.GetAllQuests();
        foreach (KeyValuePair<int, Data_Quest.Param> quest in quests)
        {
            if (QuestManager.instance.isStartQuest)
            {
                AddQuestSlot();
            }
        }
    }

    private void OnEnable()
    {
        LoadQuests();
    }
}
