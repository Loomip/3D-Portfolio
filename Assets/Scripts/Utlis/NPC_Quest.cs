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
        // ����Ʈ �Ϸ� �� ���� ����
        QuestManager.instance.GoldReward(questId);

        // ����Ʈ ����
        QuestManager.instance.RemoveQuest(questId);

        // ����Ʈ ����
        questId = 0;
    }
}
