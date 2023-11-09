using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class School_building : GameManager
{
    public TextMeshProUGUI nameText; // �̸��� ǥ���� Text ������Ʈ
    public TextMeshProUGUI dialogText; // ��ȭâ�� ǥ���� Text ������Ʈ
    public TextMeshProUGUI narrationText; // �����̼��� ǥ���� Text ������Ʈ
    public GameObject dialogBox; // ��ȭâ UI
    public GameObject narrationBox; // �����̼� UI
    public GameObject nameBox; //�̸� UI

    public List<Data_Messages.Param> dialogData;
    public int currentDialogIndex = 0;

    // ��� ��� ������ ��Ÿ���� �÷���
    private bool isPrinting = false;  

    public override void Init()
    {
        base.Init();

        SceneType = e_Scene.Dialog;

        dialogData = DataManager.Inst.GetDialogData(GetCurrentSceneIndex());

        ShowDialog();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return))
        {
            if (isPrinting)  // ��� ��� ���̶��
            {
                // ���� ��� ���� ��縦 ��� ����ϰ� ��� ����� ����
                StopAllCoroutines();
                if (string.IsNullOrEmpty(dialogData[currentDialogIndex].Name))
                {
                    narrationText.text = dialogData[currentDialogIndex].Text;
                }
                else
                {
                    dialogText.text = dialogData[currentDialogIndex].Text;
                }
                isPrinting = false;
            }
            else if (currentDialogIndex <= dialogData.Count)  // ��� ����� ������ ���� ��簡 �ִٸ�
            {
                currentDialogIndex++;
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
                // �̸��� ���� ��� (�����̼�)
                narrationBox.SetActive(true);
                nameBox.SetActive(false); 
                dialogBox.SetActive(false);
                StartCoroutine(ShowTextEffect(narrationText, dialog.Text));
                isPrinting = true;
            }
            else
            {
                // �̸��� �ִ� ��� (��ȭâ)
                nameBox.SetActive(true);
                dialogBox.SetActive(true); 
                narrationBox.SetActive(false); 
                nameText.text = dialog.Name; // �̸� �ؽ�Ʈ�� ����
                StartCoroutine(ShowTextEffect(dialogText, dialog.Text));
                isPrinting = true;
            }
        }

        if (currentDialogIndex > dialogData.Count)
        {
            LoadSceneManager.LoadScene("School");
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
