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

    // dialogData를 갱신하는 메서드
    public void UpdateDialogData() 
    {
        dialogData = DataManager.instance.GetDialogData(GetCurrentSceneIndex());
    }

    //씬의 인덱스
    private int currentSceneIndex;

    // 다른 스크립트에서 현재 Scene 인덱스를 얻기 위한 메서드
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

    
    //UI는 꼭 EventSystem이 필요함으로 객체가 존재하지 않으면 생성
    public virtual void Init()
    {
        // 게임이 시작될 때 현재 씬의 인덱스를 설정합니다.
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

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
        Init();
    }


    //플레이어를 언제 생성할지에 대한 bool값
    protected virtual bool ShouldCreatePlayer()
    {
        return false;
    }
}
