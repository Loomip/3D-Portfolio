using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bead : MonoBehaviour
{
    public float amplitude = 0.5f;  // �������� ũ��
    public float frequency = 1f;   // �������� ������

    // �ʱ� ��ġ ����
    Vector3 startPos;

    void Start()
    {
        // ���� ��ġ�� �ʱ� ��ġ�� ����
        startPos = transform.position;
    }

    void Update()
    {
        // �ð��� ���� ���ο� Y ��ġ�� ���
        float newY = startPos.y + amplitude * Mathf.Sin(Time.time * frequency);

        // ������ ���ο� ��ġ ����
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
