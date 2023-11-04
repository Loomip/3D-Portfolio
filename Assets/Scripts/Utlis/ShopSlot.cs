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
    public Data_Shop.Param shopData;
    private NPC_Shop npcShop;

    public void SetShopData(Data_Shop.Param shopdata, NPC_Shop shop)
    {
        npcShop = shop;
        shopData = shopdata;

        ItemData newItem = new ItemData();
        newItem.id = DataManager.instance.GetShopParams(shopdata.ID).ID;

        txt_ItemName.text = DataManager.instance.GetShopLocalizeData(shopdata).TooltipName;
        txt_Prise.text = shopData.AddPrise.ToString();
    }

    public void OnClick()
    {
        npcShop.SetSelectedItem(shopData);
    }
}
