using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEvent : MonoBehaviour
{
    ParticleSystem particleSystem;
    public GameObject explosionEffectPrefab; // ���� ����Ʈ ������
    public LayerMask groundLayer; // �� ���̾�
    List<ParticleCollisionEvent> collisionEvents;

    private GameObject owner = null;

    public void SetOwner(GameObject _owner) => owner = _owner;

    public void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    private void OnParticleCollision(GameObject other)
    {
        int groundLayerMask = 1 << other.layer;

        ParticlePhysicsExtensions.GetCollisionEvents(particleSystem, other, collisionEvents);

        if ((groundLayer & groundLayerMask) != 0)
        {
            // ���� �浹���� �� ���� ����Ʈ ����
            var evt = collisionEvents[0];
            GameObject explosionEffect = Instantiate(explosionEffectPrefab, evt.intersection, Quaternion.identity);
            SoundManager.instance.PlaySfx(e_Sfx.ExplosionSound);
            Effect effect = explosionEffect.GetComponent<Effect>();
            ShiiDeathing boss = owner.GetComponent<ShiiDeathing>();
            effect.Atk = boss.atk;

            StartCoroutine(Destroy(explosionEffect, 1f));
        }
    }

    IEnumerator Destroy(GameObject target, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(target);
    }
}
