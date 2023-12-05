using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    // 무기의 공격력 값을 저장하는 변수
    public int Atk
    {
        get;
        set;
    }

    Rigidbody rigid;

    [SerializeField] LayerMask layerMask = 0;
    Transform tfTarget;
    //최고 스피드
    public float speed = 0f;
    //현재 스피드
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

                if (Time.time - targetSetTime >= 1f)
                {
                    // 타겟이 설정되지 않았을 때 일정 시간이 지나면 파괴
                    Destroy(gameObject);
                    yield break;
                }
            }
            else
            {
                yield break; // 타겟이 설정되었으므로 코루틴 종료
            }

            yield return null; // 다음 프레임까지 대기
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (tfTarget != null)
        {
            Vector3 playerDirection = (tfTarget.position - transform.position).normalized;

            if (currentSpeed <= speed)
                currentSpeed += speed * Time.deltaTime;

            transform.position += playerDirection * currentSpeed * Time.deltaTime;

            transform.forward = Vector3.Lerp(transform.forward, playerDirection, 0.25f);
        }
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        targetSetTime = Time.time; // 초기화
        StartCoroutine(LaunchDelay());
    }
}
