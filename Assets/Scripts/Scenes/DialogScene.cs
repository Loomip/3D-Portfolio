using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogScene : GameManager
{
    public TextMeshProUGUI nameText; // 이름을 표시할 Text 컴포넌트
    public TextMeshProUGUI dialogText; // 대화창을 표시할 Text 컴포넌트
    public TextMeshProUGUI narrationText; // 나레이션을 표시할 Text 컴포넌트
    public GameObject dialogBox; // 대화창 UI
    public GameObject narrationBox; // 나레이션 UI

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
    // 현재 대사를 가져옵니다.
    if (currentDialogIndex < dialogData.Count)
    {
        Data_Messages.Param dialog = dialogData[currentDialogIndex];

        // 이름이 있는 경우 대화창을 표시하고, 없는 경우 나레이션을 표시합니다.
        if (string.IsNullOrEmpty(dialog.Name)) // Name 필드가 있는지 확인하십시오. 
        {
            // 이름이 없는 경우 (나레이션)
            narrationBox.SetActive(true); // 나레이션 박스를 활성화합니다.
            dialogBox.SetActive(false); // 대화창 박스를 비활성화합니다.
            narrationText.text = dialog.Text; // 나레이션 텍스트를 설정합니다.
        }
        else
        {
            // 이름이 있는 경우 (대화창)
            dialogBox.SetActive(true); // 대화창 박스를 활성화합니다.
            narrationBox.SetActive(false); // 나레이션 박스를 비활성화합니다.
            nameText.text = dialog.Name; // 이름 텍스트를 설정합니다.
            dialogText.text = dialog.Text; // 대화창 텍스트를 설정합니다.
        }

        currentDialogIndex++;
    }
    
    if (currentDialogIndex >= dialogData.Count)
    {
        SceneManager.LoadScene("School");
    }
}
}
