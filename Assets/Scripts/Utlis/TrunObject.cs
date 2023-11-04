using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrunObject : MonoBehaviour
{
    public float rotateSpeed = 5f;
    
    void Update()
    {
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }
}
