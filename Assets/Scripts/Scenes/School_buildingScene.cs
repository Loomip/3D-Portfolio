using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

public class School_buildingScene : GameManager
{
    public List<Data_Messages.Param> dialogData;
    public int currentDialogIndex = 0;

    // 대사 출력 중인지 나타내는 플래그
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
            if (isPrinting)  // 대사 출력 중이라면
            {
                // 현재 출력 중인 대사를 모두 출력하고 대사 출력을 종료
                StopAllCoroutines();
                if (string.IsNullOrEmpty(dialogData[currentDialogIndex].Name))
                {
                    UIManager.Inst.talkText.text = dialogData[currentDialogIndex].Text;
                }
                isPrinting = false;
            }

            // 대화가 끝났는지 확인
            if (currentDialogIndex > dialogData.Count)
            {
                // 대화가 끝났을 때 대화창을 끕니다.
                UIManager.Inst.MessageBox.SetActive(false);
            }
            else
            {
                // 대화가 끝나지 않았다면 다음 대화를 출력합니다.
                ShowDialog();
            }
        }
    }

    void ShowDialog()
    {
        // 현재 대사를 가져옵니다.
        if (currentDialogIndex < dialogData.Count)
        {
            Data_Messages.Param dialog = dialogData[currentDialogIndex];

            // 이름이 있는 경우 대화창을 표시하고, 없는 경우 나레이션을 표시
            if (string.IsNullOrEmpty(dialog.Name))
            {
                // 이름이 있는 경우 (대화창)
                UIManager.Inst.MessageBox.SetActive(true);
                UIManager.Inst.talkText.text = dialog.Text;
                UIManager.Inst.nameText.text = dialog.Name; // 이름 텍스트를 설정
                StartCoroutine(ShowTextEffect(UIManager.Inst.talkText, dialog.Text));
                isPrinting = true;
            }
        }
    }

    //대화 연출
    IEnumerator ShowTextEffect(TextMeshProUGUI textUI, string fullText)
    {
        textUI.text = "";  // 텍스트 초기화
        foreach (char letter in fullText.ToCharArray())
        {
            textUI.text += letter;  // 한 글자씩 추가
            yield return new WaitForSeconds(0.03f);  // 일정한 딜레이 후 다음 글자 표시
        }
        isPrinting = false;
    }
}
