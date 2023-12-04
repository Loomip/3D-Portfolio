using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{ 
    // ������ ���ݷ� ���� �����ϴ� ����
    public int Atk
    {
        get;
        set;
    }

    protected Rigidbody rigid = null;
    protected float speed = 0f;

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}

