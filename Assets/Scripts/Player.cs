using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

//���������� ������ �ҹ��� ���� (�Ϲ��� ���)
// CTRL + K,D �� ����
// CTRL + R,G ���ʿ��� using ����
// Action ������ȯ x => void ���� (){}
// Func ���� ��ȯ o => void ���� (int (int,int .. +n)))

public class Player : SingletonDontDestroy<Player>
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

    // ȸ�� ���� ���� ������ ������ ����
    private Vector3 dodgeDir;

    //0. ĳ���� ����
    public string Name;

    //1. ����
    public int gaugeCount = 0;    // ������ ī��Ʈ

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
    public int Def
        => stat.GetStat(e_StatType.Def);
    public int Speed
        => stat.GetStat(e_StatType.Spd);
    public int Acc
        => stat.GetStat(e_StatType.Acc);
    public int Del
        => stat.GetStat(e_StatType.Del);

    //================================================================================================================

    // 2. �̵�, �ִϸ��̼�

    // �̵� ������ ���θ� ��Ÿ���� ����
    bool isMoving = true;
    public bool isUseSkill = false;

    void Locomotion()
    {
        float x = Input.GetAxisRaw("Horizontal") * stat.GetStat(e_StatType.Spd);
        float z = Input.GetAxisRaw("Vertical") * stat.GetStat(e_StatType.Spd);

        // ĳ���� ��Ʈ�ѷ��� �̵��� ���� �ʰ� ���� ��ġ�� ������Ʈ
        Model.transform.position = character.transform.position;

        // �̵�
        MoveDir = new Vector3(x, 0, z).normalized;

        //ȸ�� �̵� ����
        if (isDodge)
            MoveDir = dodgeDir;

        // �̵� ������ ���� �Ǵ�
        isMoving = (x != 0f || z != 0f);

        //�ִϸ��̼�
        animator.SetBool("isRun", isMoving);
    }

    void MoveControl()
    {
        //ȭ�鸶�� ����� ī�޶� ã�Ƽ� �÷��̾��� ķ����Ʈ�� �־��� : �����ߴ°� ���� �Ⱦ try-catch ������� ����
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
            //ĳ������ ����
            Vector3 char_forward = new Vector3(camPoint.forward.x, MoveDir.y, camPoint.forward.z).normalized * stat.GetStat(e_StatType.Spd);

            //ĳ������ ����
            Vector3 char_right = new Vector3(camPoint.right.x, MoveDir.y, camPoint.right.z).normalized * stat.GetStat(e_StatType.Spd);

            //���� ������ �ٲ���
            Vector3 dir = char_forward * MoveDir.z + char_right * MoveDir.x;

            //�ٶ󺸴� ������ ���� ������ �������� �ٲ���
            Model.transform.forward = Vector3.Lerp(Model.transform.forward, dir, rotationSpeed * Time.deltaTime);

            //�߷�
            if (character.isGrounded == false)
            {
                dir.y -= gravity;
            }

            character.Move(dir * Time.deltaTime);
            //char_forward = MoveDir.z < 0 ? char_forward / 2f : char_forward; //�ڷ� �� �� �ӵ��� ���ҽ�Ų��.
        }
    }

    //ȸ��
    bool isDodge;

    void Dodge()
    {
        // "Dodge" ��ư�� ó�� ������ ���� ó��
        if (Input.GetButtonDown("Dodge") && !isDodge)
        {
            //��������
            isDamage = true;

            //ȸ�� �̵� ����
            dodgeDir = MoveDir;

            // ���� ���ǵ� ����
            int originalSpeed = stat.GetStat(e_StatType.Spd);

            // ���ǵ� 2�� ����
            int newSpeed = originalSpeed * 2;

            // ���� ����
            stat.SetStat(e_StatType.Spd, newSpeed);

            animator.SetTrigger("doDodge");

            // ȸ�� ���� ���·� ����
            isDodge = true;

            // ���� �ð� �� ���ǵ带 �ٽ� ���� ������ ������ �ڷ�ƾ ����
            StartCoroutine(ResetSpeedAfterDelay(originalSpeed, 0.8f));
        }
    }

    IEnumerator ResetSpeedAfterDelay(int originalSpeed, float delay)
    {
        yield return new WaitForSeconds(delay);

        // ���ǵ带 ���� ������ �����ϴ�.
        stat.SetStat(e_StatType.Spd, originalSpeed);

        // ȸ�� ���� ���� ����
        isDodge = false;

        //�������� ����
        isDamage = false;
    }

    //================================================================================================================

    // ���⸦ ������� ���θ� ��Ÿ���� ����
    public bool isWeaponEquipped = false;
    // ��ų ��� ���� ���θ� ��Ÿ���� �÷���
    private bool canUseSkill = true;

    //3. ����
    void Attack()
    {
        if (Input.GetMouseButtonDown(0) && !isMoving && isWeaponEquipped)
        {
            animator.SetTrigger("isAtk");
        }
        if (Input.GetMouseButtonDown(1) && !isMoving && isWeaponEquipped)
        {
            if (gaugeCount > 0)
            {
                // ��ų ��� ��
                canUseSkill = false;

                StartCoroutine(SkillLength());
            }
        }
    }

    IEnumerator SkillLength()
    {
        animator.SetTrigger("isSkill");
        yield return new WaitForEndOfFrame();
        isUseSkill = true;
        float length = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(length);
        --gaugeCount;
        UIManager.instance.Refresh_Gauge(instance);
        isUseSkill = false;

        canUseSkill = true;
    }


    public void FillGauge(int fill_Gauge)
    {
        // �������� ä��� ����
        Gauge += fill_Gauge;

        // �������� �Ӱ谪�� �Ѿ��� �� ī��Ʈ�� ����
        if (Gauge >= MGauge)
        {
            ++gaugeCount;
            Gauge = 0;
        }

        // ������ ��������
        UIManager.instance.Refresh_Gauge(instance);
    }


    //================================================================================================================

    //4. ��Ʈ/����

    // ���� �ִϸ��̼� ��� ������ ����
    private bool isDeadAnimating = false;

    // �ǰ� ���� ����
    private bool isDamage = false;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            if (!isDamage && !isDeadAnimating)
            {
                Attack enemyAttack = other.GetComponentInChildren<Attack>();
                Bullets bullets = other.GetComponent<Bullets>();
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
            // ���� ��Ƽ���� �迭�� �����Ͽ� ���ο� ��Ƽ���� �迭�� ����
            Material[] materialsCopy = mesh.materials;

            // �� ��Ƽ������ ������ ����
            for (int i = 0; i < materialsCopy.Length; i++)
            {
                materialsCopy[i].color = Color.red;
            }

            // ����� ��Ƽ���� �迭�� SkinnedMeshRenderer ������Ʈ�� �Ҵ��Ͽ� ������ ����ǵ��� ��
            mesh.materials = materialsCopy;
        }

        //�����ð�
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

    //��ƼŬ ����� ������� ����.
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

            //�׾�����
            else if (CurHealth <= 0)
            {
                animator.SetTrigger("isDead");
                isDeadAnimating = true;
            }
        }
    }


    // ���� �ִϸ��̼� ��� ���� ���� ����
    //IEnumerator ResetDeadAnimation()
    //{
    //    // ���� �ִϸ��̼��� ���� ������ ���
    //    yield return new WaitForSeconds();

    //    isDeadAnimating = false;
    //    curHealth = maxHealth;
    //}







    //================================================================================================================

    //5. ��ȣ�ۿ�

    //��ȣ�ۿ� Ű
    void NPCInteract()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            InteractWithCurrentTarget();
        }
    }

    //��ȣ �ۿ�
    public void InteractWithCurrentTarget()
    {
        // ��ȭâ�� �̹� �����ִ� ���, ���ο� ��ȭ�� �������� ����
        if (UIManager.instance.isAction) return;

        float interactRange = 1.5f;
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        foreach (Collider collider in colliderArray)
        {
            if (collider.TryGetComponent(out NPC_Base npc))
            {
                npc.OnInteract();

                UIManager.instance.Refresh_Talk(gameObject);
            }
        }
    }

    //��ȣ�ۿ� ������Ʈ
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

    //6. ������ �ʱ�ȭ
    void Init()
    {
        character = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        camPoint = GetComponent<CamController>().camPoint;
        meshs = GetComponentsInChildren<SkinnedMeshRenderer>();
        //ĳ���͸� �ڽ����� �ִ� ����. �÷��̾�� �״������ �𵨹����� �ٲ㼭 �� �ٲ�� ������ �ִ°�
        Model = transform.GetChild(0).gameObject;
        stat = new Stat(Name);
    }

    //=================================================================================================================================

    //7. ����

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


}
