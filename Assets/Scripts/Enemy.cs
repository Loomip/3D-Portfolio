using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    //����
    public Rigidbody rigid;
    public Animator animator;
    public Stat stat;
    public SkinnedMeshRenderer[] meshs;
    public NavMeshAgent nav;
    public Transform target;
    public e_MonsterType monsterType;
    public Collider meleeArea;
    public Transform bulletPos;
    public GameObject rangeBullet;
    private ClassroomController classroomController;
    public Player player;

    //======================================================================================
    //1. ���� (����)
    public string Name;

    public int maxHealth
        => stat.GetStat(e_StatType.MHP);
    public int curHealth
    {
        get => stat.GetStat(e_StatType.HP);
        set => stat.SetStat(e_StatType.HP, value);
    }
    public int atk
        => stat.GetStat(e_StatType.Atk);
    public int def
        => stat.GetStat(e_StatType.Def);
    public int speed
        => stat.GetStat(e_StatType.Spd);
    public int acc
        => stat.GetStat(e_StatType.Acc);
    public int del
        => stat.GetStat(e_StatType.Del);


    //======================================================================================

    //2. �ν� ����

    //�������� ���� ��ġ
    private Vector3 randomDestination;
    //�̵�������
    private bool isWalking = false;
    // �̵� ����
    public float moveInterval = 10f;
    // ��� �ð�
    public float waitTime = 5f;
    //������ ���� �ϴ� bool��
    public bool isChase = false;
    //���� ������
    public bool isAttack;


    void TargetTrack()
    {
        if (nav.enabled)
        {
            if (Targerting())
            {
                // �ν� ������ ���� ���
                nav.isStopped = false;
                nav.speed = speed;
                if (monsterType == e_MonsterType.Boss)
                {
                    StartCoroutine(WalkAndWait());
                }
                else
                {
                    nav.SetDestination(target.position);
                    StartCoroutine(enumeAttack());

                }
                if (!isChase)
                {
                    // ������ �ν� ������ ������ �ʾҴ� ��쿡�� Chase ���·� ��ȯ
                    isChase = true;
                    animator.SetBool("isWalk", true);
                }
            }
            else
            {
                // �ν� ������ ������ ���� ���
                if (!isChase || monsterType != e_MonsterType.Boss)
                {
                    // ������ �ν� ������ ���Դ� ��쿡�� Chase ���� ����
                    isChase = false;
                    animator.SetBool("isWalk", false);
                }

                // �÷��̾ �ν� ������ ������ ���� �̵�
                StartCoroutine(WalkAndWait());
            }
        }
    }

    //���� ���� �̵�
    IEnumerator WalkAndWait()
    {
        while (true)
        {
            if (!isWalking)
            {
                isWalking = true;

                // ���ο� ������ ��ġ
                if (monsterType == e_MonsterType.Boss && target != null)
                {
                    // "Boss" ������ ��� �÷��̾� ������ �ٰ�����
                    randomDestination = transform.position + (target.position - transform.position);
                }
                else
                {
                    randomDestination = RandomNavmeshLocation();
                }

                nav.SetDestination(randomDestination);
                yield return new WaitForSeconds(0.1f);
                animator.SetBool("isWalk", true);

                // ��ǥ���� �����ϰų� �������� ���� ������ ���
                yield return new WaitUntil(() => nav.remainingDistance <= nav.stoppingDistance ||
                nav.pathStatus == NavMeshPathStatus.PathPartial);
                animator.SetBool("isWalk", false);

                yield return new WaitForSeconds(waitTime);

                isWalking = false;
            }

            yield return null;
        }
    }

    Vector3 RandomNavmeshLocation()
    {
        Vector3 randomDirection = Random.insideUnitSphere * 10f;
        randomDirection += transform.position;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, 10f, NavMesh.AllAreas);

        return navHit.position;
    }

    //������ �ٵ�� ���� Player�� �浹�� ����� �ڷ� �з����ų� ���� �ޱ� ������ �װ��� ����
    void FreezeVelocity()
    {
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
    }

    bool Targerting()
    {
        //�ν� ����
        float targetRadius = 0f;

        //���� ��Ÿ�
        float targetRange = 0f;

        switch (monsterType)
        {
            case e_MonsterType.Melee:
                targetRadius = 3f;
                targetRange = 0.5f;
                break;
            case e_MonsterType.Range:
                targetRadius = 5f;
                targetRange = 5f;
                break;
            case e_MonsterType.Boss:
                targetRadius = 100f;
                targetRange = 100f;
                break;
        }

        //NavMeshAgent : �������� ���� �־ �и�
        if (isAttack == true)
        {
            nav.isStopped = true;
            nav.velocity = Vector3.zero;
        }

        else if (isAttack == false)
        {
            nav.isStopped = false;
        }

        RaycastHit[] raycasts =
            Physics.SphereCastAll(transform.position, targetRadius, transform.forward,
                                  targetRange, LayerMask.GetMask("Player"));

        if (raycasts.Length > 0)
        {
            target = raycasts[0].transform;
            transform.LookAt(target);
            return true; // �÷��̾ �ν� ���� ���� ����
        }
        else
        {
            target = null;
            return false; // �÷��̾ �ν� ���� ���� ����
        }

    }

    //======================================================================================

    //3. ����
    IEnumerator enumeAttack()
    {
        //�̹� �������̸� ����
        if (isAttack) yield break;

        isChase = false;
        isAttack = true;

        yield return new WaitForSeconds(0.5f);

        animator.SetBool("isWalk", false);

        yield return new WaitForSeconds(0.5f);

            // ȸ�� �Ŀ� ���� �ִϸ��̼� ����
            animator.SetTrigger("isAttack");

            switch (monsterType)
            {
                case e_MonsterType.Melee:
                    meleeArea.GetComponent<Attack>().Atk = atk;

                    yield return new WaitForSeconds(0.5f);
                    meleeArea.enabled = true;

                    yield return new WaitForSeconds(1f);
                    meleeArea.enabled = false;

                    break;

                case e_MonsterType.Range:
                    yield return new WaitForSeconds(0.5f);

                    GameObject rangeinstanceant = Instantiate(rangeBullet, bulletPos.position, bulletPos.rotation);
                    Rigidbody bulletRigid = rangeinstanceant.GetComponent<Rigidbody>();
                    Bullets bullets = bulletRigid.GetComponent<Bullets>();
                    bulletRigid.velocity = bulletPos.forward * 20f;
                    bullets.Atk = atk;

                    yield return new WaitForSeconds(1f);
                    DestroyImmediate(rangeinstanceant, true);

                    break;
            }
        yield return new WaitForSeconds(1f);

        animator.SetBool("isWalk", true);


        isChase = true;
        isAttack = false;
    }

    //======================================================================================

    //4. �ǰ� / ����
    //4-1. ������ ǥ��

    //������� �޾Ҵ���
    private bool canTakeDamage = true;

    // ������� ���� �� �ִ� ��ٿ� �ð�
    private float damageCooldown = 0.2f;


    //4-2. ������� �޾�����
    public void TakeDamage(int damage, Vector3 reactvec)
    {
        if (curHealth <= 0)
            return;

        curHealth -= damage;
        canTakeDamage = false;

        if (curHealth <= 0)
        {
            Die(reactvec);
        }
        else
        {
            StartCoroutine(DamageCooldown());
        }
    }

    //4-3 �������
    protected virtual void Die(Vector3 reactvec)
    {
        StopAllCoroutines();

        nav.isStopped = true;
        nav.enabled = false;
        foreach (SkinnedMeshRenderer mesh in meshs)
        {
            Material[] materialsCopy = mesh.materials;

            // �� ��Ƽ������ ������ ����
            for (int i = 0; i < materialsCopy.Length; i++)
            {
                materialsCopy[i].color = Color.gray;
            }

            mesh.materials = materialsCopy;
        }
        gameObject.layer = 10;
        isChase = false;
        animator.SetTrigger("doDie");

        //�˹�
        reactvec = reactvec.normalized;
        reactvec += Vector3.up;
        rigid.AddForce(reactvec * 5, ForceMode.Impulse);

        if (classroomController != null)
        {
            classroomController.MonsterDied(this);
        }

        if (monsterType != e_MonsterType.Boss)
        {
            Destroy(gameObject, 4); // ���� ������Ʈ ����
        }
    }

    // ClassroomController ����
    public void SetClassroomController(ClassroomController controller)
    {
        classroomController = controller;
    }

    //4-4. �ǰ�ȿ�� (���߿� �ٲܰ�)
    private IEnumerator DamageCooldown()
    {
        foreach (SkinnedMeshRenderer mesh in meshs)
        {
            Material[] materialsCopy = mesh.materials;

            // �� ��Ƽ������ ������ ����
            for (int i = 0; i < materialsCopy.Length; i++)
            {
                materialsCopy[i].color = Color.red;
            }

            mesh.materials = materialsCopy;
        }
        yield return new WaitForSeconds(0.2f);
        foreach (SkinnedMeshRenderer mesh in meshs)
        {
            Material[] materialsCopy = mesh.materials;

            // �� ��Ƽ������ ������ ����
            for (int i = 0; i < materialsCopy.Length; i++)
            {
                materialsCopy[i].color = Color.white;
            }

            mesh.materials = materialsCopy;
        }

        yield return new WaitForSeconds(damageCooldown);
        canTakeDamage = true;
    }

    //���� �����
    private void OnTriggerEnter(Collider other)
    {
        if (!canTakeDamage)
            return;

        if (other.CompareTag("Weapon"))
        {
            AttackCollider atk = other.GetComponent<AttackCollider>();
            Vector3 reactVec = transform.position - other.transform.position;
            player.FillGauge(player.fill_Gauge);

            TakeDamage(atk.Atk, reactVec);
            
            //����� ǥ����
            Debug.Log("Weapon : " + curHealth);
        }

        else if (other.CompareTag("Bullet"))
        {
            Bullets bullet = other.GetComponent<Bullets>();
            Vector3 reactVec = transform.position - other.transform.position;
            player.FillGauge(player.fill_Gauge);
            TakeDamage(bullet.Atk, reactVec);

            //����� ǥ����
            Debug.Log("Range : " + curHealth);
        }

        else if (other.CompareTag("Skill"))
        {
            Effect effect = other.GetComponent<Effect>();
            Vector3 reactVec = transform.position - other.transform.position;
            TakeDamage(effect.Atk, reactVec);

            //����� ǥ����
            Debug.Log("Skill : " + curHealth);
        }
    }

    //===============================================================================================================

    //5. ������ �ʱ�ȭ
    public virtual void Init()
    {
        rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        stat = new Stat(Name);
        meshs = GetComponentsInChildren<SkinnedMeshRenderer>();
        nav = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        TargetTrack();
    }
    private void FixedUpdate()
    {
        FreezeVelocity();
    }
}
