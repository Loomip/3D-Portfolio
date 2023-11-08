using UnityEngine;

public class Portal : MonoBehaviour
{
    // 다음 씬 이름을 저장할 변수
    public string nextSceneName;

    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 오브젝트가 Player인지 확인
        if (other.CompareTag("Player"))
        { 
            //다음씬을 불러옴
            LoadSceneManager.LoadScene(nextSceneName);
        }
    }
}
