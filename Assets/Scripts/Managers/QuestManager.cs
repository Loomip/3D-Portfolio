using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : SingletonDontDestroy<QuestManager>
{
    //����Ʈ ID�� ���� �����͸� �������� ��ųʸ�
    private Dictionary<int, Data_Quest.Param> quests = new Dictionary<int, Data_Quest.Param>();

    //����Ʈ ID�� ���� �� ����Ʈ�� ���� ���¸� ���������� �����ϴ� ��ųʸ�
    private Dictionary<int, int> questClearValues = new Dictionary<int, int>();

    //����Ʈ�� ���� �Ǿ�����
    public bool isStartQuest = false;

    // ����Ʈ�� ��ųʸ��� �߰��ϴ� �޼���
    public void AddQuest(int questId, Data_Quest.Param questData)
    {
        if (!quests.ContainsKey(questId))
        {
            isStartQuest = true;
            quests.Add(questId, questData);
            questClearValues.Add(questId, 0);
        }
    }

    // ��� ����Ʈ�� �ҷ����� �޼���
    public Dictionary<int, Data_Quest.Param> GetAllQuests()
    {
        return quests;
    }

    private void OnEnable()
    {
        // Enemy ��� �̺�Ʈ�� �޼��带 ���
        Enemy.OnEnemyDied += OnEnemyDied;
    }

    private void OnDisable()
    {
        // Enemy ��� �̺�Ʈ���� �޼��带 ����
        Enemy.OnEnemyDied -= OnEnemyDied;
    }

    public void IncreaseClearValue(int questId)
    {
        if (questClearValues.ContainsKey(questId))
        {
            questClearValues[questId]++;
        }
    }

    //���� ����Ʈ ������Ʈ �޼���
    public void OnEnemyDied()
    {
        foreach (var questId in quests.Keys)
        {
            IncreaseClearValue(questId);
        }
    }

    // ����Ʈ�� currentClearValue�� ��ȯ�ϴ� �޼���
    public int GetQuestClearValue(int questId)
    {
        if (questClearValues.ContainsKey(questId))
        {
            return questClearValues[questId];
        }
        else
        {
            return 0;
        }
    }

    //��� ����
    public void GoldReward(int questId)
    {
        if (quests.ContainsKey(questId))
        {
            Data_Quest.Param quest = quests[questId];
            int gold = DataManager.instance.GetQuestData(questId).Reward1Amount;
            InventoryManager.instance.gold += gold;
            InventoryManager.instance.Refresh_Gold();
        }
    }

    //����Ʈ �Ϸ��� ���� ����
    public void RemoveQuest(int questId)
    {
        if (quests.ContainsKey(questId))
        {
            quests.Remove(questId);
            questClearValues.Remove(questId);
            isStartQuest = false;
        }
    }

}
