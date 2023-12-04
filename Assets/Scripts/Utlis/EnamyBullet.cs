using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnamyBullet : Bullets
{
    protected float currentSpeed;

    protected override void Awake()
    {
        base.Awake();
    }

    protected void Update()
    {
        if (currentSpeed <= speed)
            currentSpeed += speed * Time.deltaTime;

        transform.position += transform.forward * currentSpeed * Time.deltaTime;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.transform.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
