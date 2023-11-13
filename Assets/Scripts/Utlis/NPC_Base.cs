using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Base : MonoBehaviour
{
    public string npcNameCode;
    public int currentDialogIndex = 0;
    public GameManager gameManager;

    public List<Data_Messages.Param> dialogData;

    public virtual void OnInteract()
    {
        dialogData = DataManager.instance.GetNPCDialogData(gameManager.GetCurrentSceneIndex(), npcNameCode);
        StartCoroutine(NpcManager.instance.DialogueCoroutine(this));
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

    public void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
}
