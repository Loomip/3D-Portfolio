using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class School_building : GameManager
{
    public TextMeshProUGUI nameText; // 이름을 표시할 Text 컴포넌트
    public TextMeshProUGUI dialogText; // 대화창을 표시할 Text 컴포넌트
    public TextMeshProUGUI narrationText; // 나레이션을 표시할 Text 컴포넌트
    public GameObject dialogBox; // 대화창 UI
    public GameObject narrationBox; // 나레이션 UI
    public GameObject nameBox; //이름 UI

    public List<Data_Messages.Param> dialogData;
    public int currentDialogIndex = 0;

    // 대사 출력 중인지 나타내는 플래그
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
            if (isPrinting)  // 대사 출력 중이라면
            {
                // 현재 출력 중인 대사를 모두 출력하고 대사 출력을 종료
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
            else if (currentDialogIndex <= dialogData.Count)  // 대사 출력이 끝났고 다음 대사가 있다면
            {
                currentDialogIndex++;
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
                // 이름이 없는 경우 (나레이션)
                narrationBox.SetActive(true);
                nameBox.SetActive(false); 
                dialogBox.SetActive(false);
                StartCoroutine(ShowTextEffect(narrationText, dialog.Text));
                isPrinting = true;
            }
            else
            {
                // 이름이 있는 경우 (대화창)
                nameBox.SetActive(true);
                dialogBox.SetActive(true); 
                narrationBox.SetActive(false); 
                nameText.text = dialog.Name; // 이름 텍스트를 설정
                StartCoroutine(ShowTextEffect(dialogText, dialog.Text));
                isPrinting = true;
            }
        }

        if (currentDialogIndex > dialogData.Count)
        {
            LoadSceneManager.LoadScene("School");
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
