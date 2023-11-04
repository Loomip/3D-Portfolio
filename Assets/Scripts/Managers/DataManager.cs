using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataManager : SingletonDontDestroy<DataManager>
{
    //1. ĳ���� ����

    public Data_Charater data;

    //�������� ������ ĳ���� ����͵��� ��ųʸ�ȭ
    Dictionary<string, Dictionary<e_StatType, int>> CharacterData = new Dictionary<string, Dictionary<e_StatType, int>>();

    //ĳ���� �̸��� ����Ÿ���� �Ἥ ���� ��������
    public int GetCharacterData(string character, e_StatType stat)
    {
        int value = 0;
        if (CharacterData.ContainsKey(character))
        {
            value = CharacterData[character][stat];
        }
        else
        {
            Debug.LogError("������ �ҷ����� ���� : ĳ���� �̸��� �߸� �Է��߰ų� �����Ͱ� �����ϴ�.");
        }
        return value;
    }

    void InitCharacterData()
    {
        for (int i = 0; i < data.sheets[0].list.Count; ++i)
        {
            CharacterData.Add(data.sheets[0].list[i].Name, new Dictionary<e_StatType, int>());
            for (int ii = 1; ii < (int)e_StatType.Length; ++ii)
            {
                var d = Utils.GetValue(data.sheets[0].list[i], ((e_StatType)ii).ToString());

                if (null == d)
                {
                    CharacterData[data.sheets[0].list[i].Name].Add((e_StatType)ii, 0);
                    continue;
                }

                int value = (int)d;

                CharacterData[data.sheets[0].list[i].Name].Add((e_StatType)ii, value);
            }
        }
    }

    //===========================================================================================================
    //2. ������

    public Data_Item item;

    public Data_Shop shop;

    private Dictionary<int, Dictionary<e_StatType, int>> itemData = new Dictionary<int, Dictionary<e_StatType, int>>();

    public Dictionary<int, Data_Item.Param> itemDataParamDics = new Dictionary<int, Data_Item.Param>();

    private Dictionary<int, Data_Shop.Param> ShopDataParamDics = new Dictionary<int, Data_Shop.Param>();

    void InitItemData()
    {
        for (int i = 0; i < item.sheets[0].list.Count; ++i)
        {
            itemData.Add(item.sheets[0].list[i].ID, new Dictionary<e_StatType, int>());
            itemDataParamDics.Add(item.sheets[0].list[i].ID, item.sheets[0].list[i]);
            ShopDataParamDics.Add(shop.sheets[0].list[i].ID, shop.sheets[0].list[i]);

            for (int ii = 1; ii < (int)e_StatType.Length; ++ii)
            {
                var data = Utils.GetValue(item.sheets[0].list[i], ((e_StatType)ii).ToString());

                if (null == data)
                {
                    itemData[item.sheets[0].list[i].ID].Add((e_StatType)ii, 0);
                    continue;
                }

                int value = (int)data;

                itemData[item.sheets[0].list[i].ID].Add((e_StatType)ii, value);
            }
        }
    }

    public Dictionary<e_StatType, int> GetItemDataStatus(int _itemKey)
    {
        return itemData.TryGetValue(_itemKey, out var result) ? result : null;
    }

    public Data_Item.Param GetItemDataParams(int _itemKey)
    {
        return itemDataParamDics.TryGetValue(_itemKey, out var result) ? result : null;
    }

    public bool GetItemData(int _itemKey, out Data_Item.Param data)
    {
        return itemDataParamDics.TryGetValue(_itemKey, out data);
    }


    public Data_Item.Param GetRandomItemDataParams()
    {
        return itemDataParamDics.Values.OrderBy(o => Guid.NewGuid()).FirstOrDefault();
    }

    public Data_Shop.Param GetShopParams(int _itemKey)
    {
        return ShopDataParamDics.TryGetValue(_itemKey, out var result) ? result : null;
    }

    //===========================================================================================================
    //3. ������ ����

    //������ �̸��� ���� ǥ��
    public Data_Lacalize Lacalize;

    //�⺻ ������ �ѱ���
    public e_Language language = e_Language.Kor;

    //������ ���缭 �ٲ��ִ� ��ųʸ�
    Dictionary<e_Language, Dictionary<int, Data_Lacalize.Param>> lacalizeData = new Dictionary<e_Language, Dictionary<int, Data_Lacalize.Param>>();

    //event�� ���� �˾ƺ���
    public event Action OnChangeLocalizeEvent = null;

    void InitLacalizeData()
    {
        //Linq�� ���� ã�ƺ���(������)
        var dics = Lacalize.sheets.ToDictionary(k => (e_Language)Enum.Parse(typeof(e_Language), k.name), v => v.list);

        foreach (var d in dics)
        {
            lacalizeData.Add(d.Key, new Dictionary<int, Data_Lacalize.Param>());

            foreach (var l in d.Value)
            {
                lacalizeData[d.Key].Add(l.ID, l);
            }
        }
    }

    public Data_Lacalize.Param GetLocalizeData(int _itemKey)
    {
        if (lacalizeData.ContainsKey(language))
        {
            return lacalizeData[language].TryGetValue(_itemKey, out var result) ? result : null;
        }
        return null;
    }
    public Data_Lacalize.Param GetShopLocalizeData(int _itemKey)
    {
        if (lacalizeData.ContainsKey(language))
        {
            return lacalizeData[language].TryGetValue(_itemKey, out var result) ? result : null;
        }
        return null;
    }



    //==============================================================================================================================
    //4. �Ŵ� ����

    //�Ŵ� �̸�
    public Data_LaclizeWord word;

    //�Ŵ� �̸��� ���� ��ųʸ�
    Dictionary<string, Dictionary<e_Language, string>> wordLaclizeData = new Dictionary<string, Dictionary<e_Language, string>>();

    void InitwordData()
    {
        for (int i = 0; i < word.sheets[0].list.Count; ++i)
        {
            wordLaclizeData.Add(word.sheets[0].list[i].Type, new Dictionary<e_Language, string>());
            for (int ii = 0; ii < (int)e_Language.Length; ++ii)
            {
                string text = Utils.GetValue(word.sheets[0].list[i], ((e_Language)ii).ToString()).ToString();
                wordLaclizeData[word.sheets[0].list[i].Type].Add((e_Language)ii, text);
            }
        }
    }

    public string GetWordData(string type)
    {
        if (!wordLaclizeData.ContainsKey(type))
        {
            Debug.LogError("�����͸� �ҷ��� �� �����ϴ� - " + type);
            return wordLaclizeData["None"][language];
        }
        return wordLaclizeData[type][language];
    }


    public void SetLanguage(e_Language _lang)
    {
        language = _lang;
        PlayerPrefs.SetInt("Language", (int)language);
        PlayerPrefs.Save();

        OnChangeLocalizeEvent?.Invoke();
    }

    //==============================================================================================================================
    //5. ��ȭ,����Ʈ ��ũ��Ʈ

    //��ȭ�� ������ ��ųʸ�
    public Data_Messages _massages;

    Dictionary<e_Language, List<Data_Messages.Param>> DialogData = new Dictionary<e_Language, List<Data_Messages.Param>>();

    //�ߺ��� Ű�� �����ö��� ����Ʈ�� ����� <<< *** �߿� ***
    void InitDialogData()
    {
        var language = _massages.sheets.ToDictionary(key => (e_Language)Enum.Parse(typeof(e_Language), key.name), value => value.list);

        foreach (var k in language)
        {
            if (!DialogData.ContainsKey(k.Key))
            {
                DialogData.Add(k.Key, new List<Data_Messages.Param>());
            }

            foreach (var v in k.Value)
            {
                // �ߺ��Ǵ� Scene ���� �־ ��� �߰�
                DialogData[k.Key].Add(v);
            }
        }
    }

    public List<Data_Messages.Param> GetDialogData(int scene, string name)
    {
        if (DialogData.ContainsKey(language))
        {
            // DialogData �������� ���� �� �ش��ϴ� ������
            var filteredData = DialogData[language]
                // - �������� Scene�� �Ű������� ���޵� scene�� ��ġ�ϴ��� Ȯ��
                // - �������� Name�� �Ű������� ���޵� name�� ��ġ�ϴ��� Ȯ��
                .Where(entry => entry.Scene == scene && entry.Name == name)
                .ToList();

            return filteredData;
        }
        return null;
    }


    protected override void DoAwake()
    {
        InitCharacterData();
        InitItemData();
        InitLacalizeData();
        InitwordData();
        InitDialogData();
    }
}



