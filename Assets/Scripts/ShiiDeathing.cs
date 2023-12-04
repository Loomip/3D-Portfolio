using System.Collections;
using UnityEngine;


public class ShiiDeathing : Enemy
{
    // 보스의 패턴 개수
    int numberOfPatterns = 3; 

    public Transform bulletPos2;
    public GameObject Skill_2;

    [SerializeField] LayerMask layerMask;

    // 보스룸 컨트롤러
    private BossroomController bossroomController;

    void Start()
    {
        // "BossRoom" 태그를 가진 게임 오브젝트의 BossroomController 컴포넌트를 찾아 참조 설정
        bossroomController = GameObject.FindGameObjectWithTag("BossRoom").GetComponent<BossroomController>();
        Init();
        StartCoroutine(Think());
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
                    yield return ExecutePattern1();
                    break;
                case 1:
                    yield return ExecutePattern2();
                    break;
                case 2:
                    yield return ExecutePattern3();
                    break;
                    // 추가적인 패턴들 추가 가능
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
        base.Die(reactvec);  // Enemy 클래스의 Die 함수 호출

        bossroomController.isFinalBossRoom = true;

        if (bossroomController != null)
        {
            bossroomController.MonsterDied();  // 보스가 죽었을 때 MonsterDied 함수 호출
        }
    }
}
