using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClassroomController : MonoBehaviour
{
    public List<GameObject> effectPrefabs;  // 소환 이팩트
    public List<GameObject> monsterPrefabs; // 몬스터
    public GameObject doorin;               // 문 오브젝트
    public GameObject doorOut;              // 문 오브젝트
    public BoxCollider zone;                // 구역의 BoxCollider
    public float effectAppearTime = 2.0f;   // 소환 이팩트가 나타나는 시간

    public Vector3 spawnAreaCenter; // 몬스터 스폰 영역 중심 위치
    public Vector3 spawnAreaSize;   // 몬스터 스폰 영역 크기

    public int monstersToClear = 10; // 클리어하기 위해 필요한 몬스터 수

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
            // 랜덤 이팩트 선택 코드
            GameObject selectedEffectPrefab = effectPrefabs[Random.Range(0, effectPrefabs.Count)];

            // 랜덤 몬스터 선택 코드
            GameObject selectedMonsterPrefab = monsterPrefabs[Random.Range(0, monsterPrefabs.Count)];

            // 몬스터 스폰 위치 계산
            Vector3 randomSpawnPosition = GetRandomSpawnPosition();

            // 이팩트 생성
            GameObject effectInstance = Instantiate(selectedEffectPrefab, randomSpawnPosition, selectedEffectPrefab.transform.rotation);

            // 몬스터 생성
            GameObject monsterInstance = Instantiate(selectedMonsterPrefab, randomSpawnPosition, Quaternion.identity);

            Destroy(effectInstance, 5f);

            // Enemy 스크립트의 SetClassroomController 메서드를 호출하여 ClassroomController 인스턴스 전달
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
            // 모든 몬스터가 사망하여 클리어 상태

            doorin.SetActive(false); // 문을 염
            doorOut.SetActive(false); // 문을 염

            Debug.Log("구역 클리어!");

            // 클리어 후에는 초기화 또는 다음 동작을 수행하는 등의 로직 추가
        }

        Debug.Log("DiedcurrentMonsterCount : " + GetEnemyCount());
    }

    public int GetEnemyCount() => enemyList.Count;

    IEnumerator AppearEffectAndSpawn()
    {
        float elapsedTime = 0f;
        float startScale = 0.1f;
        float targetScale = 1f;

        // 이팩트 나타나는 애니메이션
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

        // 몬스터 소환 등의 로직 실행
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

    //소환 영역 표시
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(spawnAreaCenter, spawnAreaSize);
    }

    //몬스터 소환
    // 여기서 연출이 들어가면 되나?
    //1. 플레이어가 방에 들어갓다는 인식
    //1-1. 플레이어가 다시 방에서 못나가겟금 설정
    //1-2. 이팩트 찾아서 문이 막혓다는 인식 연출
    //2. 몬스터 소환 이팩트
    //3. 몬스터 소환 연출
    //4. 다 잡으면 다음방으로 넘어가는 카운트 UI
}
