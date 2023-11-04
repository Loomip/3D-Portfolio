using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : SingletonDontDestroy<CharacterManager>
{
    // Player를 저장할 변수
    public Player Player;

    // 원하는 위치를 저장할 변수
    public Vector3 desiredPlayerPosition;

    protected override void DoAwake()
    {
        // Scene 전환 시 파괴되지 않도록 설정`
        DontDestroyOnLoad(Player);
    }
}
