using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : SingletonDontDestroy<CharacterManager>
{
    // Player�� ������ ����
    public Player Player;

    // ���ϴ� ��ġ�� ������ ����
    public Vector3 desiredPlayerPosition;

    protected override void DoAwake()
    {
        // Scene ��ȯ �� �ı����� �ʵ��� ����`
        DontDestroyOnLoad(Player);
    }
}
