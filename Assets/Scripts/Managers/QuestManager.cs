using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : SingletonDontDestroy<QuestManager>
{
    // ���� ���� ���� ����Ʈ ���
    private Dictionary<int, Data_Quest.Param> activeQuests = new Dictionary<int, Data_Quest.Param>();

    // ����Ʈ�� �߰��մϴ�.
    public void StartQuest(int questId)
    {
        Data_Quest.Param questData = DataManager.instance.GetQuestData(questId);
        if (questData != null)
        {
            activeQuests.Add(questId, questData);
        }
    }

    // ����Ʈ ���� ���� ������Ʈ �޼���
    public void UpdateQuestStatus()
    {
        foreach (var quest in activeQuests.Values)
        {
            if (quest.TargetType == "Enamy")
            {
                quest.TargetCount--;
                if (quest.TargetCount <= 0)
                {
                    CompleteQuest(quest.ID);  // ����Ʈ �Ϸ� ó��
                }
            }
        }
    }

    // ����Ʈ�� �Ϸ��մϴ�.
    public void CompleteQuest(int questId)
    {
        if (activeQuests.ContainsKey(questId))
        {
            // ������ �ִ� ���� �߰����� ó���� �̰����� �մϴ�.
            activeQuests.Remove(questId); // ����Ʈ�� �Ϸ� ��Ͽ��� �����մϴ�.
        }
    }

    // ����Ʈ�� �Ϸ� ���¸� Ȯ���մϴ�.
    public bool IsQuestCompleted(int questId)
    {
        // �Ϸ�� ����Ʈ�� activeQuests���� ���ŵǹǷ�, �ش� ����Ʈ�� ��Ͽ� ���ٸ� �Ϸ�� ������ �����մϴ�.
        return !activeQuests.ContainsKey(questId);
    }
}
