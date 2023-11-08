using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : SingletonDontDestroy<GameManager>
{
    public e_Scene SceneType { get; protected set; } = e_Scene.None;

    public GameObject eventSystemPrefab;

    //씬의 인덱스
    private int currentSceneIndex;

    public virtual void OnInteract()
    {
        Init();

        // 게임이 시작될 때 현재 씬의 인덱스를 설정합니다.
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    // 다른 스크립트에서 현재 Scene 인덱스를 얻기 위한 메서드
    public int GetCurrentSceneIndex()
    {
        return currentSceneIndex;
    }

    //UI는 꼭 EventSystem이 필요함으로 객체가 존재하지 않으면 생성
    protected virtual void Init()
    {
        SceneType = e_Scene.None;
        // EventSystem 객체가 존재하지 않으면 생성
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
        OnInteract();
    }
}
