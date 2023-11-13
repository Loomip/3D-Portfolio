using UnityEngine;

public class Portal : MonoBehaviour
{
    public string nextSceneName;
    public Vector3 playerPositionInNextScene;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어의 위치를 PlayerPrefs에 저장
            PlayerPrefs.SetFloat("PlayerPosX", playerPositionInNextScene.x);
            PlayerPrefs.SetFloat("PlayerPosY", playerPositionInNextScene.y);
            PlayerPrefs.SetFloat("PlayerPosZ", playerPositionInNextScene.z);

            LoadSceneManager.LoadScene(nextSceneName);
        }
    }
}
