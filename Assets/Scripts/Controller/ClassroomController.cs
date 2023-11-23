using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClassroomController : MonoBehaviour
{
    public List<GameObject> effectPrefabs;  // ��ȯ ����Ʈ
    public List<GameObject> monsterPrefabs; // ����
    public GameObject doorin;               // �� ������Ʈ
    public GameObject doorOut;              // �� ������Ʈ
    public BoxCollider zone;                // ������ BoxCollider
    public float effectAppearTime = 2.0f;   // ��ȯ ����Ʈ�� ��Ÿ���� �ð�

    public Vector3 spawnAreaCenter; // ���� ���� ���� �߽� ��ġ
    public Vector3 spawnAreaSize;   // ���� ���� ���� ũ��

    public int monstersToClear = 10; // Ŭ�����ϱ� ���� �ʿ��� ���� ��

    private List<Enemy> enemyList = new List<Enemy>();

    Vector3 GetRandomSpawnPosition()
    {
        Vector3 minBound = spawnAreaCenter - spawnAreaSize / 2f;
        Vector3 maxBound = spawnAreaCenter + spawnAreaSize / 2f;

        return new Vector3
        (
        Random.Range(minBound.x, maxBound.x),
        Random.Range(minBound.y, maxBound.y),
        Random.Range(minBound.z, maxBound.z)
        );
    }

    IEnumerator SpawnEffectAndMonster()
    {
        while (GetEnemyCount() < monstersToClear)
        {
            // ���� ����Ʈ ���� �ڵ�
            GameObject selectedEffectPrefab = effectPrefabs[Random.Range(0, effectPrefabs.Count)];

            // ���� ���� ���� �ڵ�
            GameObject selectedMonsterPrefab = monsterPrefabs[Random.Range(0, monsterPrefabs.Count)];

            // ���� ���� ��ġ ���
            Vector3 randomSpawnPosition = GetRandomSpawnPosition();

            // ����Ʈ ����
            GameObject effectInstance = Instantiate(selectedEffectPrefab, randomSpawnPosition, selectedEffectPrefab.transform.rotation);

            // ���� ����
            GameObject monsterInstance = Instantiate(selectedMonsterPrefab, randomSpawnPosition, Quaternion.identity);

            Destroy(effectInstance, 5f);

            // Enemy ��ũ��Ʈ�� SetClassroomController �޼��带 ȣ���Ͽ� ClassroomController �ν��Ͻ� ����
            Enemy enemy = monsterInstance.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemyList.Add(enemy);

                enemy.SetClassroomController(this);
                Debug.Log("currentMonsterCount : " + GetEnemyCount());
            }

            if (GetEnemyCount() == monstersToClear)
            {
                StopCoroutine(AppearEffectAndSpawn());
            }

            yield return null;
        }
    }

    public void MonsterDied(Enemy _enemy)
    {
        enemyList.Remove(_enemy);

        if (GetEnemyCount() <= 0)
        {
            // ��� ���Ͱ� ����Ͽ� Ŭ���� ����

            doorin.SetActive(false); // ���� ��
            doorOut.SetActive(false); // ���� ��

            Debug.Log("���� Ŭ����!");

            // Ŭ���� �Ŀ��� �ʱ�ȭ �Ǵ� ���� ������ �����ϴ� ���� ���� �߰�
        }

        Debug.Log("DiedcurrentMonsterCount : " + GetEnemyCount());
    }

    public int GetEnemyCount() => enemyList.Count;

    IEnumerator AppearEffectAndSpawn()
    {
        float elapsedTime = 0f;
        float startScale = 0.1f;
        float targetScale = 1f;

        // ����Ʈ ��Ÿ���� �ִϸ��̼�
        while (elapsedTime < effectAppearTime)
        {
            float t = elapsedTime / effectAppearTime;
            float scale = Mathf.Lerp(startScale, targetScale, t);

            foreach (GameObject effectPrefab in effectPrefabs)
            {
                effectPrefab.transform.localScale = new Vector3(scale, scale, scale);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ���� ��ȯ ���� ���� ����
        doorin.SetActive(true);
        doorOut.SetActive(true);

        StartCoroutine(SpawnEffectAndMonster());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(AppearEffectAndSpawn());
        }

    }

    //��ȯ ���� ǥ��
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(spawnAreaCenter, spawnAreaSize);
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
