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
            // CharacterManager 스크립트 가져오기
            CharacterManager characterManager = CharacterManager.instance;

            other.GetComponent<CharacterController>().enabled = false;

            // Player의 현재 위치 저장
            other.transform.position = characterManager.desiredPlayerPosition;

            other.GetComponent<CharacterController>().enabled = true;

            //다음씬을 불러옴
            LoadSceneManager.LoadScene(nextSceneName);
        }
    }
}
