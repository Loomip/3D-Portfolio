using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class School_buildingScene : GameManager
{
    //대화 인덱스
    public int currentDialogIndex = 0;

    // 대사 출력 중인지 나타내는 플래그
    private bool isPrinting = false;

    //대사가 끝낫는지 
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
        // 현재 대사를 가져옵니다.
        if (currentDialogIndex < dialogData.Count)
        {
            Data_Messages.Param dialog = dialogData[currentDialogIndex];

            UIManager.instance.Refresh_Talk(gameObject);
            UIManager.instance.talkText.text = dialog.Text;
            UIManager.instance.nameText.text = dialog.Name;
            UIManager.instance.DisableChoices();
            StartCoroutine(ShowTextEffect(UIManager.instance.talkText, dialog.Text));
            isPrinting = true;

            // 다음 대사를 위해 인덱스 증가
            currentDialogIndex++;

            // 대화창 닫기
            if (currentDialogIndex == 4)
            {
                UIManager.instance.Close_Talk(gameObject);
                currentDialogIndex = 0;
                isEndDialog = true;
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
            yield return new WaitForSecondsRealtime(0.01f);  // 일정한 딜레이 후 다음 글자 표시
        }
        isPrinting = false;
    }

    private void Start()
    {
        ShowDialog();
    }
}
