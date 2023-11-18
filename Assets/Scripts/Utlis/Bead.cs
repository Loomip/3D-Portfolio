using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bead : MonoBehaviour
{
    public float amplitude = 0.5f;  // 움직임의 크기
    public float frequency = 1f;   // 움직임의 빠르기

    // 초기 위치 저장
    Vector3 startPos;

    void Start()
    {
        // 현재 위치를 초기 위치로 설정
        startPos = transform.position;
    }

    void Update()
    {
        // 시간에 따라 새로운 Y 위치를 계산
        float newY = startPos.y + amplitude * Mathf.Sin(Time.time * frequency);

        // 구슬의 새로운 위치 설정
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
