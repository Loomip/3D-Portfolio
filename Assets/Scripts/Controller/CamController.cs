using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CamController : MonoBehaviour
{
    [SerializeField] private float cam_Speed = 2f; //���콺 ���� (ī�޶� �̵� �ӵ�)
    public Transform camPoint;

    private void RotateCamera()
    {
        //���� �ȱ�� Ǯ���� ��찡 �־� �ڿ� Time.timeScale�� ������
        Vector2 mouseMove = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * cam_Speed * Time.timeScale;
        Vector3 angle = camPoint.rotation.eulerAngles; //eulerAngles : rotation�� ���� ���ʹϾ� ���ε� ���Ͱ����� �ٲ���
        float x = angle.x - mouseMove.y; //���� �������� ���� �ݴ�� �Ǿ�����
        if (x < 180f)
            x = Mathf.Clamp(x, -1f, 70f); //���� Rotation.x���� �������� ����
        else
            x = Mathf.Clamp(x, 330f, 360f);
        camPoint.rotation = Quaternion.Euler(x, angle.y + mouseMove.x, angle.z);
    }

    void Update()
    {
        //������ Ȯ�� ��� �����ߵ�
        // if (Input.GetMouseButton(1))
        // {
        RotateCamera();
       // }
    }
}
