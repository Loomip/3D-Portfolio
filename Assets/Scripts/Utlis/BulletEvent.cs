using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEvent : MonoBehaviour
{
    public Transform bulletPos;
    public GameObject bullet;
    public Player player;

    public void Shot()
    {
        // �Ѿ� �߻�
        GameObject intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRigid = intantBullet.GetComponent<Rigidbody>();
        Bullets bullets = intantBullet.GetComponent<Bullets>();
        bulletRigid.velocity = bulletPos.forward * 20f;
        bullets.Atk = player.stat.GetStat(e_StatType.Atk);
        SoundManager.instance.PlaySfx(e_Sfx.BulletSound);
        player.isUseSkill = false;
    }

    void Awake()
    {
        player = FindObjectOfType<Player>();
    }
}
