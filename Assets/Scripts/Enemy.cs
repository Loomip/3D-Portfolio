using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    //몬스터
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
    //1. 스텟 (네임)
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

    //2. 인식 범위

    //랜덤으로 정한 위치
    private Vector3 randomDestination;
    //이동중인지
    private bool isWalking = false;
    // 이동 간격
    public float moveInterval = 10f;
    // 대기 시간
    public float waitTime = 5f;
    //추적을 결정 하는 bool값
    public bool isChase = false;
    //공격 중인지
    public bool isAttack;


    void TargetTrack()
    {
        if (nav.enabled)
        {
            if (Targerting())
            {
                // 인식 범위에 들어온 경우
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
                    // 이전에 인식 범위에 들어오지 않았던 경우에만 Chase 상태로 전환
                    isChase = true;
                    animator.SetBool("isWalk", true);
                }
            }
            else
            {
                // 인식 범위에 들어오지 않은 경우
                if (!isChase || monsterType != e_MonsterType.Boss)
                {
                    // 이전에 인식 범위에 들어왔던 경우에만 Chase 상태 해제
                    isChase = false;
                    animator.SetBool("isWalk", false);
                }

                // 플레이어가 인식 범위에 없으면 랜덤 이동
                StartCoroutine(WalkAndWait());
            }
        }
    }

    //몬스터 랜덤 이동
    IEnumerator WalkAndWait()
    {
        while (true)
        {
            if (!isWalking)
            {
                isWalking = true;

                // 새로운 무작위 위치
                if (monsterType == e_MonsterType.Boss && target != null)
                {
                    // "Boss" 몬스터일 경우 플레이어 쪽으로 다가가기
                    randomDestination = transform.position + (target.position - transform.position);
                }
                else
                {
                    randomDestination = RandomNavmeshLocation();
                }

                nav.SetDestination(randomDestination);
                yield return new WaitForSeconds(0.1f);
                animator.SetBool("isWalk", true);

                // 목표까지 도달하거나 움직이지 못할 때까지 대기
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

    //리지드 바디로 인해 Player와 충돌이 생기면 뒤로 밀려나거나 힘을 받기 때문에 그것을 방지
    void FreezeVelocity()
    {
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
    }

    bool Targerting()
    {
        //인식 범위
        float targetRadius = 0f;

        //공격 사거리
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

        //NavMeshAgent : 물리적인 힘이 있어서 밀림
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
            return true; // 플레이어가 인식 범위 내에 있음
        }
        else
        {
            target = null;
            return false; // 플레이어가 인식 범위 내에 없음
        }

    }

    //======================================================================================

    //3. 공격
    IEnumerator enumeAttack()
    {
        //이미 공격중이면 종료
        if (isAttack) yield break;

        isChase = false;
        isAttack = true;

        yield return new WaitForSeconds(0.5f);

        animator.SetBool("isWalk", false);

        yield return new WaitForSeconds(0.5f);

            // 회전 후에 공격 애니메이션 실행
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

    //4. 피격 / 죽음
    //4-1. 에너지 표시

    //대미지를 받았는지
    private bool canTakeDamage = true;

    // 대미지를 받을 수 있는 쿨다운 시간
    private float damageCooldown = 0.2f;


    //4-2. 대미지를 받았을때
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

    //4-3 죽음모션
    protected virtual void Die(Vector3 reactvec)
    {
        StopAllCoroutines();

        nav.isStopped = true;
        nav.enabled = false;
        foreach (SkinnedMeshRenderer mesh in meshs)
        {
            Material[] materialsCopy = mesh.materials;

            // 각 머티리얼의 색상을 변경
            for (int i = 0; i < materialsCopy.Length; i++)
            {
                materialsCopy[i].color = Color.gray;
            }

            mesh.materials = materialsCopy;
        }
        gameObject.layer = 10;
        isChase = false;
        animator.SetTrigger("doDie");

        //넉백
        reactvec = reactvec.normalized;
        reactvec += Vector3.up;
        rigid.AddForce(reactvec * 5, ForceMode.Impulse);

        if (classroomController != null)
        {
            classroomController.MonsterDied(this);
        }

        if (monsterType != e_MonsterType.Boss)
        {
            Destroy(gameObject, 4); // 몬스터 오브젝트 삭제
        }
    }

    // ClassroomController 설정
    public void SetClassroomController(ClassroomController controller)
    {
        classroomController = controller;
    }

    //4-4. 피격효과 (나중에 바꿀거)
    private IEnumerator DamageCooldown()
    {
        foreach (SkinnedMeshRenderer mesh in meshs)
        {
            Material[] materialsCopy = mesh.materials;

            // 각 머티리얼의 색상을 변경
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

            // 각 머티리얼의 색상을 변경
            for (int i = 0; i < materialsCopy.Length; i++)
            {
                materialsCopy[i].color = Color.white;
            }

            mesh.materials = materialsCopy;
        }

        yield return new WaitForSeconds(damageCooldown);
        canTakeDamage = true;
    }

    //실제 대미지
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
            
            //대미지 표기방식
            Debug.Log("Weapon : " + curHealth);
        }

        else if (other.CompareTag("Bullet"))
        {
            Bullets bullet = other.GetComponent<Bullets>();
            Vector3 reactVec = transform.position - other.transform.position;
            player.FillGauge(player.fill_Gauge);
            TakeDamage(bullet.Atk, reactVec);

            //대미지 표기방식
            Debug.Log("Range : " + curHealth);
        }

        else if (other.CompareTag("Skill"))
        {
            Effect effect = other.GetComponent<Effect>();
            Vector3 reactVec = transform.position - other.transform.position;
            TakeDamage(effect.Atk, reactVec);

            //대미지 표기방식
            Debug.Log("Skill : " + curHealth);
        }
    }

    //===============================================================================================================

    //5. 데이터 초기화
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
