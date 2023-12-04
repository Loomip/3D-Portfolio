using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

//전역변수는 무조건 소문자 시작 (암묵적 약속)
// CTRL + K,D 줄 정리
// CTRL + R,G 불필요한 using 제거
// Action 변수변환 x => void 제목 (){}
// Func 변수 변환 o => void 제목 (int (int,int .. +n)))

public class Player : MonoBehaviour
{
    CharacterController character;
    public Animator animator;
    GameObject Model;
    Transform camPoint;
    public Stat stat;
    public GameObject[] weapons;
    public List<int> weaponsTypes;
    SkinnedMeshRenderer[] meshs;
    CinemachineVirtualCamera virtualCamera;
    public BulletEvent bulletEvent;

    public float rotationSpeed = 5f;
    public float dash = 5f;
    public float gravity = 9.81f;
    private Vector3 MoveDir;

    // 회피 중일 때의 방향을 저장할 변수
    private Vector3 dodgeDir;

    //플레이어 위치 정보
    public Vector3 position;

    // 이동 중인지 여부를 나타내는 변수
    bool isMoving = true;
    public bool isUseSkill = false;

    //회피
    bool isDodge;

    // 무기를 들었는지 여부를 나타내는 변수
    public bool isWeaponEquipped = false;

    // 스킬 사용 가능 여부를 나타내는 플래그
    public bool canUseSkill = false;

    //공격중인지 
    public bool isAttacking = false;

    public Player(Vector3 position)
    {
        this.position = position;
    }

    //0. 캐릭터 네임
    public string Name;

    //1. 스탯
    public int gaugeCount = 0;    // 게이지 카운트

    public int MaxHealth
        => stat.GetStat(e_StatType.MHP);
    public int CurHealth
    {
        get => stat.GetStat(e_StatType.HP);
        set => stat.SetStat(e_StatType.HP, value);
    }
    public int Gauge
    {
        get => stat.GetStat(e_StatType.Gauge);
        set => stat.SetStat(e_StatType.Gauge, value);
    }
    public int MGauge
        => stat.GetStat(e_StatType.MGauge);
    public int fill_Gauge
    {
        get => stat.GetStat(e_StatType.Fill_Gauge);
        set => stat.SetStat(e_StatType.Fill_Gauge, value);
    }
    public int Atk
        => stat.GetStat(e_StatType.Atk);
    public int Speed
        => stat.GetStat(e_StatType.Spd);

    //================================================================================================================

    // 2. 이동, 애니메이션

    void Locomotion()
    {
        if (isAttacking) return;

        float x = Input.GetAxisRaw("Horizontal") * stat.GetStat(e_StatType.Spd);
        float z = Input.GetAxisRaw("Vertical") * stat.GetStat(e_StatType.Spd);

        // 캐릭터 컨트롤러의 이동은 하지 않고 모델의 위치만 업데이트
        Model.transform.position = character.transform.position;

        // 이동
        MoveDir = new Vector3(x, 0, z).normalized;

       

        // 이동 중인지 여부 판단
        isMoving = (x != 0f || z != 0f);

        //애니메이션
        animator.SetBool("isRun", isMoving);
    }

