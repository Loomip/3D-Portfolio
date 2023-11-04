using System.Collections;
using UnityEngine;


public class ShiiDeathing : Enemy
{
    // 보스의 패턴 개수
    int numberOfPatterns = 3; 

    public Transform bulletPos2;
    public GameObject Skill_2;

    [SerializeField] LayerMask layerMask;
    
    void Start()
    {
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

        GameObject rangeInstant = Instantiate(rangeBullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRigid = rangeInstant.GetComponent<Rigidbody>();
        Bullets bullets = bulletRigid.GetComponent<Bullets>();
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
}
