using System.Collections;
using UnityEngine;

public class KingSlime : Enemy
{
    // ������ ���� ����
    int numberOfPatterns = 2;
    BoxCollider collider;
    public BoxCollider TauntArea;
    Vector3 lookVec;
    Vector3 tauntVec;
    private bool isExecutingPattern2 = false;
    [SerializeField] LayerMask layerMask;
    int meleeAttackRange = 3;
    // ������ ��Ʈ�ѷ�
    private BossroomController bossroomController;

    void Start()
    {
        // "BossRoom" �±׸� ���� ���� ������Ʈ�� BossroomController ������Ʈ�� ã�� ���� ����
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
            //ó�� �����ϴ� �ð� (����� ���̵��� ������)
            yield return new WaitForSeconds(1f);

            // ���� ���� ���� ����
            // ���� ���� ���� �� ����
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
                    // �߰����� ���ϵ� �߰� ����
            }

            //���� Ȯ��
            Debug.Log($"{randomPattern} Pattern Finish");
        }
    }

    IEnumerator ExecutePattern1()
    {
        // target ������ �÷��̾� ������Ʈ�� �ʱ�ȭ
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

        // NavMeshAgent Ȱ��ȭ
        nav.enabled = true;
        nav.SetDestination(tauntVec);

        // �̵��� �Ϸ�� ������ ���
        yield return new WaitUntil(() => !nav.pathPending && nav.remainingDistance < 0.1f);

        animator.SetTrigger("isSkill_1_1");
        TauntArea.enabled = true;

        yield return new WaitForSeconds(0.5f);
        TauntArea.enabled = false;

        // NavMeshAgent ��Ȱ��ȭ
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
        base.Die(reactvec);  // Enemy Ŭ������ Die �Լ� ȣ��

        if (bossroomController != null)
        {
            bossroomController.MonsterDied();  // ������ �׾��� �� MonsterDied �Լ� ȣ��
        }
    }
}
