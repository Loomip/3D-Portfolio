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

    //�޴� �������� �� ��ġ
    public Transform Maun;

    // �ɼ� �޴��� �̸�
    private const string optionMenuName = "Option";

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
                Debug.Log("�޴� ���� " + optionMenuName + "�� ���� �޴� �������� �����ϴ�.");
            }
        }
    }

    // �̽������� Ű�� ������ �ɼ� �޴��� �ݽ��ϴ�.
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
