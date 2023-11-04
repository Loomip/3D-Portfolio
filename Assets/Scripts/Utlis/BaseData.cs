using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//string °ªÀ» enym°ªÀ¸·Î ¹Ù²ãÁÜ
public class BaseData
{
    static public T ToEnum<T>(string str)
    {
        System.Array a = System.Enum.GetValues(typeof(T));
        foreach(T item in a)
        {
            if (item.ToString() == str)
                return item;
        }
        return default(T);
    }

    static public float Rate(int value) { return value * 0.01f; }
}

public class Stat
{
    public Stat(string character)
    {
        Init(character);
    }

    private Dictionary<e_StatType, int> StatData = new Dictionary<e_StatType, int>();

    public void Init(string character)
    {
        for (int i = 1; i < (int)e_StatType.Length; ++i)
        {
            int value = DataManager.instance.GetCharacterData(character, (e_StatType)i);
            StatData.Add((e_StatType)i, value);
        }
    }

    public int GetStat(e_StatType statType)
    {
        return StatData[statType];
    }

    public void SetStat(e_StatType statType, int value)
    {
        int max = statType == e_StatType.HP ? GetStat(e_StatType.MHP) : Consts.MAX_STAT;
        StatData[statType] = Mathf.Clamp((int)value, 0, max);
    }

    public void AddStat(e_StatType statType, int value)
    {
        int max = statType == e_StatType.HP ? GetStat(e_StatType.MHP) : Consts.MAX_STAT;
        StatData[statType] += value;
        StatData[statType] = Mathf.Clamp(StatData[statType], 0, max);
    }

    public void RemoveStat(e_StatType statType, int value)
    {
        int max = statType == e_StatType.HP ? GetStat(e_StatType.MHP) : Consts.MAX_STAT;
        StatData[statType] -= value;
        StatData[statType] = Mathf.Clamp(StatData[statType], 0, max);
    }
}


