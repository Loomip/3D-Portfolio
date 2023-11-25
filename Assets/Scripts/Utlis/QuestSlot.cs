using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestSlot : MonoBehaviour
{
    public TextMeshProUGUI explanationText; // 퀘스트 설명을 나타내는 Text
    public TextMeshProUGUI countText; // 현재 잡은 몬스터 수 / 총 잡아야 하는 몬스터 수를 나타내는 Text
    public GameObject completeEffect; // 퀘스트 완료 이펙트
    public TextMeshProUGUI completeText; //퀘스트 완료 Text


    // 퀘스트 슬롯을 설정
    public void SetQuestSlot(NPC_Quest quest)
    {
        explanationText.text = DataManager.instance.GetQuestData(quest.questId).Explanation;
        int currentClearValue = QuestManager.instance.GetQuestClearValue(quest.questId);  // QuestManager에서 currentClearValue를 가져옵니다.
        UpdateProgressText(currentClearValue, DataManager.instance.GetQuestData(quest.questId).TargetCount);
        completeText.text = DataManager.instance.GetWordData("Complete");
    }

    // 퀘스트 진행 상태를 업데이트
    public void UpdateProgressText(int currentCount, int totalCount)
    {
        countText.text = $"{currentCount} / {totalCount}";

        if (currentCount >= totalCount)
        {
            // 퀘스트 완료 이펙트 표시
            completeEffect.SetActive(true);
        }
    }
}
