using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnamyBullet : MonoBehaviour
{
    // ������ ���ݷ� ���� �����ϴ� ����
    public int Atk
    {
        get;
        set;
    }

    Rigidbody rigid;

    //�ְ� �ӵ�
    public float speed = 0f;
    //���� �ӵ�
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
