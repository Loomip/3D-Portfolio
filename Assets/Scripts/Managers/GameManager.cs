using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : SingletonDontDestroy<GameManager>
{
    public e_Scene SceneType { get; protected set; } = e_Scene.None;

    //���� �ε���
    private int currentSceneIndex;

    protected override void DoAwake()
    {
        // ������ ���۵� �� ���� ���� �ε����� �����մϴ�.
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    // �ٸ� ��ũ��Ʈ���� ���� Scene �ε����� ��� ���� �޼���
    public int GetCurrentSceneIndex()
    {
        return currentSceneIndex;
    }

    private void SceneStart()
    {
        LoadSceneManager.LoadScene("School");
    }

    protected virtual void Init()
    {


    }
}
