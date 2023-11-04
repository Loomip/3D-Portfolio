using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{ 
    // ������ ���ݷ� ���� �����ϴ� ����
    public int Atk
    {
        get;
        set;
    }

    Rigidbody rigid = null;
    Transform tfTarget = null;
    [SerializeField] float speed = 0f;
    float currentSpeed = 0f;
    [SerializeField] LayerMask layerMask = 0;

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

            else
            {
                // ���Ͱ� �÷��̾ ���� ����ź�� �߻��ϴ� ���� �߰�
                Vector3 playerDirection = (Player.instance.transform.position - transform.position).normalized;

                if (currentSpeed <= speed)
                    currentSpeed += speed * Time.deltaTime;

                transform.position += playerDirection * currentSpeed * Time.deltaTime;

                transform.forward = Vector3.Lerp(transform.forward, playerDirection, 0.25f);
            }
            yield return null;
        }
    }

    

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        targetSetTime = Time.time; // �ʱ�ȭ
        StartCoroutine(LaunchDelay());
    }

    private void Update()
    {
        if (tfTarget != null)
        {
            Vector3 targetDirection = (tfTarget.position - transform.position).normalized;

            if (currentSpeed <= speed)
                currentSpeed += speed * Time.deltaTime;

            transform.position += targetDirection * currentSpeed * Time.deltaTime;

            transform.forward = Vector3.Lerp(transform.forward, targetDirection, 0.25f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }

        else if(other.transform.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}

