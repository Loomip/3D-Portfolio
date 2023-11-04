using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillEffect : MonoBehaviour
{
    public GameObject Explosion_Effect;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            Destroy(gameObject);

            // 적에게 맞았을 때 이펙트를 생성
            GameObject game = Instantiate(Explosion_Effect, transform.position, Quaternion.identity);
            Effect effect = game.GetComponent<Effect>();
            effect.Atk = Player.instance.stat.GetStat(e_StatType.Atk) + 300;
        }
    }

    private void Start()
    {
        StartCoroutine(DestroyAfterDelay(gameObject, 5f));
    }


    IEnumerator DestroyAfterDelay(GameObject target, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(target);
    }
}
