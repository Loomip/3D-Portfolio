using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : SingletonDontDestroy<UIManager>
{
    [Header("체력바")]
    public GameObject hp;
    public Image player_HP;
    Dictionary<string, Image> hpList = new Dictionary<string, Image>();

    //HP 리프레쉬
    public void Refresh_HP(Player player)
    {
        Image img = player.Name == "Player" ? player_HP : null;
        if (img == null)
        {
            if (hpList.ContainsKey(player.Name))
            {
                img = hpList[player.Name];
            }
        }
        img.fillAmount = player.CurHealth / (float)player.MaxHealth;
    }

    //=========================================================================================================

    [Header("Gauge")]
    public GameObject gauge;
    public Image player_Gauge;
    public TextMeshProUGUI gaugeText;
    Dictionary<string, Image> gaugeList = new Dictionary<string, Image>();

    //Gauge 리프레쉬
    public void Refresh_Gauge(Player player)
    {
        Image img = player.Name == "Player" ? player_Gauge : null;

        if (img != null)
        {
            if (gaugeList.ContainsKey(player.Name))
            {
                img = gaugeList[player.Name];
            }
        }
        if (player.gaugeCount < 0)
        {
            player.gaugeCount = 0;
        }

        img.fillAmount = player.Gauge / (float)player.MGauge;
        gaugeText.text = player.gaugeCount.ToString();
    }

    //=========================================================================================================

    [Header("대화창 UI")]
    public GameObject InteractText;
    public Player player;
    public GameObject MessageBox;
    public TextMeshProUGUI talkText;
    public TextMeshProUGUI nameText;
    public bool isAction;
    private GameObject scanObject;

    [System.Serializable]
    public class ChoiceUI
    {
        public Button button;
        public TextMeshProUGUI text;
    }
    public ChoiceUI[] choices;

    // 대화창 리프레쉬
    public void Refresh_Talk(GameObject scanObj)
    {
        if (!isAction) // 이미 대화창이 열려있다면 종료하지 않음
        {
            isAction = true;
            scanObject = scanObj;
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            // 대화창 활성화
            MessageBox.SetActive(isAction);
        }
    }

    //대화창 끄기
    public void Close_Talk()
    {
        if (isAction)
        {
            isAction = false;
            Time.timeScale = 1;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            // 대화창 비활성화
            MessageBox.SetActive(isAction);
        }
    }

    void InteractableObject()
    {
        if (player != null)
        {
            if (player.GetInterctableObject() != null)
                InteractableShow();
            else
                InteractableHide();
        }
    }

    private void InteractableShow()
    {
        InteractText.SetActive(true);
    }
    private void InteractableHide()
    {
        InteractText.SetActive(false);
    }

    //===========================================================================================================================

    [Header("인벤토리")]
    //메뉴
    public GameObject inven;

    [Header("인벤 전체 UI")]

    public List<UIInventoryMenuButton> m_menuList = null;
    public List<GameObject> m_menu = null;

    //기본적으로 켜있는 슬롯
    e_MenuType CurMenu = e_MenuType.Equip;

    //버튼이 들어갈 위치
    [SerializeField] Transform MaunButton;

    //버튼
    [SerializeField] GameObject Button;

    //메뉴 프리펩이 들어갈 위치
    [SerializeField] Transform Maun;

    public void InvenShow()
    {
        Initialize();
    }

    private void Initialize()
    {
        InitializeMenuButton();
        InitializeMenuParent(CurMenu);
    }
    private void InitializeMenuButton()
    {
        if (m_menuList.Count <= 0)
        {
            for (var i = e_MenuType.None + 1; i < e_MenuType.Length; ++i)
            {
                UIInventoryMenuButton b = Instantiate(Button, MaunButton).GetComponent<UIInventoryMenuButton>();
                m_menuList.Add(b);
            }
        }

        for (var i = e_MenuType.None + 1; i < e_MenuType.Length; ++i)
        {
            var index = (int)i - 1;
            m_menuList[index].InitButton(i, OnClickButtonCallback);
            m_menuList[index].OnSelect(i == CurMenu);
        }
    }

    private void InitializeMenuParent(e_MenuType menuType)
    {
        SetActiveAll(false);

        if (m_menu == null)
        {
            m_menu = new List<GameObject>();
        }


        //메뉴 생성
        for (var i = e_MenuType.None + 1; i < e_MenuType.Length; ++i)
        {
            // 메뉴가 이미 생성되어 있다면, 다시 생성하지 않음
            if (m_menu.Count > (int)i - 1 && m_menu[(int)i - 1] != null)
            {
                continue;
            }

            string prefabPath = "Maun/" + i.ToString();
            GameObject prefab = Resources.Load<GameObject>(prefabPath);

            if (prefab == null)
            {
                Debug.Log("메뉴 유형 " + i.ToString() + "에 대한 메뉴 프리팹이 없습니다.");
            }
            else
            {
                GameObject instanceance = Instantiate(prefab, Maun);
                m_menu.Add(instanceance);
                instanceance.SetActive(false);

                if (i == e_MenuType.Enhance)
                {
                    Enhance enhance = instanceance.GetComponent<Enhance>();
                    enhance.Initialize();
                }
            }
        }

        int menuIndex = (int)menuType - 1;

        if (menuIndex >= 0 && menuIndex < m_menu.Count)
        {
            GameObject menuinstanceance = m_menu[menuIndex];

            // 인스턴스가 존재할 경우에만 활성화
            if (menuinstanceance != null)
            {
                menuinstanceance.SetActive(true);
            }
            else
            {
                Debug.Log("메뉴 유형 " + menuType.ToString() + "에 대한 메뉴 인스턴스를 찾을 수 없습니다.");
            }
        }
        else
        {
            Debug.Log("메뉴 유형 " + menuType.ToString() + "에 대한 메뉴 인덱스가 올바르지 않습니다.");
        }
    }

    public void OnClickButtonCallback(e_MenuType menuType)
    {
        CurMenu = menuType;
        Initialize();
    }

    private void SetActiveAll(bool enable)
    {
        if (m_menu != null)
        {
            for (int i = m_menu.Count - 1; i >= 0; i--)
            {
                if (m_menu[i] != null)
                {
                    m_menu[i].SetActive(enable);
                }
                else
                {
                    m_menu.RemoveAt(i);
                }
            }
        }

        if (CurMenu != e_MenuType.None)
        {
            int menuIndex = (int)CurMenu;
            if (menuIndex >= 0 && menuIndex < m_menu.Count)
            {
                GameObject menuinstanceance = m_menu[menuIndex];
                menuinstanceance.SetActive(enable);
            }
            else
            {
                Debug.Log("메뉴 유형 " + CurMenu.ToString() + "에 대한 메뉴 인스턴스를 찾을 수 없습니다.");
            }
        }
    }

    public void SetActive(e_MenuType type, bool enable)
    {
        if (type == CurMenu)
        {
            GameObject menuinstanceance = m_menu[(int)type];
            menuinstanceance.SetActive(enable);
        }
    }

    protected override void DoAwake()
    {
        player = FindObjectOfType<Player>();
        inven.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // 현재 씬의 이름
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName != "Title" /*&& sceneName != "Menu"*/)
        {
            // HP와 게이지 활성화
            hp.SetActive(true);
            gauge.SetActive(true);

            InteractableObject();

            if (Input.GetKeyDown(KeyCode.I))
            {
                inven.SetActive(!inven.activeInHierarchy);

                var isShow = inven.activeInHierarchy;

                Cursor.lockState = isShow ? CursorLockMode.None : CursorLockMode.Locked;

                Time.timeScale = isShow ? 0f : 1f;

                if (isShow) InvenShow();
            }
        }
        else
        {
            // HP와 게이지 비활성화
            hp.SetActive(false);
            gauge.SetActive(false);
        }
    }
}
