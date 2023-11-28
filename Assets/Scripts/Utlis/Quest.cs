using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{
    public GameObject questSlotPrefab; // QuestSlot 프리팹
    public Transform content; // QuestSlot을 추가할 위치

    // 퀘스트 슬롯을 추가
    public void AddQuestSlot()
    {
        GameObject slot = Instantiate(questSlotPrefab, content);
        QuestSlot questSlot = slot.GetComponent<QuestSlot>();
        questSlot.SetQuestSlot();
    }

    // 퀘스트 진행 상태를 업데이트
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

    // 모든 퀘스트를 불러와 UI에 표시
    public void LoadQuests()
    {
        // 기존에 추가된 모든 퀘스트 슬롯 삭제
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
