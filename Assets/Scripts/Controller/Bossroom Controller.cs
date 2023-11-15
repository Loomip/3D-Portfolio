using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossroomController : MonoBehaviour
{
    //��ȯ ����Ʈ
    public GameObject effectPrefabs;
    // ����
    public GameObject bossPrefabs;
    // �� ������Ʈ
    public GameObject doorin;
    // �� ������Ʈ
    public GameObject doorOut;
    // ������ BoxCollider
    public BoxCollider zone;

    // ���� ��ȯ ��ġ
    public Vector3 spawnAreaCenter;

    // ������ ���� ������ �Ǵ��ϴ� �÷���
    public bool isFinalBossRoom = false;


    public void MonsterDied()
    {
        // ��� ���Ͱ� ����Ͽ� Ŭ���� ����

        doorin.SetActive(false); // ���� ��
        doorOut.SetActive(false); // ���� ��

        // ������ ���� ���� ��쿡�� Ư���� ó���� �մϴ�.
        if (isFinalBossRoom)
        {
            UIManager.instance.ShowBossDiedScreen(); // UIManager�� �޼��� ȣ��
        }

        Debug.Log("���� Ŭ����!");
    }

    IEnumerator AppearEffectAndSpawn()
    { 
        //����Ʈ ��ȯ
        GameObject SummonsEffect = Instantiate(effectPrefabs, spawnAreaCenter, effectPrefabs.transform.rotation);

        yield return new WaitForSeconds(1f);

        //���� ��ȯ
        GameObject BossSummons = Instantiate(bossPrefabs, spawnAreaCenter, Quaternion.Euler(0, -90, 0));

        doorin.SetActive(true);
        doorOut.SetActive(true);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(AppearEffectAndSpawn());
        }
    }
    //���� ��ȯ
    // ���⼭ ������ ���� �ǳ�?
    //1. �÷��̾ �濡 ���ٴ� �ν�
    //1-1. �÷��̾ �ٽ� �濡�� �������ٱ� ����
    //1-2. ����Ʈ ã�Ƽ� ���� �����ٴ� �ν� ����
    //2. ���� ��ȯ ����Ʈ
    //3. ���� ��ȯ ����
    //4. �� ������ ���������� �Ѿ�� ī��Ʈ UI
}
