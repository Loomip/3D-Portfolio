using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TitleScene : GameManager
{
    public GameObject inven;
    public GameObject titleUI;
    public TextMeshProUGUI txt_Title;
    public TextMeshProUGUI txt_Start;
    public TextMeshProUGUI txt_Option;
    public TextMeshProUGUI txt_Exit;

    //�޴� �������� �� ��ġ
    public Transform Maun;

    // �ɼ� �޴��� �̸�
    private e_MenuType OptionMenu = e_MenuType.Option;

    private GameObject currentOptionMenu;

    public GameObject Gold;

    // "School" ������ ��ȯ
    public void SceneStart()
    {
        titleUI.SetActive(false);
        LoadSceneManager.LoadScene("DialogScene");
    }

    // ���ø����̼� ����
    public void Clear()
    {
        Application.Quit();
    }

    // �ɼ� ��ư
    public void Option()
    {
        // ���� �ɼ� �޴��� Ȱ��ȭ�� ���¶��, ��Ȱ��ȭ�մϴ�.
        if (currentOptionMenu != null)
        {
            Destroy(currentOptionMenu);
            currentOptionMenu = null;
        }

        else // �׷��� �ʴٸ�, �ɼ� �޴��� �ν��Ͻ�ȭ�ϰ� Ȱ��ȭ�մϴ�.
        {
            string prefabPath = "Maun/" + OptionMenu;
            GameObject prefab = Resources.Load<GameObject>(prefabPath);
            if (prefab != null)
            {
                inven.SetActive(true);
                currentOptionMenu = Instantiate(prefab, Maun);
                Gold.SetActive(false);

            }
            else
            {
                Debug.Log("�޴� ���� " + OptionMenu + "�� ���� �޴� �������� �����ϴ�.");
            }
        }
    }

    // �̽������� Ű�� ������ �ɼ� �޴��� �ݽ��ϴ�.
    public override void Init()
    {
        base.Init();

        SceneType = e_Scene.Title;

        titleUI.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        txt_Title.text = DataManager.instance.GetWordData("Title");
        txt_Start.text = DataManager.instance.GetWordData("Start");
        txt_Option.text = DataManager.instance.GetWordData("Option");
        txt_Exit.text = DataManager.instance.GetWordData("Exit");
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
