using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossroomController : MonoBehaviour
{
    //소환 이팩트
    public GameObject effectPrefabs;
    // 몬스터
    public GameObject bossPrefabs;
    // 문 오브젝트
    public GameObject doorin;
    // 문 오브젝트
    public GameObject doorOut;
    // 구역의 BoxCollider
    public BoxCollider zone;

    // 보스 소환 위치
    public Vector3 spawnAreaCenter;

    // 클리어하기 위해 필요한 몬스터 수
    public int monstersToClear; 


    public void MonsterDied()
    {
        // 모든 몬스터가 사망하여 클리어 상태

        doorin.SetActive(false); // 문을 염
        doorOut.SetActive(false); // 문을 염

        Debug.Log("구역 클리어!");
    }

    IEnumerator AppearEffectAndSpawn()
    { 
        //이팩트 소환
        GameObject SummonsEffect = Instantiate(effectPrefabs, spawnAreaCenter, effectPrefabs.transform.rotation);

        yield return new WaitForSeconds(1f);

        //보스 소환
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
    //몬스터 소환
    // 여기서 연출이 들어가면 되나?
    //1. 플레이어가 방에 들어갓다는 인식
    //1-1. 플레이어가 다시 방에서 못나가겟금 설정
    //1-2. 이팩트 찾아서 문이 막혓다는 인식 연출
    //2. 몬스터 소환 이팩트
    //3. 몬스터 소환 연출
    //4. 다 잡으면 다음방으로 넘어가는 카운트 UI
}
