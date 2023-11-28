using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : SingletonDontDestroy<QuestManager>
{
    //퀘스트 ID에 따른 데이터를 가져오는 딕셔너리
    private Dictionary<int, Data_Quest.Param> quests = new Dictionary<int, Data_Quest.Param>();

    //퀘스트 ID에 따른 각 퀘스트의 진행 상태를 개별적으로 관리하는 딕셔너리
    private Dictionary<int, int> questClearValues = new Dictionary<int, int>();

    //퀘스트가 시작 되었는지
    public bool isStartQuest = false;

    // 퀘스트를 딕셔너리에 추가하는 메서드
    public void AddQuest(int questId, Data_Quest.Param questData)
    {
        if (!quests.ContainsKey(questId))
        {
            isStartQuest = true;
            quests.Add(questId, questData);
            questClearValues.Add(questId, 0);
        }
    }

    // 모든 퀘스트를 불러오는 메서드
    public Dictionary<int, Data_Quest.Param> GetAllQuests()
    {
        return quests;
    }

    private void OnEnable()
    {
        // Enemy 사망 이벤트에 메서드를 등록
        Enemy.OnEnemyDied += OnEnemyDied;
    }

    private void OnDisable()
    {
        // Enemy 사망 이벤트에서 메서드를 해제
        Enemy.OnEnemyDied -= OnEnemyDied;
    }

    public void IncreaseClearValue(int questId)
    {
        if (questClearValues.ContainsKey(questId))
        {
            questClearValues[questId]++;
        }
    }

    //전투 퀘스트 업데이트 메서드
    public void OnEnemyDied()
    {
        foreach (var questId in quests.Keys)
        {
            IncreaseClearValue(questId);
        }
    }

    // 퀘스트의 currentClearValue를 반환하는 메서드
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

    //골드 보상
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

    //퀘스트 완료후 슬롯 삭제
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
