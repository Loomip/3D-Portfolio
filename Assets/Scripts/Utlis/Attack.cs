using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public GameObject AttackPos;

    // ������ ���ݷ� ���� �����ϴ� ����
    public int Atk
    {
        get;
        set;
    }

    void OnAttack()
    {
        AttackCollider attackCollider = AttackPos.GetComponent<AttackCollider>();
        if (attackCollider != null)
        {
            attackCollider.Atk = Player.instance.stat.GetStat(e_StatType.Atk);
            AttackPos.SetActive(true);
        }
    }

    void StertTR()
    {
        foreach (var weapon in Player.instance.weapons)
        {
            if (weapon != null)
            {
                Weapons weapons = weapon.GetComponent<Weapons>();
                if (weapons != null && weapons.trailEffect != null)
                {
                    weapons.trailEffect.enabled = true;
                }
            }
        }
    }

    void EndTR()
    {
        foreach (var weapon in Player.instance.weapons)
        {
            if (weapon != null)
            {
                Weapons weapons = weapon.GetComponent<Weapons>();
                if (weapons != null && weapons.trailEffect != null)
                {
                    weapons.trailEffect.enabled = false;
                }
            }
        }
    }
}
