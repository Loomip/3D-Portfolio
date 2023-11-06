using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : SingletonDontDestroy<GameManager>
{
    public e_Scene SceneType { get; protected set; } = e_Scene.None;

    //씬의 인덱스
    private int currentSceneIndex;

    protected override void DoAwake()
    {
        // 게임이 시작될 때 현재 씬의 인덱스를 설정합니다.
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    // 다른 스크립트에서 현재 Scene 인덱스를 얻기 위한 메서드
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
