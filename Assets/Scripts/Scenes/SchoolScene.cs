using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchoolScene : GameManager
{
    public void ChangeScene()
    {
        SaveGame();
    }

    public override void Init()
    {
        base.Init();

        SceneType = e_Scene.School;

        GameObject playerObject = Instantiate(playerPrefab);
        player = playerObject.GetComponent<Player>();
        Vector3 startPosition = new Vector3(0, 0, 0);
        player.transform.position = startPosition;
    }
}
