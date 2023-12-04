using System.Collections;
using UnityEngine;


public class ShiiDeathing : Enemy
{
    // ������ ���� ����
    int numberOfPatterns = 3; 

    public Transform bulletPos2;
    public GameObject Skill_2;

    [SerializeField] LayerMask layerMask;

    // ������ ��Ʈ�ѷ�
    private BossroomController bossroomController;

    void Start()
    {
        // "BossRoom" �±׸� ���� ���� ������Ʈ�� BossroomController ������Ʈ�� ã�� ���� ����
        bossroomController = GameObject.FindGameObjectWithTag("BossRoom").GetComponent<BossroomController>();
        Init();
        StartCoroutine(Think());
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
                    yield return ExecutePattern1();
                    break;
                case 1:
                    yield return ExecutePattern2();
                    break;
                case 2:
                    yield return ExecutePattern3();
                    break;
                    // �߰����� ���ϵ� �߰� ����
            }
        }
    }

    IEnumerator ExecutePattern1()
    {
        animator.SetTrigger("isAttack");

        yield return new WaitForSeconds(1f);
        SoundManager.instance.PlaySfx(e_Sfx.BossAtteckSound);
        GameObject rangeInstant = Instantiate(rangeBullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRigid = rangeInstant.GetComponent<Rigidbody>();
        EnamyBullet bullets = bulletRigid.GetComponent<EnamyBullet>();
        bulletRigid.velocity = bulletPos.forward;
        bullets.Atk = atk;
        yield return new WaitForSeconds(3f);
        DestroyImmediate(rangeInstant, true);

        yield return new WaitForSeconds(2.5f);

    }
    IEnumerator ExecutePattern2()
    {
        animator.SetTrigger("isSkill_1");
        yield return new WaitForSeconds(12f);
    }
    IEnumerator ExecutePattern3()
    {
        animator.SetTrigger("isSkill_2");

        yield return new WaitForSeconds(1f);
        SoundManager.instance.PlaySfx(e_Sfx.BossSkill2Sound);
        GameObject rangeInstant = Instantiate(Skill_2, bulletPos2.position, bulletPos.rotation);
        var evt = rangeInstant.GetComponentInChildren<ExplosionEvent>();
        evt.SetOwner(gameObject);

        yield return new WaitForSeconds(8f);
        DestroyImmediate(rangeInstant, true);
    }

    public override void Init()
    {
        base.Init();
    }

    protected override void Die(Vector3 reactvec)
    {
        base.Die(reactvec);  // Enemy Ŭ������ Die �Լ� ȣ��

        bossroomController.isFinalBossRoom = true;

        if (bossroomController != null)
        {
            bossroomController.MonsterDied();  // ������ �׾��� �� MonsterDied �Լ� ȣ��
        }
    }
}
