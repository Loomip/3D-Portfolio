using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Quest : NPC_Base
{
    public int questId;

    public void StartQuest()
    {
        Data_Quest.Param questData = DataManager.instance.GetQuestData(questId);
        QuestManager.instance.AddQuest(questId, questData);
    }

    public void ReportQuest()
    {
        // 퀘스트 완료 시 보상 지급
        QuestManager.instance.GoldReward(questId);

        // 퀘스트 제거
        QuestManager.instance.RemoveQuest(questId);

        // 퀘스트 종료
        questId = 0;
    }
}
