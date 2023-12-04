using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullets
{
    Player player;
    [SerializeField] LayerMask layerMask = 0;
    Transform tfTarget = null;
    float currentSpeed = 0f;

    // Ÿ���� �����Ǿ����� ���θ� ��Ÿ���� �÷���
    public bool isTargetSet = false;
    float targetSetTime; // Ÿ���� ������ �ð�

    //����ź
    void SearchEnemy(Vector3 bulletDirection)
    {
        Collider[] searchRange = Physics.OverlapSphere(transform.position, 5f, layerMask);

        float closestAngle = 360f; // �Ѿ��� �ν��� �� �ִ� �ִ� ����

        foreach (var collider in searchRange)
        {
            // �±װ� "Enemy"�� �� �ݶ��̴��� ó��
            if (collider.CompareTag("Enemy"))
            {
                Vector3 toTarget = collider.transform.position - transform.position;
                float angle = Vector3.Angle(bulletDirection, toTarget);

                if (angle < closestAngle)
                {
                    closestAngle = angle;
                    tfTarget = collider.transform;
                    isTargetSet = true;
                    targetSetTime = Time.time;
                    break;
                }
            }
        }
    }

    IEnumerator LaunchDelay()
    {
        yield return new WaitForSeconds(0.1f);
        Vector3 bulletDirection = rigid.velocity.normalized; // �Ѿ��� ������ ���մϴ�

        while (true)
        {
            if (!isTargetSet)
            {
                SearchEnemy(bulletDirection);

                // Ÿ���� �������� �ʾ��� �� ���� �ð��� ������ �ı�
                if (!isTargetSet && Time.time - targetSetTime >= 1f)
                {
                    Destroy(gameObject);
                    yield break; // �ڷ�ƾ ����
                }
            }

        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.transform.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        Vector3 playerDirection = (player.transform.position - transform.position).normalized;

        if (currentSpeed <= speed)
            currentSpeed += speed * Time.deltaTime;

        transform.position += playerDirection * currentSpeed * Time.deltaTime;

        transform.forward = Vector3.Lerp(transform.forward, playerDirection, 0.25f);
    }

    protected override void Awake()
    {
        targetSetTime = Time.time; // �ʱ�ȭ
        StartCoroutine(LaunchDelay());
        player = FindObjectOfType<Player>();
    }
}
