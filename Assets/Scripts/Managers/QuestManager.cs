using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : SingletonDontDestroy<QuestManager>
{
    // 현재 진행 중인 퀘스트 목록
    private Dictionary<int, Data_Quest.Param> activeQuests = new Dictionary<int, Data_Quest.Param>();

    // 퀘스트를 추가합니다.
    public void StartQuest(int questId)
    {
        Data_Quest.Param questData = DataManager.instance.GetQuestData(questId);
        if (questData != null)
        {
            activeQuests.Add(questId, questData);
        }
    }

    // 퀘스트 진행 상태 업데이트 메서드
    public void UpdateQuestStatus()
    {
        foreach (var quest in activeQuests.Values)
        {
            if (quest.TargetType == "Enamy")
            {
                quest.TargetCount--;
                if (quest.TargetCount <= 0)
                {
                    CompleteQuest(quest.ID);  // 퀘스트 완료 처리
                }
            }
        }
    }

    // 퀘스트를 완료합니다.
    public void CompleteQuest(int questId)
    {
        if (activeQuests.ContainsKey(questId))
        {
            // 보상을 주는 등의 추가적인 처리를 이곳에서 합니다.
            activeQuests.Remove(questId); // 퀘스트를 완료 목록에서 제거합니다.
        }
    }

    // 퀘스트의 완료 상태를 확인합니다.
    public bool IsQuestCompleted(int questId)
    {
        // 완료된 퀘스트는 activeQuests에서 제거되므로, 해당 퀘스트가 목록에 없다면 완료된 것으로 간주합니다.
        return !activeQuests.ContainsKey(questId);
    }
}
