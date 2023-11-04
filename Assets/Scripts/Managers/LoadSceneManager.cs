using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneManager : MonoBehaviour
{
    //������ �ϴ� Scene�� �̸�
    public static string next_SceneName;

    //�ε�â�� ��������
    [SerializeField] Image loading_Gauge;

    //�ε� â�� �ƴ� �ٸ� ������ �ٸ� ���� ������� �Ѿ �� ���
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
        // LoadSceneAsync: LoadScene ���� �����ϰ� �ҷ��� (�ε��� �������� ����)
        AsyncOperation op = SceneManager.LoadSceneAsync(next_SceneName.ToString());
        //���� �ٷ� �Ѿ�� �ʰ�
        op.allowSceneActivation = false;
        float timer = 0f;
        //�츮�� ���� �� ���δ� �ҷ��� ��������, isDone = �Ϻ��ϰ� �ҷ������� true
        while (!op.isDone)
        {
            yield return new WaitForEndOfFrame();
            timer += Time.deltaTime;
            //�ε��߿��� �ð��� ����
            if (op.progress < 0.9f)
            {
                loading_Gauge.fillAmount = Mathf.Lerp(loading_Gauge.fillAmount, 1f, timer);
                if (loading_Gauge.fillAmount >= op.progress)
                {
                    timer = 0f;
                }
            }
            //�ε��� ������ ���� �ҷ���
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