    void MoveControl()
    {
        //화면마다 버츄얼 카메라를 찾아서 플레이어의 캠포인트를 넣어줌 : 오류뜨는게 보기 싫어서 try-catch 블록으로 만듬
        try
        {
            virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
            if (virtualCamera != null)
            {
                virtualCamera.Follow = camPoint;
            }
            else
            {
                Debug.LogWarning("VirtualCamera not found!");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error finding VirtualCamera: " + e.Message);
        }
        if (isMoving)
        {
            Vector3 dir;

            //회피 이동 고정
            if (isDodge)
            {
                dir = Model.transform.forward * stat.GetStat(e_StatType.Spd);
            }
            else
            {
                //캐릭터의 정면
                Vector3 char_forward = new Vector3(camPoint.forward.x, MoveDir.y, camPoint.forward.z).normalized * stat.GetStat(e_StatType.Spd);

                //캐릭터의 측면
                Vector3 char_right = new Vector3(camPoint.right.x, MoveDir.y, camPoint.right.z).normalized * stat.GetStat(e_StatType.Spd);

                //가는 방향을 바꿔줌
                dir = char_forward * MoveDir.z + char_right * MoveDir.x;

                //바라보는 방향을 내가 누르는 방향으로 바꿔줌
                Model.transform.forward = Vector3.Lerp(Model.transform.forward, dir, rotationSpeed * Time.deltaTime);
            }

            //중력
            if (character.isGrounded == false)
            {
                dir.y -= gravity;
            }

            character.Move(dir * Time.deltaTime);
            //char_forward = MoveDir.z < 0 ? char_forward / 2f : char_forward; //뒤로 갈 때 속도를 감소시킨다.
        }
    }

    
    void Dodge()
    {
        // "Dodge" 버튼이 처음 눌렸을 때만 처리
        if (Input.GetButtonDown("Dodge") && !isDodge)
        {
            isAttacking = false;
            //무적상태
            isDamage = true;
            foreach (var weapon in weapons)
            {
                if (weapon != null)
                {
                    Weapons weapons = weapon.GetComponent<Weapons>();
                    if (weapons != null && weapons.trailEffect != null)
                    {
                        weapons.trailEffect.enabled = false;
                    }
                }
            }
            //회피 이동 고정
            dodgeDir = transform.forward;

            // 현재 스피드 저장
            int originalSpeed = stat.GetStat(e_StatType.Spd);

            // 스피드 2배 증가
            int newSpeed = originalSpeed * 2;

            // 스텟 설정
            stat.SetStat(e_StatType.Spd, newSpeed);

            animator.SetTrigger("doDodge");

            // 회피 중인 상태로 설정
            isDodge = true;

            // 일정 시간 후 스피드를 다시 원래 값으로 돌리는 코루틴 시작
            StartCoroutine(ResetSpeedAfterDelay(originalSpeed, 0.8f));
        }
    }

    IEnumerator ResetSpeedAfterDelay(int originalSpeed, float delay)
    {
        yield return new WaitForSeconds(delay);

        // 스피드를 원래 값으로 돌립니다.
        stat.SetStat(e_StatType.Spd, originalSpeed);

        // 회피 중인 상태 해제
        isDodge = false;

        //무적상태 해제
        isDamage = false;

        isUseSkill = false;
    }

    //================================================================================================================

    

    //3. 공격
    void Attack()
    {
        //기본공격
        if (Input.GetMouseButtonDown(0) && !isMoving && isWeaponEquipped)
        {
            isAttacking = true;
            isUseSkill = true;
            animator.SetTrigger("isAtk");
        }

        //스킬
        if (Input.GetMouseButtonDown(1) && !isMoving && isWeaponEquipped && !canUseSkill)
        {
            canUseSkill = true;
            if (gaugeCount > 0 && canUseSkill)
            {
                // 스킬 사용 중
                StartCoroutine(SkillLength());
            }
        }
    }

    IEnumerator SkillLength()
    {
        animator.SetTrigger("isSkill");
        yield return new WaitForEndOfFrame();
        isUseSkill = true;
        gaugeCount -= stat.GetStat(e_StatType.Skill_Gauge);
        UIManager.instance.Refresh_Gauge(this);

        //스킬 사용중 이동 금지
        yield return new WaitForSeconds(2f);
        isUseSkill = false;

        //스킬 쿨타임
        yield return new WaitForSeconds(stat.GetStat(e_StatType.CoolTime) - 2f);
        canUseSkill = false;
    }


    public void FillGauge(int fill_Gauge)
    {
        // 게이지를 채우는 로직
        Gauge += fill_Gauge;

        // 게이지가 임계값을 넘었을 때 카운트를 증가
        if (Gauge >= MGauge)
        {
            ++gaugeCount;
            Gauge = 0;
        }

        // 게이지 리프레시
        UIManager.instance.Refresh_Gauge(this);
    }

   


    //================================================================================================================

    //4. 히트/죽음

    // 죽음 애니메이션 재생 중인지 여부
    private bool isDeadAnimating = false;

    // 피격 상태 여부
    private bool isDamage = false;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            if (!isDamage && !isDeadAnimating)
            {
                Attack enemyAttack = other.GetComponentInChildren<Attack>();
                EnamyBullet bullets = other.GetComponent<EnamyBullet>();
                Effect effects = other.GetComponent<Effect>();

                int damage = 0;

                if (effects != null)
                {
                    damage += effects.Atk;
                }

                if (enemyAttack != null)
                {
                    damage += enemyAttack.Atk;
                }

                if (bullets != null)
                {
                    damage += bullets.Atk;
                }

                if (CurHealth > 0)
                {
                    TakeDamage(damage);
                }
            }
        }

    }


    IEnumerator OnDamage()
    {
        isDamage = true;

        foreach (SkinnedMeshRenderer mesh in meshs)
        {
            // 현재 머티리얼 배열을 복사하여 새로운 머티리얼 배열을 생성
            Material[] materialsCopy = mesh.materials;

            // 각 머티리얼의 색상을 변경
            for (int i = 0; i < materialsCopy.Length; i++)
            {
                materialsCopy[i].color = Color.red;
            }

            // 변경된 머티리얼 배열을 SkinnedMeshRenderer 컴포넌트에 할당하여 색상이 변경되도록 함
            mesh.materials = materialsCopy;
        }

        //무적시간
        yield return new WaitForSeconds(1f);
        isDamage = false;

        foreach (SkinnedMeshRenderer mesh in meshs)
        {
            Material[] materialsCopy = mesh.materials;

            for (int i = 0; i < materialsCopy.Length; i++)
            {
                materialsCopy[i].color = Color.white;
            }

            mesh.materials = materialsCopy;
        }
    }

    //파티클 대미지 계산으로 인함.
    public void TakeDamage(int damage)
    {
        if (!isDamage && !isDeadAnimating)
        {
            if (CurHealth > 0)
            {
                CurHealth -= damage;
                Debug.Log("Player Health: " + CurHealth);
                UIManager.instance.Refresh_HP(this);
                StartCoroutine(OnDamage());
            }

            //죽었을때
            else if (CurHealth <= 0)
            {
                animator.SetTrigger("isDead");
                isDeadAnimating = true;
            }
        }
    }


    // 죽음 애니메이션 재생 중인 상태 해제
    //IEnumerator ResetDeadAnimation()
    //{
    //    // 죽음 애니메이션이 끝날 때까지 대기
    //    yield return new WaitForSeconds();

    //    isDeadAnimating = false;
    //    curHealth = maxHealth;
    //}







    //================================================================================================================

    //5. 상호작용

    //상호작용 키
    void NPCInteract()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            InteractWithCurrentTarget();
        }
    }

    //상호 작용
    public void InteractWithCurrentTarget()
    {
        // 대화창이 이미 열려있는 경우, 새로운 대화를 시작하지 않음
        if (UIManager.instance.isAction) return;

        float interactRange = 1.5f;

        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        foreach (Collider collider in colliderArray)
        {
            if (collider.TryGetComponent(out NPC_Base npc))
            {
                npc.OnInteract();

                // 상점이 열리지 않은 경우에만 대화창을 업데이트합니다.
                if (!npc.IsShopOpen())
                {
                    UIManager.instance.Refresh_Talk(gameObject);
                }
            }
        }
    }

    //상호작용 오브젝트
    public NPC_Base GetInterctableObject()
    {
        float interactRange = 1.5f;
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        foreach (Collider collider in colliderArray)
        {
            if (collider.TryGetComponent(out NPC_Base npc))
            {
                return npc;
            }
        }
        return null;
    }



    //================================================================================================================

    //6. 데이터 초기화
    void Init()
    {
        character = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        camPoint = GetComponent<CamController>().camPoint;
        meshs = GetComponentsInChildren<SkinnedMeshRenderer>();
        //캐릭터를 자식으로 넣는 이유. 플레이어는 그대로지만 모델방향을 바꿔서 다 바뀌게 착각을 주는것
        Model = transform.GetChild(0).gameObject;
        stat = new Stat(Name);
    }

    //=================================================================================================================================

    //7. 저장

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        Locomotion();
        NPCInteract();
        Attack();
        Dodge();
    }

    private void FixedUpdate()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") && !isDeadAnimating && !isUseSkill)
            MoveControl();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // PlayerPrefs에서 플레이어의 위치를 불러옴
        float x = PlayerPrefs.GetFloat("PlayerPosX");
        float y = PlayerPrefs.GetFloat("PlayerPosY");
        float z = PlayerPrefs.GetFloat("PlayerPosZ");

        // 플레이어의 위치를 설정
        transform.position = new Vector3(x, y, z);
    }

}
