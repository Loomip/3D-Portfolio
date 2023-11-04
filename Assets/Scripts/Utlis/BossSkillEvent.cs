using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkillEvent : MonoBehaviour
{
    public GameObject Skiil_1;
    public Transform BulletPos3;

    //보스 애니메이션 이벤트
    void BossSkill_1()
    {
        GameObject rangeInstant = Instantiate(Skiil_1, BulletPos3.position, BulletPos3.rotation);
        Effect effect = rangeInstant.GetComponent<Effect>();
        ShiiDeathing boss = GetComponent<ShiiDeathing>();
        effect.Atk = boss.atk;
        StartCoroutine(DestroyAfterDelay(rangeInstant, 5f));
    }

    IEnumerator DestroyAfterDelay(GameObject target, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(target);
    }

}
