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
            // CharacterManager ��ũ��Ʈ ��������
            CharacterManager characterManager = CharacterManager.instance;

            other.GetComponent<CharacterController>().enabled = false;

            // Player�� ���� ��ġ ����
            other.transform.position = characterManager.desiredPlayerPosition;

            other.GetComponent<CharacterController>().enabled = true;

            //�������� �ҷ���
            LoadSceneManager.LoadScene(nextSceneName);
        }
    }
}
