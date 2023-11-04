using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Base : MonoBehaviour
{
    public string npcNameCode;
    public int currentDialogIndex = 0;

    public List<Data_Messages.Param> dialogData;

    public virtual void OnInteract()
    {
        dialogData = DataManager.instance.GetDialogData(GameManager.instance.GetCurrentSceneIndex(), npcNameCode);
    }

    // 현재 대화 인덱스 설정 메서드
    public void SetCurrentDialogIndex(int index)
    {
        currentDialogIndex = index;
    }

    // 현재 진행중인 대화 인덱스 가져오기 메서드
    public int GetCurrentDialogIndex()
    {
        return currentDialogIndex;
    }

    // 현재 진행중인 대화 인덱스 증가 메서드
    public void IncrementCurrentDialogue()
    {
        if (currentDialogIndex < dialogData.Count - 1)
        {
            currentDialogIndex++;
        }
    }
}
