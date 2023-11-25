using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Base : MonoBehaviour
{
    public string npcNameCode;
    public int currentDialogIndex = 0;
    public GameManager gameManager;

    public List<Data_Messages.Param> dialogData;

    // 상점이 열려있는지 확인하는 메서드
    public virtual bool IsShopOpen()
    {
        return false;
    }

    public virtual void OnInteract()
    {
        // 상점이 열려있지 않을 때만 대화창을 엽니다.
        if (!IsShopOpen())
        {
            dialogData = DataManager.instance.GetNPCDialogData(gameManager.GetCurrentSceneIndex(), npcNameCode);
            StartCoroutine(NpcManager.instance.DialogueCoroutine(this));
        }
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

    //퀘스트 초기화 매서드
    public void ResetCurrentDialogueQuest()
    {
        if (GetCurrentDialogIndex() < dialogData.Count)
        {
            dialogData[GetCurrentDialogIndex()].Quest = 0;
        }
    }

    public void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
}
