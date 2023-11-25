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
        int currentClearValue = QuestManager.instance.GetQuestClearValue(questId);  // QuestManager���� currentClearValue�� �����ɴϴ�.
        if (currentClearValue == DataManager.instance.GetQuestData(questId).TargetCount)
        {
            // ����Ʈ �Ϸ� �� ���� ����
            QuestManager.instance.GoldReward(questId);

            // ����Ʈ ����
            questId = 0;
        }
    }
}
