using System.Collections;
using UnityEngine;

public class KingSlime : Enemy
{
    // 보스의 패턴 개수
    int numberOfPatterns = 2;
    BoxCollider collider;
    public BoxCollider TauntArea;
    Vector3 lookVec;
    Vector3 tauntVec;
    private bool isExecutingPattern2 = false;
    [SerializeField] LayerMask layerMask;
    int meleeAttackRange = 3;
    // 보스룸 컨트롤러
    private BossroomController bossroomController;

    void Start()
    {
        // "BossRoom" 태그를 가진 게임 오브젝트의 BossroomController 컴포넌트를 찾아 참조 설정
        bossroomController = GameObject.FindGameObjectWithTag("BossRoom").GetComponent<BossroomController>();
        Init();
        StartCoroutine(Think());
        nav.enabled = true;
    }

    private void Update()
    {
        if(isExecutingPattern2)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            lookVec = new Vector3(h, 0, v) * 3f;
        }
    }

    IEnumerator Think()
    {
        while (isActiveAndEnabled)
        {
            //처음 생각하는 시간 (길수록 난이도가 쉬워짐)
            yield return new WaitForSeconds(1f);

            // 보스 동작 관련 로직
            // 패턴 랜덤 선택 및 실행
            int randomPattern = Random.Range(0, numberOfPatterns);
            switch (randomPattern)
            {
                case 0:
                    if (Vector3.Distance(transform.position, player.transform.position) < meleeAttackRange)
                    {
                        yield return ExecutePattern1();
                    }
                    break;
                case 1:
                    if (Vector3.Distance(transform.position, player.transform.position) > meleeAttackRange)
                    {
                        yield return ExecutePattern2();
                    }
                    break;
                    // 추가적인 패턴들 추가 가능
            }

            //패턴 확인
            Debug.Log($"{randomPattern} Pattern Finish");
        }
    }

    IEnumerator ExecutePattern1()
    {
        // target 변수를 플레이어 오브젝트로 초기화
        target = GameObject.FindGameObjectWithTag("Player").transform;

        transform.LookAt(target);
        
        animator.SetTrigger("isAttack");

        meleeArea.GetComponent<Attack>().Atk = atk;

        yield return new WaitForSeconds(0.5f);
        meleeArea.enabled = true;

        yield return new WaitForSeconds(1f);
        meleeArea.enabled = false;

    }

    IEnumerator ExecutePattern2()
    {
        isExecutingPattern2 = true;
        collider.enabled = false;
        animator.SetTrigger("isSkill_1");

        TauntArea.GetComponent<Attack>().Atk = atk;

        Vector3 offset = new Vector3(1.5f, 0f, 1.5f);
        tauntVec = player.transform.position + lookVec + offset;

        // NavMeshAgent 활성화
        nav.enabled = true;
        nav.SetDestination(tauntVec);

        // 이동이 완료될 때까지 대기
        yield return new WaitUntil(() => !nav.pathPending && nav.remainingDistance < 0.1f);

        animator.SetTrigger("isSkill_1_1");
        TauntArea.enabled = true;

        yield return new WaitForSeconds(0.5f);
        TauntArea.enabled = false;

        // NavMeshAgent 비활성화
        nav.enabled = false;

        collider.enabled = true;
        isExecutingPattern2 = false;
    }

    public override void Init()
    {
        base.Init();
        collider = GetComponent<BoxCollider>();
        nav.enabled = false;
    }

    protected override void Die(Vector3 reactvec)
    {
        base.Die(reactvec);  // Enemy 클래스의 Die 함수 호출

        if (bossroomController != null)
        {
            bossroomController.MonsterDied();  // 보스가 죽었을 때 MonsterDied 함수 호출
        }
    }
}
