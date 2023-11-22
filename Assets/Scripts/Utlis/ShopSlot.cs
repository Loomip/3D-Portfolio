using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
{
    [SerializeField] Image img_Icon;
    [SerializeField] TextMeshProUGUI txt_ItemName;
    [SerializeField] TextMeshProUGUI txt_Prise;
    [SerializeField] GameObject selectedIndicator;
    public Data_Shop.Param shopData;
    private NPC_Shop npcShop;

    public void SetShopData(Data_Shop.Param shopdata, NPC_Shop shop)
    {
        npcShop = shop;
        shopData = shopdata;

        img_Icon.sprite = Resources.Load<Sprite>("Itemicons/" + shopdata.Name);
        txt_ItemName.text = DataManager.instance.GetShopLocalizeData(shopdata.ID).TooltipName;
        txt_Prise.text = shopData.AddPrise.ToString();
    }

    public void Select()
    {
        selectedIndicator.SetActive(true);
    }

    public void Deselect()
    {
        selectedIndicator.SetActive(false);
    }

    public void OnClick()
    {
        npcShop.SetSelectedItem(shopData);
    }
}
