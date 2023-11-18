using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class School_buildingScene : GameManager
{
    //��ȭ �ε���
    private int currentDialogIndex = 0;

    // ��� ��� ������ ��Ÿ���� �÷���
    private bool isPrinting = false;

    public override void Init()
    {
        base.Init();

        SceneType = e_Scene.Schoo_Building;

        UpdateDialogData();

        ShowDialog();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return))
        {
            if (!isPrinting)
            {
                ShowDialog();
            }
        }
    }

    void ShowDialog()
    {
        // 3��° ��ȭ�� ������ �޼��带 ����
        if (currentDialogIndex == 3)
        {
            UIManager.instance.Close_Talk();
            return;
        }

        // ���� ��縦 �����ɴϴ�.
        if (currentDialogIndex < dialogData.Count)
        {
            Data_Messages.Param dialog = dialogData[currentDialogIndex];

            UIManager.instance.Refresh_Talk(gameObject);
            UIManager.instance.talkText.text = dialog.Text;
            UIManager.instance.nameText.text = dialog.Name;
            UIManager.instance.DisableChoices();
            StartCoroutine(ShowTextEffect(UIManager.instance.talkText, dialog.Text));
            isPrinting = true;

            // ���� ��縦 ���� �ε��� ����
            currentDialogIndex++;
        }
        else
        {
            UIManager.instance.Close_Talk();
        }
    }

    //��ȭ ����
    IEnumerator ShowTextEffect(TextMeshProUGUI textUI, string fullText)
    {
        textUI.text = "";  // �ؽ�Ʈ �ʱ�ȭ
        foreach (char letter in fullText.ToCharArray())
        {
            textUI.text += letter;  // �� ���ھ� �߰�
            yield return new WaitForSecondsRealtime(0.01f);  // ������ ������ �� ���� ���� ǥ��
        }
        isPrinting = false;
    }
}
