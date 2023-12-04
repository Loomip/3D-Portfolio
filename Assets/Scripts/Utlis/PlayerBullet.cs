using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullets
{
    Player player;
    [SerializeField] LayerMask layerMask = 0;
    Transform tfTarget = null;
    float currentSpeed = 0f;

    // 타겟이 설정되었는지 여부를 나타내는 플래그
    public bool isTargetSet = false;
    float targetSetTime; // 타겟이 설정된 시간

    //유도탄
    void SearchEnemy(Vector3 bulletDirection)
    {
        Collider[] searchRange = Physics.OverlapSphere(transform.position, 5f, layerMask);

        float closestAngle = 360f; // 총알이 인식할 수 있는 최대 각도

        foreach (var collider in searchRange)
        {
            // 태그가 "Enemy"인 적 콜라이더만 처리
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
        Vector3 bulletDirection = rigid.velocity.normalized; // 총알의 방향을 구합니다

        while (true)
        {
            if (!isTargetSet)
            {
                SearchEnemy(bulletDirection);

                // 타겟이 설정되지 않았을 때 일정 시간이 지나면 파괴
                if (!isTargetSet && Time.time - targetSetTime >= 1f)
                {
                    Destroy(gameObject);
                    yield break; // 코루틴 종료
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
        targetSetTime = Time.time; // 초기화
        StartCoroutine(LaunchDelay());
        player = FindObjectOfType<Player>();
    }
}
