using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : SingletonDontDestroy<UIManager>
{
    [Header("ü�¹�")]
    public GameObject hp;
    public Image player_HP;
    Dictionary<string, Image> hpList = new Dictionary<string, Image>();

    //HP ��������
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

    //Gauge ��������
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

    [Header("��ȭâ UI")]
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

    // ��ȭâ ��������
    public void Refresh_Talk(GameObject scanObj)
    {
        if (!isAction) // �̹� ��ȭâ�� �����ִٸ� �������� ����
        {
            isAction = true;
            scanObject = scanObj;
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            // ��ȭâ Ȱ��ȭ
            MessageBox.SetActive(isAction);
        }
    }

    //��ȭâ ����
    public void Close_Talk()
    {
        if (isAction)
        {
            isAction = false;
            Time.timeScale = 1;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            // ��ȭâ ��Ȱ��ȭ
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

    [Header("�κ��丮")]
    //�޴�
    public GameObject inven;

    [Header("�κ� ��ü UI")]

    public List<UIInventoryMenuButton> m_menuList = null;
    public List<GameObject> m_menu = null;

    //�⺻������ ���ִ� ����
    e_MenuType CurMenu = e_MenuType.Equip;

    //��ư�� �� ��ġ
    [SerializeField] Transform MaunButton;

    //��ư
    [SerializeField] GameObject Button;

    //�޴� �������� �� ��ġ
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


        //�޴� ����
        for (var i = e_MenuType.None + 1; i < e_MenuType.Length; ++i)
        {
            // �޴��� �̹� �����Ǿ� �ִٸ�, �ٽ� �������� ����
            if (m_menu.Count > (int)i - 1 && m_menu[(int)i - 1] != null)
            {
                continue;
            }

            string prefabPath = "Maun/" + i.ToString();
            GameObject prefab = Resources.Load<GameObject>(prefabPath);

            if (prefab == null)
            {
                Debug.Log("�޴� ���� " + i.ToString() + "�� ���� �޴� �������� �����ϴ�.");
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

            // �ν��Ͻ��� ������ ��쿡�� Ȱ��ȭ
            if (menuinstanceance != null)
            {
                menuinstanceance.SetActive(true);
            }
            else
            {
                Debug.Log("�޴� ���� " + menuType.ToString() + "�� ���� �޴� �ν��Ͻ��� ã�� �� �����ϴ�.");
            }
        }
        else
        {
            Debug.Log("�޴� ���� " + menuType.ToString() + "�� ���� �޴� �ε����� �ùٸ��� �ʽ��ϴ�.");
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
                Debug.Log("�޴� ���� " + CurMenu.ToString() + "�� ���� �޴� �ν��Ͻ��� ã�� �� �����ϴ�.");
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
        // ���� ���� �̸�
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName != "Title" /*&& sceneName != "Menu"*/)
        {
            // HP�� ������ Ȱ��ȭ
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
            // HP�� ������ ��Ȱ��ȭ
            hp.SetActive(false);
            gauge.SetActive(false);
        }
    }
}
