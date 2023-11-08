using UnityEngine;

public class Portal : MonoBehaviour
{
    // ���� �� �̸��� ������ ����
    public string nextSceneName;

    private void OnTriggerEnter(Collider other)
    {
        // �浹�� ������Ʈ�� Player���� Ȯ��
        if (other.CompareTag("Player"))
        { 
            //�������� �ҷ���
            LoadSceneManager.LoadScene(nextSceneName);
        }
    }
}
