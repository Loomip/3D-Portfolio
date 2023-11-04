using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CamController : MonoBehaviour
{
    [SerializeField] private float cam_Speed = 2f; //마우스 감도 (카메라 이동 속도)
    public Transform camPoint;

    private void RotateCamera()
    {
        //맵을 옴기면 풀리는 경우가 있어 뒤에 Time.timeScale을 곱해줌
        Vector2 mouseMove = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * cam_Speed * Time.timeScale;
        Vector3 angle = camPoint.rotation.eulerAngles; //eulerAngles : rotation은 원래 쿼터니언 값인데 백터값으로 바꿔줌
        float x = angle.x - mouseMove.y; //직접 돌려보면 축이 반대로 되어있음
        if (x < 180f)
            x = Mathf.Clamp(x, -1f, 70f); //직접 Rotation.x값을 돌려보고 결정
        else
            x = Mathf.Clamp(x, 330f, 360f);
        camPoint.rotation = Quaternion.Euler(x, angle.y + mouseMove.x, angle.z);
    }

    void Update()
    {
        //감도와 확대 축소 만들어야됨
        // if (Input.GetMouseButton(1))
        // {
        RotateCamera();
       // }
    }
}
