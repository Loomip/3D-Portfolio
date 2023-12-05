using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnamyBullet : MonoBehaviour
{
    // 무기의 공격력 값을 저장하는 변수
    public int Atk
    {
        get;
        set;
    }

    Rigidbody rigid;

    //최고 속도
    public float speed = 0f;
    //현재 속도
    float currentSpeed;
    Enemy enemy;
    Player player;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        enemy = FindObjectOfType<Enemy>();
        player = FindObjectOfType<Player>();
    }

    void Update()
    {
        if (enemy.monsterType == e_MonsterType.Range)
        {
            if (currentSpeed <= speed)
                currentSpeed += speed * Time.deltaTime;

            transform.position += transform.forward * currentSpeed * Time.deltaTime;
        }
        else if (enemy.monsterType == e_MonsterType.Boss)
        {
            Vector3 playerDirection = (player.transform.position - transform.position).normalized;

            if (currentSpeed <= speed)
                currentSpeed += speed * Time.deltaTime;

            transform.position += playerDirection * currentSpeed * Time.deltaTime;

            transform.forward = Vector3.Lerp(transform.forward, playerDirection, 0.25f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
