using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcManager : SingletonDontDestroy<NpcManager>
{
    //현재 켜진 코르틴을 저장하는 변수
    public Coroutine CurrentDialogue;

    //대화 끝을 알려주는 bool값
    public bool isDialogueFinished = false;

    //선택지가 켜졋는지 알려주는 bool값
    public bool hasSelectOptions = false;

    public IEnumerator DialogueCoroutine(NPC_Base npc)
    {
        while (npc.GetCurrentDialogIndex() < npc.dialogData.Count)
        {
            Data_Messages.Param currentDialogue = npc.dialogData[npc.GetCurrentDialogIndex()];
            UIManager.instance.nameText.text = currentDialogue.Name;
            UIManager.instance.talkText.text = currentDialogue.Text;

            //선택지가 없는 경우 사라지게 만듬
            for (int i = 0; i < 4; ++i)
            {
                UIManager.instance.choices[i].button.gameObject.SetActive(false);
                UIManager.instance.choices[i].button.onClick.RemoveAllListeners();
            }

            //선택지가 존재한다
            for (int i = 0; i < 4; ++i)
            {
                int selectCode =
                    (int)currentDialogue.GetType().GetField($"Select_Code_{i + 1}").GetValue(currentDialogue);

                if (selectCode != 0)
                {
                    UIManager.instance.choices[i].button.gameObject.SetActive(true);

                    UIManager.instance.choices[i].text.text =
                        currentDialogue.GetType().GetField($"Select_Txt_{i + 1}").GetValue(currentDialogue).ToString();

                    hasSelectOptions = true;

                    UIManager.instance.choices[i].button.onClick.AddListener(() =>
                    {
                        hasSelectOptions = false;
                        if (selectCode == -1)
                        {
                            UIManager.instance.Close_Talk();
                        }
                        else
                        {
                            //상점 열기
                            if (npc.TryGetComponent(out NPC_Shop shop))
                            {
                                UIManager.instance.Close_Talk();
                                shop.OnInteract();
                            }
                            else
                            {
                                isDialogueFinished = true;
                                // 퀘스트의 조건을 확인하는 부분
                                if (currentDialogue.Quest > 0)
                                {
                                    QuestManager.instance.StartQuest(currentDialogue.Quest);

                                    if (QuestManager.instance.IsQuestCompleted(currentDialogue.Quest))
                                    {
                                        //그퀘스트의 ID가 보상 숫자와 같은지 
                                        if(currentDialogue.Quest == currentDialogue.Reward)
                                        {
                                            QuestManager.instance.CompleteQuest(currentDialogue.Reward);
                                            // 퀘스트 완료 처리
                                            currentDialogue.Quest = 0;
                                        }
                                    }
                                }
                                npc.SetCurrentDialogIndex(selectCode);
                                //선택지가 켜지면 실행됬던 코루틴이 삭제가 되서 다시 실행 시켜줘야함
                                StartCoroutine(DialogueCoroutine(npc));
                            }
                        }
                    });
                }
            }

            //선택지가 켜지면 다음 대화로 넘어가지 않게 막음
            if (hasSelectOptions)
                yield break;

            //선택지가 클릭된 경우 currentDialogue값을 Return값으로 변경
            if (isDialogueFinished)
            {
                npc.SetCurrentDialogIndex(currentDialogue.Return);
            }

            yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Return) || Input.GetMouseButton(0));

            // 대화 데이터의 마지막 부분에 도달한 경우
            if (npc.GetCurrentDialogIndex() >= npc.dialogData.Count - 1)
            {
                UIManager.instance.Close_Talk();
                yield break;
            }
            else
            {
                //선택지 이후에 대화를 끔
                if (isDialogueFinished)
                {
                    UIManager.instance.Close_Talk();
                    isDialogueFinished = false;
                    hasSelectOptions = false;
                    yield break;
                }
                //선택지가 아닐시
                else
                {
                    npc.IncrementCurrentDialogue();
                }
            }
        }
    }
}
