using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonDontDestroy<GameManager>
{


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
        if (Input.GetKeyDown(KeyCode.Escape))
            LoadSceneManager.LoadScene("School");
    }
}
