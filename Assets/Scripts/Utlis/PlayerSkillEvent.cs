using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillEvent : MonoBehaviour
{
    public GameObject ont_Hand_Sword_Skill;
    public GameObject two_Hand_Sword_Skill;
    public GameObject karate_Skill;
    public GameObject boxing_Skill;
    public GameObject bag_Skill;
    public GameObject bead_Skill;
    public GameObject musical_Skil;
    public GameObject dance_Skill;
    public Transform skillPos;
    public Transform dancePos;

    //플레이어 애니메이션 이벤트
    void Ont_Hand_Sword_Skill()
    {
        GameObject rangeInstant = Instantiate(ont_Hand_Sword_Skill, skillPos.position, ont_Hand_Sword_Skill.transform.rotation);
        Effect effect = rangeInstant.GetComponent<Effect>();
        Player player = GetComponentInParent<Player>();
        effect.Atk = player.Atk + 50;
        StartCoroutine(DamageWithDelay(rangeInstant, effect.collider, 1f, 3f));
    }

    void Two_Hand_Sword_Skill()
    {
        GameObject rangeInstant = Instantiate(two_Hand_Sword_Skill, skillPos.position, two_Hand_Sword_Skill.transform.rotation);
        Effect effect = rangeInstant.GetComponent<Effect>();
        Player player = GetComponentInParent<Player>();
        effect.Atk = player.Atk + 100;
        StartCoroutine(DamageWithDelay(rangeInstant, effect.collider, 1f, 3f));
    }

    void Karate_Skill()
    {
        GameObject rangeInstant = Instantiate(karate_Skill, skillPos.position,  skillPos.rotation * Quaternion.Euler(180f,0f,0f));
        Effect effect = rangeInstant.GetComponent<Effect>();
        Player player = GetComponentInParent<Player>();
        effect.Atk = player.Atk + 200;
        StartCoroutine(DamageWithDelay(rangeInstant, effect.collider, 2f, 2f));
    }

    void Boxing_Skill()
    {
        GameObject rangeInstant = Instantiate(boxing_Skill, skillPos.position, skillPos.rotation * Quaternion.Euler(180f, 0f, 0f));
        Effect effect = rangeInstant.GetComponent<Effect>();
        Player player = GetComponentInParent<Player>();
        effect.Atk = player.Atk + 200;
        StartCoroutine(DamageWithDelay(rangeInstant, effect.collider, 2f, 2f));
    }

    void Bag_Skill()
    {
        GameObject rangeInstant = Instantiate(bag_Skill, skillPos.position, bag_Skill.transform.rotation);
        Rigidbody bulletRigid = rangeInstant.GetComponent<Rigidbody>();
        bulletRigid.velocity = skillPos.forward * 20f;
    }

    void Bead_Skill()
    {
        GameObject rangeInstant = Instantiate(bead_Skill, skillPos.position, bead_Skill.transform.rotation);
        Rigidbody bulletRigid = rangeInstant.GetComponent<Rigidbody>();
        bulletRigid.velocity = skillPos.forward * 10f;
        Effect effect = rangeInstant.GetComponent<Effect>();
        Player player = GetComponentInParent<Player>();
        effect.Atk = player.Atk + 300;
        StartCoroutine(DamageWithDelay(rangeInstant, effect.collider, 5f, 5f));
    }

    void Musical_Skil()
    {
        GameObject rangeInstant = Instantiate(musical_Skil, dancePos.position, dancePos.rotation);
        Effect effect = rangeInstant.GetComponent<Effect>();
        Player player = GetComponentInParent<Player>();
        effect.Atk = player.Atk + 50;
        StartCoroutine(DamageWithDelay(rangeInstant, effect.collider, 0.5f, 5f));
    }

    void Dance_Skill()
    {
        GameObject rangeInstant = Instantiate(dance_Skill, dancePos.position, dancePos.rotation * Quaternion.Euler(-90f, 0f, 0f));
        Effect effect = rangeInstant.GetComponent<Effect>();
        Player player = GetComponentInParent<Player>();
        effect.Atk = player.Atk + 50;
        StartCoroutine(DamageWithDelay(rangeInstant, effect.collider, 0.5f, 5.5f));
    }

    IEnumerator DamageWithDelay(GameObject gameObject, Collider collider, float delay, float duration)
    {
        while (duration > 0f)
        {
            yield return new WaitForSeconds(delay);
            collider.enabled = !collider.enabled; // Collider를 껐다 켜기
            duration -= delay;
        }
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

    // 공격 애니메이션 종료 이벤트
    void OnAttackEnd()
    {
        Player player = GetComponentInParent<Player>();
        player.isAttacking = false;
    }
}
