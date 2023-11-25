using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestSlot : MonoBehaviour
{
    public TextMeshProUGUI explanationText; // ����Ʈ ������ ��Ÿ���� Text
    public TextMeshProUGUI countText; // ���� ���� ���� �� / �� ��ƾ� �ϴ� ���� ���� ��Ÿ���� Text
    public GameObject completeEffect; // ����Ʈ �Ϸ� ����Ʈ
    public TextMeshProUGUI completeText; //����Ʈ �Ϸ� Text


    // ����Ʈ ������ ����
    public void SetQuestSlot(NPC_Quest quest)
    {
        explanationText.text = DataManager.instance.GetQuestData(quest.questId).Explanation;
        int currentClearValue = QuestManager.instance.GetQuestClearValue(quest.questId);  // QuestManager���� currentClearValue�� �����ɴϴ�.
        UpdateProgressText(currentClearValue, DataManager.instance.GetQuestData(quest.questId).TargetCount);
        completeText.text = DataManager.instance.GetWordData("Complete");
    }

    // ����Ʈ ���� ���¸� ������Ʈ
    public void UpdateProgressText(int currentCount, int totalCount)
    {
        countText.text = $"{currentCount} / {totalCount}";

        if (currentCount >= totalCount)
        {
            // ����Ʈ �Ϸ� ����Ʈ ǥ��
            completeEffect.SetActive(true);
        }
    }
}
