using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

public class TitleScene : GameManager
{
    public GameObject inven;
    public GameObject titleUI;
    private GameManager gameManager;

    //메뉴 프리펩이 들어갈 위치
    public Transform Maun;

    // 옵션 메뉴의 이름
    private const string optionMenuName = "Option";

    private GameObject currentOptionMenu;

    public GameObject Gold;

    // "School" 씬으로 전환
    public void SceneStart()
    {
        titleUI.SetActive(false);
        LoadSceneManager.LoadScene("DialogScene");
    }

    // 애플리케이션 종료
    public void Clear()
    {
        Application.Quit();
    }

    // 옵션 버튼
    public void Option()
    {
        // 현재 옵션 메뉴가 활성화된 상태라면, 비활성화합니다.
        if (currentOptionMenu != null)
        {
            Destroy(currentOptionMenu);
            currentOptionMenu = null;
        }

        else // 그렇지 않다면, 옵션 메뉴를 인스턴스화하고 활성화합니다.
        {
            string prefabPath = "Maun/" + optionMenuName;
            GameObject prefab = Resources.Load<GameObject>(prefabPath);
            if (prefab != null)
            {
                inven.SetActive(true);
                currentOptionMenu = Instantiate(prefab, Maun);
                Gold.SetActive(false);

            }
            else
            {
                Debug.Log("메뉴 유형 " + optionMenuName + "에 대한 메뉴 프리팹이 없습니다.");
            }
        }
    }

    // 이스케이프 키를 누르면 옵션 메뉴를 닫습니다.
    public override void OnInteract()
    {
        base.Init();

        SceneType = e_Scene.Title;

        titleUI.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    private void Awake()
    {
        gameManager = GameManager.instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && currentOptionMenu != null)
        {
            Destroy(currentOptionMenu);
            currentOptionMenu = null;
            inven.SetActive(false);
            Gold.SetActive(true);
        }
    }
}
