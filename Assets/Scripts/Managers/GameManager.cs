using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class GameData
{
    public Player player;

    public GameData(Player player)
    {
        this.player = player;
    }
}

public class GameManager : SingletonDontDestroy<GameManager>
{
    public e_Scene SceneType { get; protected set; } = e_Scene.None;

    public void SetSceneType(e_Scene scene)
    {
        SceneType = scene;
    }

    public GameObject eventSystemPrefab;

    //씬의 인덱스
    private int currentSceneIndex;

    public GameObject playerPrefab;
    public Player player;

    // 다른 스크립트에서 현재 Scene 인덱스를 얻기 위한 메서드
    public int GetCurrentSceneIndex()
    {
        return currentSceneIndex;
    }

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

    public void SaveGame()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "/save.dat", FileMode.Create);

        GameData data = new GameData(player);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public void LoadGame()
    {
        string path = Application.persistentDataPath + "/save.dat";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameData data = formatter.Deserialize(stream) as GameData;
            stream.Close();

            player = data.player;
        }
        else
        {
            Debug.LogWarning("Save file not found in " + path);
        }
    }
}
