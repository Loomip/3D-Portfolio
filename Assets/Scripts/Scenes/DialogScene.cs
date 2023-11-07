using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogScene : GameManager
{
    public TextMeshProUGUI nameText; // �̸��� ǥ���� Text ������Ʈ
    public TextMeshProUGUI dialogText; // ��ȭâ�� ǥ���� Text ������Ʈ
    public TextMeshProUGUI narrationText; // �����̼��� ǥ���� Text ������Ʈ
    public GameObject dialogBox; // ��ȭâ UI
    public GameObject narrationBox; // �����̼� UI

    public List<Data_Messages.Param> dialogData;
    private int currentDialogIndex = 0;

    public override void OnInteract()
    {
        base.Init();

        SceneType = e_Scene.Dialog;

        ShowDialog();
        dialogData = DataManager.instance.GetDialogData(GetCurrentSceneIndex());
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return))
        {
            ShowDialog();
        }
    }

    void ShowDialog()
{
    // ���� ��縦 �����ɴϴ�.
    if (currentDialogIndex < dialogData.Count)
    {
        Data_Messages.Param dialog = dialogData[currentDialogIndex];

        // �̸��� �ִ� ��� ��ȭâ�� ǥ���ϰ�, ���� ��� �����̼��� ǥ���մϴ�.
        if (string.IsNullOrEmpty(dialog.Name)) // Name �ʵ尡 �ִ��� Ȯ���Ͻʽÿ�. 
        {
            // �̸��� ���� ��� (�����̼�)
            narrationBox.SetActive(true); // �����̼� �ڽ��� Ȱ��ȭ�մϴ�.
            dialogBox.SetActive(false); // ��ȭâ �ڽ��� ��Ȱ��ȭ�մϴ�.
            narrationText.text = dialog.Text; // �����̼� �ؽ�Ʈ�� �����մϴ�.
        }
        else
        {
            // �̸��� �ִ� ��� (��ȭâ)
            dialogBox.SetActive(true); // ��ȭâ �ڽ��� Ȱ��ȭ�մϴ�.
            narrationBox.SetActive(false); // �����̼� �ڽ��� ��Ȱ��ȭ�մϴ�.
            nameText.text = dialog.Name; // �̸� �ؽ�Ʈ�� �����մϴ�.
            dialogText.text = dialog.Text; // ��ȭâ �ؽ�Ʈ�� �����մϴ�.
        }

        currentDialogIndex++;
    }
    
    if (currentDialogIndex >= dialogData.Count)
    {
        SceneManager.LoadScene("School");
    }
}
}
