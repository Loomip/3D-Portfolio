using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Base : MonoBehaviour
{
    public string npcNameCode;
    public int currentDialogIndex = 0;
    public GameManager gameManager;

    public List<Data_Messages.Param> dialogData;

    // ������ �����ִ��� Ȯ���ϴ� �޼���
    public virtual bool IsShopOpen()
    {
        return false;
    }

    public virtual void OnInteract()
    {
        // ������ �������� ���� ���� ��ȭâ�� ���ϴ�.
        if (!IsShopOpen())
        {
            dialogData = DataManager.instance.GetNPCDialogData(gameManager.GetCurrentSceneIndex(), npcNameCode);
            StartCoroutine(NpcManager.instance.DialogueCoroutine(this));
        }
    }

    // ���� ��ȭ �ε��� ���� �޼���
    public void SetCurrentDialogIndex(int index)
    {
        currentDialogIndex = index;
    }

    // ���� �������� ��ȭ �ε��� �������� �޼���
    public int GetCurrentDialogIndex()
    {
        return currentDialogIndex;
    }

    // ���� �������� ��ȭ �ε��� ���� �޼���
    public void IncrementCurrentDialogue()
    {
        if (currentDialogIndex < dialogData.Count - 1)
        {
            currentDialogIndex++;
        }
    }

    //����Ʈ �ʱ�ȭ �ż���
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
