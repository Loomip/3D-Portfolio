using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataManager : SingletonDontDestroy<DataManager>
{
    //1. 캐릭터 스텟

    public Data_Charater data;

    //엑셀에서 가져온 캐릭터 대아터들을 딕셔너리화
    Dictionary<string, Dictionary<e_StatType, int>> CharacterData = new Dictionary<string, Dictionary<e_StatType, int>>();

    //캐릭터 이름과 스텟타입을 써서 정보 가져오기
    public int GetCharacterData(string character, e_StatType stat)
    {
        int value = 0;
        if (CharacterData.ContainsKey(character))
        {
            value = CharacterData[character][stat];
        }
        else
        {
            Debug.LogError("데이터 불러오기 실패 : 캐릭터 이름을 잘못 입력했거나 데이터가 없습니다.");
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
    //2. 아이템

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
    //3. 아이템 정보

    //아이템 이름과 설명 표시
    public Data_Lacalize Lacalize;

    //기본 설정은 한국어
    public e_Language language = e_Language.Kor;

    //설정에 맞춰서 바꿔주는 딕셔너리
    Dictionary<e_Language, Dictionary<int, Data_Lacalize.Param>> lacalizeData = new Dictionary<e_Language, Dictionary<int, Data_Lacalize.Param>>();

    //event에 대해 알아보기
    public event Action OnChangeLocalizeEvent = null;

    void InitLacalizeData()
    {
        //Linq에 대해 찾아볼껏(쿼리문)
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
    //4. 매뉴 정보

    //매뉴 이름
    public Data_LaclizeWord word;

    //매뉴 이름에 따른 딕셔너리
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
            Debug.LogError("데이터를 불러올 수 없습니다 - " + type);
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
    //5. 대화,퀘스트 스크립트

    //대화를 저장할 딕셔너리
    public Data_Messages _massages;

    Dictionary<e_Language, List<Data_Messages.Param>> DialogData = new Dictionary<e_Language, List<Data_Messages.Param>>();

    //중복된 키를 가져올때는 리스트를 써야함 <<< *** 중요 ***
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
                // 중복되는 Scene 값이 있어도 모두 추가
                DialogData[k.Key].Add(v);
            }
        }
    }

    public List<Data_Messages.Param> GetDialogData(int scene, string name)
    {
        if (DialogData.ContainsKey(language))
        {
            // DialogData 사전에서 현재 언어에 해당하는 데이터
            var filteredData = DialogData[language]
                // - 데이터의 Scene과 매개변수로 전달된 scene과 일치하는지 확인
                // - 데이터의 Name이 매개변수로 전달된 name과 일치하는지 확인
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



