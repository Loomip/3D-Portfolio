using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{ 
    // 무기의 공격력 값을 저장하는 변수
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

