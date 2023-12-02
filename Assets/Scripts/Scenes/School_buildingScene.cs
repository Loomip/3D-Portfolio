using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class School_buildingScene : GameManager
{
    //��ȭ �ε���
    public int currentDialogIndex = 0;

    // ��� ��� ������ ��Ÿ���� �÷���
    private bool isPrinting = false;

    //��簡 �������� 
    public bool isEndDialog = false;


    public override void Init()
    {
        base.Init();

        SceneType = e_Scene.Schoo_Building;

        UpdateDialogData();
        SoundManager.instance.PlayBgm(e_Bgm.FightSound);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return))
        {
            if (!isPrinting && !isEndDialog)
            {
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

            UIManager.instance.Refresh_Talk(gameObject);
            UIManager.instance.talkText.text = dialog.Text;
            UIManager.instance.nameText.text = dialog.Name;
            UIManager.instance.DisableChoices();
            StartCoroutine(ShowTextEffect(UIManager.instance.talkText, dialog.Text));
            isPrinting = true;

            // ���� ��縦 ���� �ε��� ����
            currentDialogIndex++;

            // ��ȭâ �ݱ�
            if (currentDialogIndex == 4)
            {
                UIManager.instance.Close_Talk(gameObject);
                currentDialogIndex = 0;
                isEndDialog = true;
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
            yield return new WaitForSecondsRealtime(0.01f);  // ������ ������ �� ���� ���� ǥ��
        }
        isPrinting = false;
    }

    private void Start()
    {
        ShowDialog();
    }
}
