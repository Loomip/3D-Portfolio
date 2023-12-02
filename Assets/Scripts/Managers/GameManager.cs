using DG.Tweening.Core.Easing;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public e_Scene SceneType { get; protected set; } = e_Scene.None;

    public List<Data_Messages.Param> dialogData { get; private set; }

    // dialogData�� �����ϴ� �޼���
    public void UpdateDialogData() 
    {
        dialogData = DataManager.instance.GetDialogData(GetCurrentSceneIndex());
    }

    //���� �ε���
    private int currentSceneIndex;

    // �ٸ� ��ũ��Ʈ���� ���� Scene �ε����� ��� ���� �޼���
    public int GetCurrentSceneIndex()
    {
        return currentSceneIndex;
    }

    public void SetSceneType(e_Scene scene)
    {
        SceneType = scene;
    }

    public GameObject eventSystemPrefab;

    public GameObject playerPrefab;
    public Player player;

    
    //UI�� �� EventSystem�� �ʿ������� ��ü�� �������� ������ ����
    public virtual void Init()
    {
        // ������ ���۵� �� ���� ���� �ε����� �����մϴ�.
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        SceneType = e_Scene.None;

        // EventSystem ��ü�� �������� ������ ����
        if (FindObjectOfType<EventSystem>() == null)
        {
            Instantiate(eventSystemPrefab);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Init();
    }


    //�÷��̾ ���� ���������� ���� bool��
    protected virtual bool ShouldCreatePlayer()
    {
        return false;
    }
}
