using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

public class School_buildingScene : GameManager
{
    public List<Data_Messages.Param> dialogData;
    public int currentDialogIndex = 0;

    // ��� ��� ������ ��Ÿ���� �÷���
    private bool isPrinting = false;

    public override void Init()
    {
        base.Init();

        SceneType = e_Scene.Schoo_Building;

        dialogData = DataManager.Inst.GetDialogData(GetCurrentSceneIndex());

        ShowDialog();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return))
        {
            if (isPrinting)  // ��� ��� ���̶��
            {
                // ���� ��� ���� ��縦 ��� ����ϰ� ��� ����� ����
                StopAllCoroutines();
                if (string.IsNullOrEmpty(dialogData[currentDialogIndex].Name))
                {
                    UIManager.Inst.talkText.text = dialogData[currentDialogIndex].Text;
                }
                isPrinting = false;
            }

            // ��ȭ�� �������� Ȯ��
            if (currentDialogIndex > dialogData.Count)
            {
                // ��ȭ�� ������ �� ��ȭâ�� ���ϴ�.
                UIManager.Inst.MessageBox.SetActive(false);
            }
            else
            {
                // ��ȭ�� ������ �ʾҴٸ� ���� ��ȭ�� ����մϴ�.
                ShowDialog();
            }
        }
    }

    void ShowDialog()
    {
        // ���� ��縦 �����ɴϴ�.
        if (currentDialogIndex < dialogData.Count)
        {
            Data_Messages.Param dialog = dialogData[currentDialogIndex];

            // �̸��� �ִ� ��� ��ȭâ�� ǥ���ϰ�, ���� ��� �����̼��� ǥ��
            if (string.IsNullOrEmpty(dialog.Name))
            {
                // �̸��� �ִ� ��� (��ȭâ)
                UIManager.Inst.MessageBox.SetActive(true);
                UIManager.Inst.talkText.text = dialog.Text;
                UIManager.Inst.nameText.text = dialog.Name; // �̸� �ؽ�Ʈ�� ����
                StartCoroutine(ShowTextEffect(UIManager.Inst.talkText, dialog.Text));
                isPrinting = true;
            }
        }
    }

    //��ȭ ����
    IEnumerator ShowTextEffect(TextMeshProUGUI textUI, string fullText)
    {
        textUI.text = "";  // �ؽ�Ʈ �ʱ�ȭ
        foreach (char letter in fullText.ToCharArray())
        {
            textUI.text += letter;  // �� ���ھ� �߰�
            yield return new WaitForSeconds(0.03f);  // ������ ������ �� ���� ���� ǥ��
        }
        isPrinting = false;
    }
}
