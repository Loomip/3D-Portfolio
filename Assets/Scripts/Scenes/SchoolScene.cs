using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchoolScene : GameManager
{
    public override void Init()
    {
        base.Init();

        SceneType = e_Scene.School;

        player = FindObjectOfType<Player>();

        UpdateDialogData();
        SoundManager.instance.PlayBgm(e_Bgm.SchoolSound);

        if (player == null)
        {
            GameObject playerObject = Instantiate(playerPrefab);
            player = playerObject.GetComponent<Player>();
            Vector3 startPosition = new Vector3(0, 0, 0);
            player.transform.position = startPosition;
            DontDestroyOnLoad(player.gameObject);
        }
    }

    protected override bool ShouldCreatePlayer()
    {
        return true;
    }
}
