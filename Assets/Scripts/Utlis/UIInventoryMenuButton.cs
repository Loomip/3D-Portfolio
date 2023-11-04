using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using TMPro;

public class UIInventoryMenuButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI meun;

    private e_MenuType menuType = e_MenuType.None;
    private Action<e_MenuType> onClickCallback = null;

    public void InitButton(e_MenuType _menuType, Action<e_MenuType> _clickCallback = null)
    {
        menuType = _menuType;
        onClickCallback = _clickCallback;
        SetMenuButton();
    }

    private void SetMenuButton()
    {
        //번역
        switch (menuType)
        {
            case e_MenuType.None:
                meun.text = string.Empty;
                break;
            case e_MenuType.Quest:
                meun.text = DataManager.instance.GetWordData("Quest");
                break;
            case e_MenuType.Equip:
                meun.text = DataManager.instance.GetWordData("Equip");
                break;
            case e_MenuType.Collection:
                meun.text = DataManager.instance.GetWordData("Collection");
                break;
            case e_MenuType.Enhance:
                meun.text = DataManager.instance.GetWordData("Enhance");
                break;
            case e_MenuType.Option:
                meun.text = DataManager.instance.GetWordData("Option");
                break;
            case e_MenuType.Length:
                break;
        }
    }

    public void OnClick()
    {
        //?. : Null연산자 (변수사용x)
        onClickCallback?.Invoke(menuType);
    }

    public void OnSelect(bool _selected)
    {
        // TODO : _selected가 true일때 변경 처리 ~~~
    }
}
