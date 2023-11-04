using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneManager : MonoBehaviour
{
    //가고자 하는 Scene의 이름
    public static string next_SceneName;

    //로딩창의 게이지바
    [SerializeField] Image loading_Gauge;

    //로딩 창이 아닌 다른 씬에서 다른 게임 장면으로 넘어갈 때 사용
    public static void LoadScene(string SceneName)
    {
        next_SceneName = SceneName;
        SceneManager.LoadScene("Loading");
    }

    private void Start()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return new WaitForEndOfFrame();
        // LoadSceneAsync: LoadScene 보다 정교하게 불러옴 (로딩이 느려질수 있음)
        AsyncOperation op = SceneManager.LoadSceneAsync(next_SceneName.ToString());
        //씬이 바로 넘어가지 않게
        op.allowSceneActivation = false;
        float timer = 0f;
        //우리가 만든 게 전부다 불러와 질때까지, isDone = 완벽하게 불러와지면 true
        while (!op.isDone)
        {
            yield return new WaitForEndOfFrame();
            timer += Time.deltaTime;
            //로딩중에는 시간을 멈춤
            if (op.progress < 0.9f)
            {
                loading_Gauge.fillAmount = Mathf.Lerp(loading_Gauge.fillAmount, 1f, timer);
                if (loading_Gauge.fillAmount >= op.progress)
                {
                    timer = 0f;
                }
            }
            //로딩이 끝나면 씬을 불러옴
            else
            {
                loading_Gauge.fillAmount = Mathf.Lerp(loading_Gauge.fillAmount, 1f, timer);
                if (loading_Gauge.fillAmount >= 1.0f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
