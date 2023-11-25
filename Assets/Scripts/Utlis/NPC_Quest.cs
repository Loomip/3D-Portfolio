using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Quest : NPC_Base
{
    private Data_Quest.Param quest;
    public int questId;


    public void StartQuest()
    {
        Data_Quest.Param questData = DataManager.instance.GetQuestData(questId);
        QuestManager.instance.AddQuest(questId, questData);
    }

    public void ReportQuest()
    {
        int currentClearValue = QuestManager.instance.GetQuestClearValue(questId);  // QuestManager에서 currentClearValue를 가져옵니다.
        if (currentClearValue == DataManager.instance.GetQuestData(questId).TargetCount)
        {
            // 퀘스트 완료 시 보상 지급
            QuestManager.instance.GoldReward(questId);

            // 퀘스트 종료
            questId = 0;
        }
    }
}
